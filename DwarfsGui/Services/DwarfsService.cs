using System.Diagnostics;
using System.Text;
using DwarfsGui.Models;

namespace DwarfsGui.Services;

public class DwarfsService
{
    private readonly string _binPath;
    private readonly Dictionary<string, Process> _mountProcesses = new();
    public List<MountInfo> MountedImages { get; } = new();

    public DwarfsService()
    {
        _binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "bin");
        if (!Directory.Exists(_binPath))
            _binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
    }

    private string MkdwarfsPath => Path.Combine(_binPath, "mkdwarfs.exe");
    private string DwarfsPath => Path.Combine(_binPath, "dwarfs.exe");
    private string ExtractPath => Path.Combine(_binPath, "dwarfsextract.exe");

    private void EnsureWinFspPath()
    {
        var envPath = Environment.GetEnvironmentVariable("PATH") ?? "";
        var winfspBin = @"C:\Program Files (x86)\WinFsp\bin";
        if (!envPath.Contains(winfspBin))
            Environment.SetEnvironmentVariable("PATH", $"{winfspBin};{envPath}");
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

    public void RegisterMount(string mountPoint, Process process)
    {
        _mountProcesses[mountPoint] = process;
    }

    public void Unmount(string mountPoint)
    {
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
        }
    }

    public void UnmountAll()
    {
        foreach (var mp in _mountProcesses.Keys.ToList())
            Unmount(mp);
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