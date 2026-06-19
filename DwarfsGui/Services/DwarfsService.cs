using System.Diagnostics;
using System.Management;
using System.Text;
using DwarfsGui.Models;

namespace DwarfsGui.Services;

public class DwarfsService
{
    private readonly string _binPath;
    private readonly Dictionary<string, Process> _mountProcesses = new();
    public List<MountInfo> MountedImages { get; } = new();
    public string WinFspPath { get; set; } = @"C:\Program Files (x86)\WinFsp\bin";

    public DwarfsService()
    {
        // 固定路径：程序目录下的 DwarfsBin 文件夹
        _binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DwarfsBin");
    }

    private string MkdwarfsPath => Path.Combine(_binPath, "mkdwarfs.exe");
    private string DwarfsPath => Path.Combine(_binPath, "dwarfs.exe");
    private string ExtractPath => Path.Combine(_binPath, "dwarfsextract.exe");

    private void EnsureWinFspPath()
    {
        var envPath = Environment.GetEnvironmentVariable("PATH") ?? "";
        if (!envPath.Contains(WinFspPath))
            Environment.SetEnvironmentVariable("PATH", $"{WinFspPath};{envPath}");
    }

    public async Task<(bool Success, string Output)> CreateImageAsync(
        string inputPath, string outputPath, DwarfsSettings settings,
        Action<string>? onOutput = null, CancellationToken ct = default)
    {
        var args = new List<string>
        {
            "-i", $"\"{inputPath}\"",
            "-o", $"\"{outputPath}\"",
            "-l", settings.CompressionLevel.ToString()
        };

        if (settings.Force)
            args.Add("-f");

        // 自定义压缩算法
        if (!string.IsNullOrEmpty(settings.CompressionAlgorithm) &&
            settings.CompressionAlgorithm != "zstd:level=22")
        {
            args.Add("-C");
            args.Add(settings.CompressionAlgorithm);
        }

        // 禁用去重（跳过文件哈希计算）
        if (settings.DisableDedup)
        {
            args.Add("--file-hash");
            args.Add("none");
        }

        return await RunCommandAsync(MkdwarfsPath, args, onOutput, ct);
    }

    public async Task<(bool Success, string Output, Process? Process)> MountImageAsync(
        string imagePath, string? mountPoint, DwarfsSettings settings,
        Action<string>? onOutput = null, CancellationToken ct = default)
    {
        EnsureWinFspPath();

        var args = new List<string> { $"\"{imagePath}\"" };

        if (!string.IsNullOrEmpty(mountPoint))
            args.Add($"\"{mountPoint}\"");
        else
            args.Add("--auto-mountpoint");

        args.Add("-f"); // 前台运行，保持进程存活

        args.Add("-o");
        args.Add($"cachesize={settings.CacheSize}");
        args.Add("-o");
        args.Add($"readahead={settings.ReadAhead}");
        args.Add("-o");
        args.Add($"workers={settings.MountWorkers}");
        args.Add("-o");
        args.Add($"debuglevel={settings.DebugLevel}");

        var psi = new ProcessStartInfo
        {
            FileName = DwarfsPath,
            Arguments = string.Join(" ", args),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = psi };
        var output = new StringBuilder();
        var mountPointResult = mountPoint ?? "";

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (output) output.AppendLine(e.Data);
                onOutput?.Invoke(e.Data);

                // 尝试从输出中提取挂载点
                if (string.IsNullOrEmpty(mountPointResult))
                {
                    var mp = ExtractMountPointFromLine(e.Data);
                    if (!string.IsNullOrEmpty(mp))
                        mountPointResult = mp;
                }
            }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (output) output.AppendLine(e.Data);
                onOutput?.Invoke(e.Data);

                if (string.IsNullOrEmpty(mountPointResult))
                {
                    var mp = ExtractMountPointFromLine(e.Data);
                    if (!string.IsNullOrEmpty(mp))
                        mountPointResult = mp;
                }
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // 等待一小段时间让挂载建立
        await Task.Delay(1500, ct);

        if (process.HasExited)
        {
            var result = output.ToString();
            return (false, result, null);
        }

        return (true, output.ToString(), process);
    }

    public async Task<(bool Success, string Output)> ExtractImageAsync(
        string imagePath, string outputPath, DwarfsSettings settings,
        Action<string>? onOutput = null, CancellationToken ct = default)
    {
        // 确保输出目录存在
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        var args = new List<string>
        {
            "-i", $"\"{imagePath}\"",
            "-o", $"\"{outputPath}\"",
            "-n", settings.ExtractWorkers.ToString(),
            "-s", settings.ExtractCacheSize
        };

        if (settings.ContinueOnError)
            args.Add("--continue-on-error");

        return await RunCommandAsync(ExtractPath, args, onOutput, ct);
    }

    public List<MountInfo> GetWinFspMountedImages()
    {
        var result = new List<MountInfo>();

        // 通过 WMI 查询所有运行中的 dwarfs.exe 进程及其命令行
        try
        {
            var searcher = new ManagementObjectSearcher(
                "SELECT ProcessId, CommandLine, CreationDate FROM Win32_Process WHERE Name = 'dwarfs.exe'");
            foreach (var obj in searcher.Get())
            {
                try
                {
                    var pid = Convert.ToInt32(obj["ProcessId"]);
                    var cmdLine = obj["CommandLine"] as string ?? "";
                    var creationDate = obj["CreationDate"];

                    var (imagePath, mountPoint) = ParseMountCommandLine(cmdLine);
                    if (string.IsNullOrEmpty(imagePath)) continue;

                    // 如果是 --auto-mountpoint，从镜像路径推导挂载点
                    if (string.IsNullOrEmpty(mountPoint) && cmdLine.Contains("--auto-mountpoint"))
                    {
                        mountPoint = DeriveAutoMountPoint(imagePath);
                    }

                    DateTime mountTime = DateTime.Now;
                    try
                    {
                        if (creationDate != null)
                            mountTime = ManagementDateTimeConverter.ToDateTime(creationDate.ToString());
                    }
                    catch { }

                    result.Add(new MountInfo
                    {
                        ImagePath = imagePath,
                        MountPoint = mountPoint ?? "",
                        ProcessId = pid,
                        MountTime = mountTime
                    });
                }
                catch { }
            }
        }
        catch { }

        return result;
    }

    /// <summary>
    /// 从镜像路径推导 --auto-mountpoint 模式下的挂载点
    /// 例如: F:\dir\test.dwarfs → F:\dir\test
    /// </summary>
    private static string DeriveAutoMountPoint(string imagePath)
    {
        try
        {
            var dir = Path.GetDirectoryName(imagePath);
            var name = Path.GetFileNameWithoutExtension(imagePath);
            return Path.Combine(dir ?? "", name);
        }
        catch { return ""; }
    }

    private static (string ImagePath, string MountPoint) ParseMountCommandLine(string cmdLine)
    {
        var parts = SplitCommandLine(cmdLine);
        string imagePath = "";
        string mountPoint = "";
        bool autoMountPoint = false;

        // 第一个参数是 dwarfs.exe 本身，跳过
        // 遍历剩余参数
        for (int i = 1; i < parts.Count; i++)
        {
            var arg = parts[i].Trim('"');

            if (arg == "--auto-mountpoint")
            {
                autoMountPoint = true;
                continue;
            }

            if (arg.StartsWith("-"))
            {
                // -o 后面跟一个值参数，需要一起跳过
                if (arg == "-o" && i + 1 < parts.Count)
                    i++;
                continue;
            }

            // 第一个非选项参数是镜像路径
            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = arg;
            }
            // 第二个非选项参数是挂载点
            else if (string.IsNullOrEmpty(mountPoint))
            {
                mountPoint = arg;
            }
        }

        if (autoMountPoint && string.IsNullOrEmpty(mountPoint))
            mountPoint = DeriveAutoMountPoint(imagePath);

        return (imagePath, mountPoint);
    }

    private static List<string> SplitCommandLine(string cmdLine)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < cmdLine.Length; i++)
        {
            var ch = cmdLine[i];
            if (ch == '"')
            {
                inQuotes = !inQuotes;
                current.Append(ch);
            }
            else if (ch == ' ' && !inQuotes)
            {
                if (current.Length > 0)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(ch);
            }
        }
        if (current.Length > 0)
            result.Add(current.ToString());

        return result;
    }

    public void RegisterMount(string mountPoint, Process process)
    {
        _mountProcesses[mountPoint] = process;
    }

    public void Unmount(string mountPoint)
    {
        // 先尝试通过本会话管理的进程卸载
        if (_mountProcesses.TryGetValue(mountPoint, out var process))
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                    process.WaitForExit(3000);
                }
            }
            catch { }
            finally
            {
                process.Dispose();
                _mountProcesses.Remove(mountPoint);
            }
            return;
        }

        // 如果本会话没有管理该挂载点，尝试通过 PID 杀掉外部 dwarfs 进程
        var mounted = GetWinFspMountedImages()
            .FirstOrDefault(m => m.MountPoint == mountPoint);
        if (mounted != null && mounted.ProcessId > 0)
        {
            try
            {
                using var proc = Process.GetProcessById(mounted.ProcessId);
                proc.Kill();
                proc.WaitForExit(3000);
            }
            catch { }
        }
    }

    /// <summary>
    /// 通过 PID 卸载外部 dwarfs 进程
    /// </summary>
    public void UnmountByPid(int pid)
    {
        try
        {
            using var proc = Process.GetProcessById(pid);
            proc.Kill();
            proc.WaitForExit(3000);
        }
        catch { }
    }

    public void UnmountAll()
    {
        // 先清理本会话管理的挂载
        foreach (var mp in _mountProcesses.Keys.ToList())
            Unmount(mp);

        // 再清理 WMI 发现的外部 dwarfs 进程
        try
        {
            var mounted = GetWinFspMountedImages();
            foreach (var mi in mounted)
            {
                if (mi.ProcessId > 0)
                {
                    try
                    {
                        using var proc = Process.GetProcessById(mi.ProcessId);
                        proc.Kill();
                        proc.WaitForExit(3000);
                    }
                    catch { }
                }
            }
        }
        catch { }
    }

    private string ExtractMountPointFromLine(string line)
    {
        // dwarfs 启动时输出类似: "mountpoint F:\path" 或包含盘符路径
        var trimmed = line.Trim();
        if (trimmed.Length >= 3 && char.IsLetter(trimmed[0]) && trimmed[1] == ':')
        {
            // 可能包含盘符路径
            var match = System.Text.RegularExpressions.Regex.Match(trimmed, @"[A-Za-z]:\\[^\s]*");
            if (match.Success)
                return match.Value;
        }
        return "";
    }

    private async Task<(bool Success, string Output)> RunCommandAsync(
        string exePath, List<string> args,
        Action<string>? onOutput, CancellationToken ct)
    {
        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = string.Join(" ", args),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        var output = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (output) output.AppendLine(e.Data);
                onOutput?.Invoke(e.Data);
            }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (output) output.AppendLine(e.Data);
                onOutput?.Invoke(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        try
        {
            await process.WaitForExitAsync(ct);
        }
        catch (OperationCanceledException)
        {
            try { process.Kill(); } catch { }
            return (false, "操作已取消");
        }

        return (process.ExitCode == 0, output.ToString());
    }
}