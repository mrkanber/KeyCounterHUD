using System.IO;
using System.Text.Json;

namespace KeyCounterHUD.Models;

public class AppConfig
{
    public long TotalCount { get; set; }
    public double Left { get; set; } = 12;
    public double Top { get; set; } = 12;
    public bool AutoStart { get; set; }
    public string TextColor { get; set; } = "#FFFFFF";

    private static string ConfigPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyCounterHUD", "config.json");

    public static AppConfig Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<AppConfig>(json);
                if (config != null) return config;
            }
        }
        catch
        {
            // bozuk config dosyası varsayılanla değiştirilir
        }

        return new AppConfig();
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(ConfigPath)!;
            Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }
        catch
        {
            // diske yazılamazsa sessizce yut, sayaç bir sonraki kapanışta tekrar denenir
        }
    }
}
