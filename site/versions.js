async function loadVersions() {
  const picker = document.querySelector(".version-picker");
  const select = document.getElementById("version-select");
  const downloadVersion = document.getElementById("download-version");
  const downloadLatest = document.getElementById("download-latest");
  const latestMeta = document.getElementById("latest-meta");

  try {
    const response = await fetch("./versions.json", { cache: "no-store" });
    if (!response.ok) {
      return;
    }

    const data = await response.json();
    const versions = Array.isArray(data.versions) ? data.versions : [];
    if (!versions.length) {
      return;
    }

    const latest = data.latest || versions[0].version;
    latestMeta.textContent = `Version ${latest} · Zip · Windows 10/11 (64-bit) · No .NET install needed`;
    downloadLatest.href = "AnimalFarm-Windows.zip";

    select.innerHTML = "";
    for (const entry of versions) {
      const option = document.createElement("option");
      option.value = entry.file;
      option.textContent = entry.label || entry.version;
      if (entry.version === latest) {
        option.selected = true;
      }
      select.appendChild(option);
    }

    const syncLink = () => {
      downloadVersion.href = select.value;
    };
    select.addEventListener("change", syncLink);
    syncLink();

    picker.hidden = false;
  } catch {
    // Keep the static latest download CTA.
  }
}

loadVersions();
