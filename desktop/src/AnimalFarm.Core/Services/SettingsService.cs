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
        var settings = Read();
        return string.Equals(settings.Language, "sv", StringComparison.OrdinalIgnoreCase)
            ? AppLanguage.Swedish
            : AppLanguage.English;
    }

    public AppTheme LoadTheme()
    {
        var settings = Read();
        return string.Equals(settings.Theme, "dark", StringComparison.OrdinalIgnoreCase)
            ? AppTheme.Dark
            : AppTheme.Light;
    }

    public void SaveLanguage(AppLanguage language)
    {
        var settings = Read();
        settings.Language = language == AppLanguage.Swedish ? "sv" : "en";
        Write(settings);
    }

    public void SaveTheme(AppTheme theme)
    {
        var settings = Read();
        settings.Theme = theme == AppTheme.Dark ? "dark" : "light";
        Write(settings);
    }

    private AppSettings Read()
    {
        try
        {
            if (!File.Exists(_paths.SettingsPath))
            {
                return new AppSettings();
            }

            var json = File.ReadAllText(_paths.SettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    private void Write(AppSettings settings)
    {
        _paths.EnsureCreated();
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_paths.SettingsPath, json);
    }

    private sealed class AppSettings
    {
        public string? Language { get; set; }
        public string? Theme { get; set; }
    }
}
