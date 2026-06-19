namespace DwarfsGui.Models;

public class DwarfsSettings
{
    // 制作镜像参数
    public int CompressionLevel { get; set; } = 7;
    public bool Force { get; set; } = true;
    public int Workers { get; set; } = 4;

    // 挂载参数
    public string CacheSize { get; set; } = "512M";
    public string ReadAhead { get; set; } = "0";
    public int MountWorkers { get; set; } = 2;
    public string DebugLevel { get; set; } = "info";
    public bool AutoMountPoint { get; set; } = true;

    // 提取参数
    public int ExtractWorkers { get; set; } = 4;
    public string ExtractCacheSize { get; set; } = "512M";
    public bool ContinueOnError { get; set; } = true;
}