# Animal Farm (Windows download site)

GitHub Pages site for installing the **Animal Farm** Windows desktop app. Visitors download a zip, unzip, and run `AnimalFarm.exe`.

Sibling to:
- [animal-farm](../animal-farm) — web gallery
- [animal-farm-windows-app](../animal-farm-windows-app) — local WPF source you develop against

This repo includes a copy of the desktop app under `desktop/` used by CI to build the zip.

## Use on GitHub Pages

After deploy, open:

`https://YOUR_GITHUB_USERNAME.github.io/animal-farm-windows/`

### One-time GitHub setup

1. Create a GitHub repo named **`animal-farm-windows`** (must match for the default Pages URL).
2. Push this folder to that repo (`main` or `master`).
3. Wait for the **Deploy Windows download site** workflow to finish once (it creates the `gh-pages` branch).
4. If the deploy step fails quickly, set **Settings → Actions → General → Workflow permissions** to **Read and write permissions**, then re-run the workflow.
5. Repo **Settings → Pages**:
   - **Source:** Deploy from a branch
   - **Branch:** `gh-pages` / `/ (root)`
6. Open the Pages URL and use **Download latest** (or pick an older build from **Other versions**).

Update the “Web gallery” link in [`site/index.html`](site/index.html) if your GitHub username differs.

## Versions

Each deploy publishes:

- `AnimalFarm-Windows.zip` — always the latest
- `releases/AnimalFarm-Windows-{version}.zip` — kept for the version selector
- `versions.json` — list used by the site

Bump `<Version>` in `desktop/src/AnimalFarm.App/AnimalFarm.App.csproj` (and the same in `animal-farm-windows-app`) when you ship a new build.

## What the workflow builds

On every push to `main`, GitHub Actions (Windows runner):

1. Publishes a **self-contained win-x64** build (no separate .NET install for users)
2. Zips it to `AnimalFarm-Windows.zip`
3. Pushes the `site/` folder (landing page + zip) to the **`gh-pages`** branch

GitHub Pages then serves that branch.

## Preview the landing page locally

Open [`site/index.html`](site/index.html) in a browser. The download link works after CI has produced the zip (or after you build one locally into `site/`).

### Build the zip on your PC

```powershell
cd C:\Users\jon19\charlies_stuff\animal-farm-windows
dotnet publish desktop/src/AnimalFarm.App/AnimalFarm.App.csproj -c Release -r win-x64 --self-contained true -o artifacts/AnimalFarm
Compress-Archive -Path artifacts/AnimalFarm\* -DestinationPath site/AnimalFarm-Windows.zip -Force
```

## Develop the desktop app

Day-to-day WPF work can stay in [`animal-farm-windows-app`](../animal-farm-windows-app). When you want the download site updated, copy changes into `desktop/` (or develop in `desktop/` and push this repo).
