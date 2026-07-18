using System.Text.Json;
using AnimalFarm.Core.I18n;
using AnimalFarm.Core.Storage;

namespace AnimalFarm.Core.Services;

public sealed class SettingsService
{
    private readonly AppPaths _paths;

    public SettingsService(AppPaths paths)
    {
        _paths = paths;
    }

    public AppLanguage LoadLanguage()
    {
        try
        {
            if (!File.Exists(_paths.SettingsPath))
            {
                return AppLanguage.English;
            }

            var json = File.ReadAllText(_paths.SettingsPath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json);
            return string.Equals(settings?.Language, "sv", StringComparison.OrdinalIgnoreCase)
                ? AppLanguage.Swedish
                : AppLanguage.English;
        }
        catch
        {
            return AppLanguage.English;
        }
    }

    public void SaveLanguage(AppLanguage language)
    {
        _paths.EnsureCreated();
        var settings = new AppSettings
        {
            Language = language == AppLanguage.Swedish ? "sv" : "en",
        };
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_paths.SettingsPath, json);
    }

    private sealed class AppSettings
    {
        public string? Language { get; set; }
    }
}
