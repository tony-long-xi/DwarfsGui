using System.Text.Json;
using DwarfsGui.Models;

namespace DwarfsGui.Services;

public class SettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DwarfsGui",
        "settings.json");

    public DwarfsSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<DwarfsSettings>(json) ?? new DwarfsSettings();
            }
        }
        catch { }

        // 首次运行，自动创建默认配置文件
        var settings = new DwarfsSettings();
        Save(settings);
        return settings;
    }

    public void Save(DwarfsSettings settings)
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch { }
    }
}