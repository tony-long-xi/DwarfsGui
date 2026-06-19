using DwarfsGui.Models;
using DwarfsGui.Services;
using System.Diagnostics;


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
            // 弹出选择：文件或文件夹
            using var choiceDlg = new Form
            {
                Text = "选择输入",
                Size = new Size(320, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label { Text = "请选择输入类型：", Location = new Point(20, 20), AutoSize = true };
            var btnFile = new Button { Text = "选择文件", Location = new Point(30, 55), Size = new Size(100, 35) };
            var btnFolder = new Button { Text = "选择文件夹", Location = new Point(150, 55), Size = new Size(120, 35) };

            btnFile.Click += (_, _) => { choiceDlg.DialogResult = DialogResult.Yes; choiceDlg.Close(); };
            btnFolder.Click += (_, _) => { choiceDlg.DialogResult = DialogResult.No; choiceDlg.Close(); };
            choiceDlg.CancelButton = btnFolder;

            choiceDlg.Controls.AddRange(new Control[] { lbl, btnFile, btnFolder });

            var result = choiceDlg.ShowDialog();

            if (result == DialogResult.Yes)
            {
                // 选择文件
                using var dlg = new OpenFileDialog
                {
                    Title = "选择要制作镜像的文件",
                    Filter = "所有文件|*.*"
                };
                var currentPath = txtCreateInput.Text.Trim();
                if (!string.IsNullOrEmpty(currentPath) && System.IO.File.Exists(currentPath))
                {
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(currentPath);
                    dlg.FileName = System.IO.Path.GetFileName(currentPath);
                }
                else if (!string.IsNullOrEmpty(currentPath) && System.IO.Directory.Exists(currentPath))
                {
                    dlg.InitialDirectory = currentPath;
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtCreateInput.Text = dlg.FileName;
            }
            else if (result == DialogResult.No)
            {
                // 选择文件夹
                using var dlg = new FolderBrowserDialog
                {
                    Description = "选择要制作镜像的文件夹",
                    UseDescriptionForTitle = true,
                    ShowNewFolderButton = false
                };
                var currentPath = txtCreateInput.Text.Trim();
                if (!string.IsNullOrEmpty(currentPath))
                {
                    if (System.IO.Directory.Exists(currentPath))
                        dlg.SelectedPath = currentPath;
                    else if (System.IO.File.Exists(currentPath))
                    {
                        var dirPath = System.IO.Path.GetDirectoryName(currentPath);
                        if (!string.IsNullOrEmpty(dirPath))
                            dlg.SelectedPath = dirPath;
                    }
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtCreateInput.Text = dlg.SelectedPath;
            }
        }

        private void btnBrowseCreateOutput_Click(object sender, EventArgs e)
        {
            BrowseSaveFile(txtCreateOutput, "Dwarfs 镜像|*.dwarfs|所有文件|*.*");
        }


        private void LoadSettings()
        {
            cmbCompressionLevel.SelectedIndex = Math.Min(_settings.CompressionLevel, 7);
            nudWorkers.Value = _settings.Workers;
            chkForce.Checked = _settings.Force;
            txtCacheSize.Text = _settings.CacheSize;
            txtReadAhead.Text = _settings.ReadAhead;
            nudMountWorkers.Value = _settings.MountWorkers;
            nudExtractWorkers.Value = _settings.ExtractWorkers;
            txtExtractCacheSize.Text = _settings.ExtractCacheSize;
            chkContinueOnError.Checked = _settings.ContinueOnError;
            txtWinFspPath.Text = _settings.WinFspPath;
            _dwarfsService.WinFspPath = _settings.WinFspPath;
            chkDisableDedup.Checked = _settings.DisableDedup;
            cmbCompressionAlgo.Text = _settings.CompressionAlgorithm;
        }

        private void SaveSettings()
        {
            _settings.CompressionLevel = cmbCompressionLevel.SelectedIndex;
            _settings.Workers = (int)nudWorkers.Value;
            _settings.Force = chkForce.Checked;
            _settings.CacheSize = txtCacheSize.Text;
            _settings.ReadAhead = txtReadAhead.Text;
            _settings.MountWorkers = (int)nudMountWorkers.Value;
            _settings.ExtractWorkers = (int)nudExtractWorkers.Value;
            _settings.ExtractCacheSize = txtExtractCacheSize.Text;
            _settings.ContinueOnError = chkContinueOnError.Checked;
            _settings.WinFspPath = txtWinFspPath.Text;
            _dwarfsService.WinFspPath = txtWinFspPath.Text;
            _settings.DisableDedup = chkDisableDedup.Checked;
            _settings.CompressionAlgorithm = cmbCompressionAlgo.Text == "(默认)" ? "" : cmbCompressionAlgo.Text;
            _settingsService.Save(_settings);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();

            // 如果有挂载记录，提示用户是否清除全部挂载
            if (lvMounted.Items.Count > 0)
            {
                var result = MessageBox.Show(
                    $"当前有 {lvMounted.Items.Count} 个镜像正在挂载中，是否清除所有挂载并退出？",
                    "提示",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _dwarfsService.UnmountAll();
                }
                //else
                //{
                //    e.Cancel = true;
                //    return;
                //}
            }

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

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 切换到挂载镜像 tab 时刷新已挂载列表
            if (tabControl.SelectedTab == tabMount)
            {
                RefreshMountedList();
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
                    // 如果有 PID，直接通过 PID 卸载（支持外部进程）
                    if (mi.ProcessId > 0)
                        _dwarfsService.UnmountByPid(mi.ProcessId);
                    else
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
                if (mi != null && !string.IsNullOrEmpty(mi.MountPoint))
                {
                    var mountPoint = mi.MountPoint;
                    if (Directory.Exists(mountPoint))
                    {
                        Process.Start(new ProcessStartInfo(mountPoint) { UseShellExecute = true });
                    }
                    else
                    {
                        MessageBox.Show($"挂载点目录不存在: {mountPoint}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("该挂载项没有有效的挂载点路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    mp = DeriveAutoMountPoint(image);

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

        private string DeriveAutoMountPoint(string imagePath)
        {
            try
            {
                var dir = Path.GetDirectoryName(imagePath);
                var name = Path.GetFileNameWithoutExtension(imagePath);
                return Path.Combine(dir ?? "", name);
            }
            catch { return ""; }
        }

        private void RefreshMountedList()
        {
            lvMounted.Items.Clear();

            // 清空旧列表，完全以 WMI 为权威来源重建
            _dwarfsService.MountedImages.Clear();

            var allMounted = new List<MountInfo>();
            try
            {
                var winfspMounted = _dwarfsService.GetWinFspMountedImages();
                foreach (var mi in winfspMounted)
                {
                    if (!allMounted.Any(m => m.ProcessId == mi.ProcessId))
                        allMounted.Add(mi);
                }
            }
            catch (Exception ex)
            {
                Log($"获取 WinFsp 挂载信息失败: {ex.Message}");
            }

            // 同步到 MountedImages
            foreach (var mi in allMounted)
                _dwarfsService.MountedImages.Add(mi);

            foreach (var mi in allMounted)
            {
                var item = new ListViewItem(mi.ImagePath);
                item.SubItems.Add(string.IsNullOrEmpty(mi.MountPoint) ? "(自动)" : mi.MountPoint);
                item.SubItems.Add(mi.MountTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.Tag = mi;
                lvMounted.Items.Add(item);
            }

            if (allMounted.Count == 0)
            {
                Log("当前没有已挂载的镜像。");
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

                // 文件右键菜单 - 制作镜像（所有文件）
                using var fileMakeKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\*\shell\DwarfsGUI.Make");
                fileMakeKey.SetValue("MUIVerb", "制作镜像");
                fileMakeKey.SetValue("Icon", $"\"{exePath}\",0");
                fileMakeKey.SetValue("Position", "Bottom");
                using var fileMakeCmd = fileMakeKey.CreateSubKey("command");
                fileMakeCmd.SetValue("", $"\"{exePath}\" --create \"%1\"");

                // .dwarfs 文件右键菜单 - 挂载镜像（只对 dwarfs 文件显示）
                using var dwarfsKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\.dwarfs");
                var dwarfsFileType = dwarfsKey.GetValue("") as string;
                if (string.IsNullOrEmpty(dwarfsFileType))
                {
                    dwarfsFileType = "DwarfsArchive";
                    dwarfsKey.SetValue("", dwarfsFileType);
                }
                dwarfsKey.Close();

                // 为 dwarfs 文件类型创建挂载菜单
                using var dwarfsMountKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    $"Software\\Classes\\{dwarfsFileType}\\shell\\DwarfsGUI.Mount");
                dwarfsMountKey.SetValue("MUIVerb", "挂载镜像");
                dwarfsMountKey.SetValue("Icon", $"\"{exePath}\",0");
                dwarfsMountKey.SetValue("Position", "Bottom");
                using var dwarfsMountCmd = dwarfsMountKey.CreateSubKey("command");
                dwarfsMountCmd.SetValue("", $"\"{exePath}\" --mount \"%1\"");

                // 文件夹右键菜单 - 制作镜像
                using var folderMakeKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\Directory\shell\DwarfsGUI.Make");
                folderMakeKey.SetValue("MUIVerb", "制作镜像");
                folderMakeKey.SetValue("Icon", $"\"{exePath}\",0");
                folderMakeKey.SetValue("Position", "Bottom");
                using var folderMakeCmd = folderMakeKey.CreateSubKey("command");
                folderMakeCmd.SetValue("", $"\"{exePath}\" --create \"%1\"");

                // 文件夹背景右键菜单 - 制作镜像
                using var bgMakeKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI.Make");
                bgMakeKey.SetValue("MUIVerb", "制作镜像");
                bgMakeKey.SetValue("Icon", $"\"{exePath}\",0");
                bgMakeKey.SetValue("Position", "Bottom");
                using var bgMakeCmd = bgMakeKey.CreateSubKey("command");
                bgMakeCmd.SetValue("", $"\"{exePath}\" --create \"%V\"");

                lblContextMenuStatus.Text = "右键菜单已注册";
                lblContextMenuStatus.ForeColor = Color.Green;
                Log("右键菜单注册成功");
                Log("- 制作镜像：所有文件/文件夹/背景");
                Log("- 挂载镜像：仅 .dwarfs 文件");
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
                // 删除文件右键菜单 - 制作镜像
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\*\shell\DwarfsGUI.Make", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\*\shell\DwarfsGUI.Mount", false);
                
                // 删除 .dwarfs 文件挂载菜单
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\DwarfsArchive\shell\DwarfsGUI.Mount", false);
                
                // 删除文件夹右键菜单
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\shell\DwarfsGUI.Make", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\shell\DwarfsGUI.Mount", false);
                
                // 删除文件夹背景右键菜单
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI.Make", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI.Mount", false);

                // 清理旧的二级菜单相关注册表（如果存在）
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\*\shell\DwarfsGUI", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\shell\DwarfsGUI", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\Directory\Background\shell\DwarfsGUI", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\DwarfsGUI.Shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(
                    @"Software\Classes\DwarfsGUI", false);

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

        private void btnBrowseWinFsp_Click(object sender, EventArgs e)
        {
            BrowseFolder(txtWinFspPath);
        }
    }


}
