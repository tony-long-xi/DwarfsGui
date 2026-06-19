namespace DwarfsGui.Models;

public class MountInfo
{
    public string ImagePath { get; set; } = string.Empty;
    public string MountPoint { get; set; } = string.Empty;
    public int ProcessId { get; set; }
    public DateTime MountTime { get; set; } = DateTime.Now;
}