# Animal Farm (Windows)

Desktop pet gallery for Windows — add photos, names, and short descriptions. Sibling to the [animal-farm](../animal-farm) webapp; data is stored separately on this PC.

## Requirements

- Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (includes the Windows Desktop runtime pieces used by WPF)

## Run

From this folder (open a **new** PowerShell/Terminal after installing the SDK so `dotnet` is on PATH):

```powershell
dotnet run --project src/AnimalFarm.App/AnimalFarm.App.csproj
```

If `dotnet` is not recognized, use the full path or restart the terminal:

```powershell
& "C:\Program Files\dotnet\dotnet.exe" run --project src/AnimalFarm.App/AnimalFarm.App.csproj
```

Or open `AnimalFarm.slnx` in Visual Studio and press F5.

## Publish a folder build

```powershell
dotnet publish src/AnimalFarm.App/AnimalFarm.App.csproj -c Release -o publish
```

Then run `publish\AnimalFarm.exe`.

## Data location

Pets and photos are stored under:

`%LocalAppData%\AnimalFarm\`

- `pets.db` — SQLite metadata  
- `Images\` — resized JPEG photos  
- `settings.json` — language preference (English / Svenska)

This is not shared with the browser IndexedDB used by the GitHub Pages webapp.
