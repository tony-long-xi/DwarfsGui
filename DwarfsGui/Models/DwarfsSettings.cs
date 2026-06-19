namespace DwarfsGui.Models;

public class DwarfsSettings
{
    // 路径配置
    public string WinFspPath { get; set; } = @"C:\Program Files (x86)\WinFsp\bin";

    // 制作镜像参数
    public int CompressionLevel { get; set; } = 2;
    public bool Force { get; set; } = true;
    public int Workers { get; set; } = 8;

    // 速度优化参数
    public bool DisableDedup { get; set; } = false;
    public string CompressionAlgorithm { get; set; } = "";

    // 挂载参数
    public string CacheSize { get; set; } = "512M";
    public string ReadAhead { get; set; } = "0";
    public int MountWorkers { get; set; } = 4;
    public string DebugLevel { get; set; } = "info";
    public bool AutoMountPoint { get; set; } = true;

    // 提取参数
    public int ExtractWorkers { get; set; } = 8;
    public string ExtractCacheSize { get; set; } = "512M";
    public bool ContinueOnError { get; set; } = true;
}