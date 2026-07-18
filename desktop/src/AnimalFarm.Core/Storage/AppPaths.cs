namespace AnimalFarm.Core.Storage;

public sealed class AppPaths
{
    public string Root { get; }
    public string DatabasePath { get; }
    public string ImagesDirectory { get; }
    public string SettingsPath { get; }

    public AppPaths()
    {
        Root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AnimalFarm");
        DatabasePath = Path.Combine(Root, "pets.db");
        ImagesDirectory = Path.Combine(Root, "Images");
        SettingsPath = Path.Combine(Root, "settings.json");
    }

    public void EnsureCreated()
    {
        Directory.CreateDirectory(Root);
        Directory.CreateDirectory(ImagesDirectory);
    }
}
