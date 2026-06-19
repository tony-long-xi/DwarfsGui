using DwarfsGui.Models;
using DwarfsGui.Services;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DwarfsGui
{
    public partial class FrmDwarfs : Form
    {
        private readonly DwarfsService _dwarfsService;
        private readonly SettingsService _settingsService;
        private DwarfsSettings _settings;
        private CancellationTokenSource? _cts = null;

        public FrmDwarfs()
        {
            _dwarfsService = new DwarfsService();
            _settingsService = new SettingsService();
            _settings = _settingsService.Load();

            InitializeComponent();
            LoadSettings();
        }

        private void BrowseFolder(TextBox target)
        {
            using var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                target.Text = dlg.SelectedPath;
        }

        private void BrowseFile(TextBox target, string filter)
        {
            using var dlg = new OpenFileDialog { Filter = filter };
            if (dlg.ShowDialog() == DialogResult.OK)
                target.Text = dlg.FileName;
        }

        private void BrowseSaveFile(TextBox target, string filter)
        {
            using var dlg = new SaveFileDialog { Filter = filter };
            if (dlg.ShowDialog() == DialogResult.OK)
                target.Text = dlg.FileName;
        }

        private void btnBrowseCreateInput_Click(object sender, EventArgs e)
        {
            BrowseFolder(txtCreateInput);
        }

        private void btnBrowseCreateOutput_Click(object sender, EventArgs e)
        {
            BrowseSaveFile(txtCreateOutput, "Dwarfs 镜像|*.dwarfs|所有文件|*.*");
        }


        private void LoadSettings()
        {
            nudCompressionLevel.Value = _settings.CompressionLevel;
            nudWorkers.Value = _settings.Workers;
            chkForce.Checked = _settings.Force;
            txtCacheSize.Text = _settings.CacheSize;
            txtReadAhead.Text = _settings.ReadAhead;
            nudMountWorkers.Value = _settings.MountWorkers;
            nudExtractWorkers.Value = _settings.ExtractWorkers;
            txtExtractCacheSize.Text = _settings.ExtractCacheSize;
            chkContinueOnError.Checked = _settings.ContinueOnError;
        }

        private void SaveSettings()
        {
            _settings.CompressionLevel = (int)nudCompressionLevel.Value;
            _settings.Workers = (int)nudWorkers.Value;
            _settings.Force = chkForce.Checked;
            _settings.CacheSize = txtCacheSize.Text;
            _settings.ReadAhead = txtReadAhead.Text;
            _settings.MountWorkers = (int)nudMountWorkers.Value;
            _settings.ExtractWorkers = (int)nudExtractWorkers.Value;
            _settings.ExtractCacheSize = txtExtractCacheSize.Text;
            _settings.ContinueOnError = chkContinueOnError.Checked;
            _settingsService.Save(_settings);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            _dwarfsService.UnmountAll();
            base.OnFormClosing(e);
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(() => Log(message));
                return;
            }
            lstLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
            //lstLog.ScrollToCaret();
        }


        private async void btnCreate_Click(object sender, EventArgs e)
        {
            var input = txtCreateInput.Text.Trim();
            var output = txtCreateOutput.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("请选择输入路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(output))
            {
                MessageBox.Show("请指定输出文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveSettings();
            btnCreate.Enabled = false;
            progressCreate.Visible = true;
            Log($"开始制作镜像: {input} -> {output}");

            _cts = new CancellationTokenSource();
            var (success, output_text) = await _dwarfsService.CreateImageAsync(input, output, _settings, Log, _cts.Token);

            btnCreate.Enabled = true;
            progressCreate.Visible = false;

            if (success)
            {
                Log("镜像制作完成！");
                MessageBox.Show("镜像制作完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log($"制作失败: {output_text}");
                MessageBox.Show("镜像制作失败，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnmount_Click(object sender, EventArgs e)
        {
            if (lvMounted.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先选择要卸载的镜像。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (ListViewItem item in lvMounted.SelectedItems)
            {
                var mi = item.Tag as MountInfo;
                if (mi != null)
                {
                    _dwarfsService.Unmount(mi.MountPoint);
                    _dwarfsService.MountedImages.Remove(mi);
                    Log($"已卸载: {mi.MountPoint}");
                }
            }
            RefreshMountedList();
        }

        private void btnOpenMountPoint_Click(object sender, EventArgs e)
        {
            if (lvMounted.SelectedItems.Count > 0)
            {
                var mi = lvMounted.SelectedItems[0].Tag as MountInfo;
                if (mi != null && Directory.Exists(mi.MountPoint))
                {
                    Process.Start("explorer.exe", mi.MountPoint);
                }
            }
        }

        private void btnBrowseMountImage_Click(object sender, EventArgs e)
        {
            BrowseFile(txtMountImage, "Dwarfs 镜像|*.dwarfs|所有文件|*.*");
        }

        private void btnBrowseMountPoint_Click(object sender, EventArgs e)
        {
            BrowseFolder(txtMountPoint);
        }

        private async void btnMount_Click(object sender, EventArgs e)
        {
            var image = txtMountImage.Text.Trim();
            var mountPoint = txtMountPoint.Text.Trim();

            if (string.IsNullOrEmpty(image))
            {
                MessageBox.Show("请选择镜像文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!System.IO.File.Exists(image))
            {
                MessageBox.Show("镜像文件不存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveSettings();
            btnMount.Enabled = false;
            progressMount.Visible = true;
            Log($"开始挂载镜像: {image}");

            _cts = new CancellationTokenSource();
            var (success, output_text, process) = await _dwarfsService.MountImageAsync(
                image, string.IsNullOrEmpty(mountPoint) ? null : mountPoint, _settings, Log, _cts.Token);

            btnMount.Enabled = true;
            progressMount.Visible = false;

            if (success && process != null)
            {
                Log("镜像挂载完成！");
                var mp = mountPoint;
                if (string.IsNullOrEmpty(mp))
                    mp = ExtractMountPoint(output_text);
                if (string.IsNullOrEmpty(mp))
                    mp = "auto";

                _dwarfsService.RegisterMount(mp, process);
                _dwarfsService.MountedImages.Add(new MountInfo
                {
                    ImagePath = image,
                    MountPoint = mp,
                    MountTime = DateTime.Now
                });
                RefreshMountedList();
                MessageBox.Show($"镜像挂载完成！\n挂载点: {mp}", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log($"挂载失败: {output_text}");
                MessageBox.Show("镜像挂载失败，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string ExtractMountPoint(string output)
        {
            foreach (var line in output.Split('\n'))
            {
                var trimmed = line.Trim();
                if (trimmed.Contains("mounted") || trimmed.Contains("Mount point"))
                {
                    var parts = trimmed.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        var candidate = parts[^1].Trim('"', '\'');
                        if (Directory.Exists(candidate) || candidate.Contains(':'))
                            return candidate;
                    }
                }
            }
            foreach (var line in output.Split('\n'))
            {
                var trimmed = line.Trim();
                if (trimmed.Length >= 2 && trimmed[1] == ':')
                    return trimmed;
            }
            return "";
        }

        private void RefreshMountedList()
        {
            lvMounted.Items.Clear();
            foreach (var mi in _dwarfsService.MountedImages)
            {
                var item = new ListViewItem(mi.ImagePath);
                item.SubItems.Add(mi.MountPoint);
                item.SubItems.Add(mi.MountTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.Tag = mi;
                lvMounted.Items.Add(item);
            }
        }

        private void btnBrowseExtractInput_Click(object sender, EventArgs e)
        {
            BrowseFile(txtExtractInput, "Dwarfs 镜像|*.dwarfs|所有文件|*.*");
        }

        private void btnBrowseExtractOutput_Click(object sender, EventArgs e)
        {
            BrowseFolder(txtExtractOutput);
        }

        private async void btnExtract_Click(object sender, EventArgs e)
        {
            var input = txtExtractInput.Text.Trim();
            var output = txtExtractOutput.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("请选择镜像文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(output))
            {
                MessageBox.Show("请指定输出目录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveSettings();
            btnExtract.Enabled = false;
            progressExtract.Visible = true;
            Log($"开始提取镜像: {input} -> {output}");

            _cts = new CancellationTokenSource();
            var (success, output_text) = await _dwarfsService.ExtractImageAsync(input, output, _settings, Log, _cts.Token);

            btnExtract.Enabled = true;
            progressExtract.Visible = false;

            if (success)
            {
                Log("镜像提取完成！");
                MessageBox.Show("镜像提取完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log($"提取失败: {output_text}");
                MessageBox.Show("镜像提取失败，请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegisterContextMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var exePath = Application.ExecutablePath;

                // 文件右键菜单 - 二级子菜单结构
                using var fileKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\*\shell\DwarfsGUI");
                fileKey.SetValue("MUIVerb", "Dwarfs GUI");
                fileKey.SetValue("Icon", $"\"{exePath}\",0");
                fileKey.SetValue("SubCommands", "01-make;02-mount;03-extract");

                using var mkKey = fileKey.CreateSubKey("01-make");
                mkKey.SetValue("MUIVerb", "制作镜像");
                using var mkCmd = mkKey.CreateSubKey("command");
                mkCmd.SetValue("", $"\"{exePath}\" --create \"%1\"");

                using var mountKey = fileKey.CreateSubKey("02-mount");
                mountKey.SetValue("MUIVerb", "加载镜像");
                using var mountCmd = mountKey.CreateSubKey("command");
                mountCmd.SetValue("", $"\"{exePath}\" --mount \"%1\"");

                using var extractKey = fileKey.CreateSubKey("03-extract");
                extractKey.SetValue("MUIVerb", "提取镜像");
                using var extractCmd = extractKey.CreateSubKey("command");
                extractCmd.SetValue("", $"\"{exePath}\" --extract \"%1\"");

                // 文件夹右键菜单
                using var folderKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\Directory\shell\DwarfsGUI");
                folderKey.SetValue("MUIVerb", "Dwarfs GUI");
                folderKey.SetValue("Icon", $"\"{exePath}\",0");
                folderKey.SetValue("SubCommands", "01-make;02-mount;03-extract");

                using var fMkKey = folderKey.CreateSubKey("01-make");
                fMkKey.SetValue("MUIVerb", "制作镜像");
                using var fMkCmd = fMkKey.CreateSubKey("command");
                fMkCmd.SetValue("", $"\"{exePath}\" --create \"%1\"");

                using var fMountKey = folderKey.CreateSubKey("02-mount");
                fMountKey.SetValue("MUIVerb", "加载镜像");
                using var fMountCmd = fMountKey.CreateSubKey("command");
                fMountCmd.SetValue("", $"\"{exePath}\" --mount \"%1\"");

                using var fExtractKey = folderKey.CreateSubKey("03-extract");
                fExtractKey.SetValue("MUIVerb", "提取镜像");
                using var fExtractCmd = fExtractKey.CreateSubKey("command");
                fExtractCmd.SetValue("", $"\"{exePath}\" --extract \"%1\"");

                // 文件夹背景右键菜单
                using var bgKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI");
                bgKey.SetValue("MUIVerb", "Dwarfs GUI");
                bgKey.SetValue("Icon", $"\"{exePath}\",0");
                bgKey.SetValue("SubCommands", "01-make;02-mount;03-extract");

                using var bgMkKey = bgKey.CreateSubKey("01-make");
                bgMkKey.SetValue("MUIVerb", "制作镜像");
                using var bgMkCmd = bgMkKey.CreateSubKey("command");
                bgMkCmd.SetValue("", $"\"{exePath}\" --create \"%V\"");

                using var bgMountKey = bgKey.CreateSubKey("02-mount");
                bgMountKey.SetValue("MUIVerb", "加载镜像");
                using var bgMountCmd = bgMountKey.CreateSubKey("command");
                bgMountCmd.SetValue("", $"\"{exePath}\" --mount \"%V\"");

                using var bgExtractKey = bgKey.CreateSubKey("03-extract");
                bgExtractKey.SetValue("MUIVerb", "提取镜像");
                using var bgExtractCmd = bgExtractKey.CreateSubKey("command");
                bgExtractCmd.SetValue("", $"\"{exePath}\" --extract \"%V\"");

                lblContextMenuStatus.Text = "右键菜单已注册（含二级子菜单）";
                lblContextMenuStatus.ForeColor = Color.Green;
                Log("右键菜单注册成功（二级子菜单：制作镜像/加载镜像/提取镜像）");
                Log("提示：可能需要重启资源管理器或重新登录才能生效");
            }
            catch (Exception ex)
            {
                lblContextMenuStatus.Text = $"注册失败: {ex.Message}";
                lblContextMenuStatus.ForeColor = Color.Red;
                Log($"右键菜单注册失败: {ex.Message}");
            }
        }

        private void btnUnregisterContextMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\*\shell\DwarfsGUI", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\shell\DwarfsGUI", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI", false);

                lblContextMenuStatus.Text = "右键菜单已取消";
                lblContextMenuStatus.ForeColor = Color.DimGray;
                Log("右键菜单已取消注册");
            }
            catch (Exception ex)
            {
                lblContextMenuStatus.Text = $"取消失败: {ex.Message}";
                lblContextMenuStatus.ForeColor = Color.Red;
                Log($"取消右键菜单失败: {ex.Message}");
            }
        }


        public void ProcessCommandLineArgs(string[] args)
        {
            if (args.Length == 0) return;

            if (args.Length >= 2 && args[0].StartsWith("--"))
            {
                var action = args[0].ToLower();
                var path = args[1];

                switch (action)
                {
                    case "--create":
                        tabControl.SelectedTab = tabCreate;
                        txtCreateInput.Text = path;
                        break;
                    case "--mount":
                        tabControl.SelectedTab = tabMount;
                        txtMountImage.Text = path;
                        break;
                    case "--extract":
                        tabControl.SelectedTab = tabExtract;
                        txtExtractInput.Text = path;
                        break;
                }
                return;
            }

            var argPath = args[0];
            if (Directory.Exists(argPath))
            {
                tabControl.SelectedTab = tabCreate;
                txtCreateInput.Text = argPath;
            }
            else if (System.IO.File.Exists(argPath) && Path.GetExtension(argPath).Equals(".dwarfs", StringComparison.OrdinalIgnoreCase))
            {
                tabControl.SelectedTab = tabMount;
                txtMountImage.Text = argPath;
            }
        }
    }


}
