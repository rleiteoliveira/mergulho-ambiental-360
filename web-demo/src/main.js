const fallbackCatalog = {
  items: [
    {
      id: "peixe-boi",
      title: "Mergulho com Peixe-Boi",
      description: "Experiência 360 sobre conservação do peixe-boi e vida marinha.",
      category: "Biodiversidade",
      sourceType: "Placeholder",
      localFileName: "",
      streamingUrl: "",
      thumbnailName: "peixe-boi-placeholder.png",
      durationLabel: "3 min",
      isEnabled: true
    }
  ]
};

const grid = document.querySelector("#video-grid");
const statusText = document.querySelector("#catalog-status");
const selectedTitle = document.querySelector("#selected-title");
const selectedDescription = document.querySelector("#selected-description");
const player = document.querySelector("#video-player");
const playerNote = document.querySelector("#player-note");

// Catálogo canônico da web demo.
// O caminho é relativo ao index.html, que é servido na raiz de web-demo/.
// Por isso usamos "src/video-catalog.json" e não "video-catalog.json".
const CATALOG_URL = "src/video-catalog.json";

async function loadCatalog() {
  try {
    const response = await fetch(CATALOG_URL);
    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    const catalog = await response.json();
    const count = Array.isArray(catalog.items) ? catalog.items.length : 0;
    console.info("[Mergulho360] Catalog loaded", count, `(${CATALOG_URL})`);
    return catalog;
  } catch (error) {
    console.warn("[Mergulho360] Using fallback catalog", error);
    statusText.textContent = "Catálogo fallback";
    return fallbackCatalog;
  }
}

function renderCatalog(catalog) {
  const items = (catalog.items || []).filter((item) => item.isEnabled);
  statusText.textContent = `${items.length} experiências`;
  grid.innerHTML = "";

  items.forEach((item) => {
    const button = document.createElement("button");
    button.className = "video-card";
    button.type = "button";
    button.innerHTML = `
      <h3>${escapeHtml(item.title)}</h3>
      <p>${escapeHtml(item.description)}</p>
      <div class="meta-row">
        <span>${escapeHtml(item.category)}</span>
        <span>${escapeHtml(item.durationLabel)}</span>
      </div>
    `;

    button.addEventListener("click", () => selectVideo(item));
    grid.appendChild(button);
  });

  console.info("[Mergulho360] Rendered cards", items.length);
}

function selectVideo(item) {
  selectedTitle.textContent = item.title;
  selectedDescription.textContent = item.description;

  const source = resolveSource(item);
  if (!source) {
    player.removeAttribute("src");
    player.load();
    playerNote.textContent = "Placeholder selecionado. Configure um arquivo ou URL autorizado para testar reprodução.";
    return;
  }

  player.src = source;
  player.load();
  playerNote.textContent = "Player HTML5 comum. Esta demo ainda não renderiza o vídeo em esfera 360.";
}

function resolveSource(item) {
  if (item.sourceType === "LocalFile" && item.localFileName) {
    return `public/videos/${item.localFileName}`;
  }

  if (item.sourceType === "StreamingUrl" && item.streamingUrl) {
    return item.streamingUrl;
  }

  return "";
}

function escapeHtml(value) {
  return String(value || "")
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

const catalog = await loadCatalog();
renderCatalog(catalog);
