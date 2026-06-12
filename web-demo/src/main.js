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
const playerNote = document.querySelector("#player-note");

// Elementos do player 360 (A-Frame).
const scene = document.querySelector("a-scene");
const sky = document.querySelector("#sky360");
const videosphere = document.querySelector("#videosphere");
const video360 = document.querySelector("#video360");
const vrMenu = document.querySelector("#vr-menu");
const vrMenuToggle = document.querySelector("#vr-menu-toggle");

let currentItems = [];

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
  currentItems = items;
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

  buildVrMenu(items);
  console.info("[Mergulho360] Rendered cards", items.length);
}

function selectVideo(item) {
  selectedTitle.textContent = item.title;
  selectedDescription.textContent = item.description;

  const source = resolveSource(item);
  if (source) {
    showVideo360(source);
    playerNote.textContent =
      "Vídeo 360 em esfera (WebXR). Mova o celular ou arraste para olhar em volta.";
  } else {
    showPlaceholder360(item);
    playerNote.textContent =
      "Placeholder 360 navegável. No celular, mova o aparelho (giroscópio); no desktop, arraste. Toque no ícone de óculos para entrar em VR.";
  }
}

function showPlaceholder360(item) {
  try { video360.pause(); } catch (error) { /* sem vídeo carregado ainda */ }
  videosphere.setAttribute("visible", false);
  sky.setAttribute("src", makePlaceholderTexture(item));
  sky.setAttribute("visible", true);
}

function showVideo360(source) {
  sky.setAttribute("visible", false);
  if (video360.getAttribute("src") !== source) {
    video360.setAttribute("src", source);
    video360.load();
  }
  videosphere.setAttribute("src", "#video360");
  videosphere.setAttribute("visible", true);
  const playback = video360.play();
  if (playback && typeof playback.catch === "function") {
    playback.catch(() => {
      // Autoplay pode exigir mais um toque do usuário em alguns navegadores.
    });
  }
}

// Gera uma textura equiretangular (2:1) por código, sem baixar nada.
// Marcadores de direção + título permitem "sentir" o olhar em volta no 360.
function makePlaceholderTexture(item) {
  const width = 2048;
  const height = 1024;
  const canvas = document.createElement("canvas");
  canvas.width = width;
  canvas.height = height;
  const ctx = canvas.getContext("2d");

  // Gradiente "oceano": superfície clara no topo, fundo escuro embaixo.
  const gradient = ctx.createLinearGradient(0, 0, 0, height);
  gradient.addColorStop(0, "#0e3a44");
  gradient.addColorStop(0.5, "#0b2c33");
  gradient.addColorStop(1, "#041417");
  ctx.fillStyle = gradient;
  ctx.fillRect(0, 0, width, height);

  // Linha do horizonte.
  ctx.strokeStyle = "rgba(255, 212, 107, 0.5)";
  ctx.lineWidth = 3;
  ctx.beginPath();
  ctx.moveTo(0, height / 2);
  ctx.lineTo(width, height / 2);
  ctx.stroke();

  // Grade vertical a cada 45 graus.
  ctx.strokeStyle = "rgba(255, 255, 255, 0.12)";
  ctx.lineWidth = 2;
  for (let deg = 0; deg < 360; deg += 45) {
    const x = (deg / 360) * width;
    ctx.beginPath();
    ctx.moveTo(x, 0);
    ctx.lineTo(x, height);
    ctx.stroke();
  }

  // "Bolhas" decorativas (determinístico, sem Math.random).
  ctx.fillStyle = "rgba(255, 255, 255, 0.10)";
  for (let i = 0; i < 70; i++) {
    const bx = (i * 211.7) % width;
    const by = (i * 137.3) % height;
    const r = 5 + (i % 6) * 4;
    ctx.beginPath();
    ctx.arc(bx, by, r, 0, Math.PI * 2);
    ctx.fill();
  }

  // Marcadores de direção (centro da imagem = frente).
  const directions = [
    { label: "FRENTE", u: 0.5 },
    { label: "DIREITA", u: 0.75 },
    { label: "ESQUERDA", u: 0.25 },
    { label: "TRÁS", u: 0.02 }
  ];
  ctx.textAlign = "center";
  ctx.textBaseline = "middle";
  ctx.fillStyle = "#ffd46b";
  ctx.font = "bold 70px Arial";
  directions.forEach((dir) => {
    ctx.fillText(dir.label, dir.u * width, height / 2 - 110);
  });

  // Título da experiência, ao centro/frente.
  ctx.fillStyle = "#f4fbfb";
  ctx.font = "bold 90px Arial";
  ctx.fillText(item.title, width * 0.5, height * 0.5 + 50);
  ctx.fillStyle = "#b9d2d5";
  ctx.font = "40px Arial";
  ctx.fillText("Placeholder 360 — olhe em volta", width * 0.5, height * 0.5 + 130);

  return canvas.toDataURL("image/png");
}

// Rótulo de botão como textura de canvas (mesma ideia do placeholder).
// Canvas usa a fonte do navegador, então acentos (ã, é, ç) saem corretos
// — a fonte SDF padrão do A-Frame não cobre bem esses caracteres.
function makeLabelTexture(title) {
  const width = 512;
  const height = 200;
  const canvas = document.createElement("canvas");
  canvas.width = width;
  canvas.height = height;
  const ctx = canvas.getContext("2d");
  ctx.clearRect(0, 0, width, height);

  const pad = 8;
  ctx.fillStyle = "rgba(11, 36, 41, 0.92)";
  ctx.strokeStyle = "#2f5b62";
  ctx.lineWidth = 5;
  if (ctx.roundRect) {
    ctx.beginPath();
    ctx.roundRect(pad, pad, width - 2 * pad, height - 2 * pad, 24);
    ctx.fill();
    ctx.stroke();
  } else {
    ctx.fillRect(pad, pad, width - 2 * pad, height - 2 * pad);
    ctx.strokeRect(pad, pad, width - 2 * pad, height - 2 * pad);
  }

  ctx.fillStyle = "#f4fbfb";
  ctx.textAlign = "center";
  ctx.textBaseline = "middle";
  ctx.font = "bold 42px Arial";

  // Quebra o título em linhas que cabem na largura.
  const maxWidth = width - 70;
  const words = String(title).split(" ");
  const lines = [];
  let line = "";
  words.forEach((word) => {
    const test = line ? `${line} ${word}` : word;
    if (ctx.measureText(test).width > maxWidth && line) {
      lines.push(line);
      line = word;
    } else {
      line = test;
    }
  });
  if (line) lines.push(line);

  const lineHeight = 48;
  const startY = height / 2 - ((lines.length - 1) * lineHeight) / 2;
  lines.forEach((text, i) => ctx.fillText(text, width / 2, startY + i * lineHeight));

  return canvas.toDataURL("image/png");
}

// Monta o menu dentro da cena (um plano clicável por experiência).
// Selecionável por gaze+dwell (celular/headset), mouse (desktop) e laser (controle).
function buildVrMenu(items) {
  if (!vrMenu) return;
  while (vrMenu.firstChild) {
    vrMenu.removeChild(vrMenu.firstChild);
  }

  const cols = 2;
  const colGap = 1.04;
  const rowGap = 0.46;
  const btnW = 0.96;
  const btnH = 0.38;

  items.forEach((item, i) => {
    const col = i % cols;
    const row = Math.floor(i / cols);
    const x = (col - (cols - 1) / 2) * colGap;
    const y = (1 - row) * rowGap;

    const button = document.createElement("a-entity");
    button.setAttribute("class", "menu-item");
    button.setAttribute("geometry", `primitive: plane; width: ${btnW}; height: ${btnH}`);
    // Material via objeto (não string): o data URL tem ";" e "," que quebrariam o parser.
    button.setAttribute("material", {
      shader: "flat",
      src: makeLabelTexture(item.title),
      transparent: true,
      side: "double"
    });
    button.setAttribute("position", `${x} ${y} 0`);
    button.addEventListener("click", () => {
      selectVideo(item);
      hideMenu();
    });
    button.addEventListener("mouseenter", () => button.setAttribute("scale", "1.07 1.07 1.07"));
    button.addEventListener("mouseleave", () => button.setAttribute("scale", "1 1 1"));
    vrMenu.appendChild(button);
  });

  console.info("[Mergulho360] VR menu built", items.length);
}

function showMenu() {
  if (vrMenu) vrMenu.setAttribute("visible", true);
  if (vrMenuToggle) vrMenuToggle.setAttribute("visible", false);
}

function hideMenu() {
  if (vrMenu) vrMenu.setAttribute("visible", false);
  if (vrMenuToggle) vrMenuToggle.setAttribute("visible", true);
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

// Pré-seleciona a primeira experiência para o 360 já aparecer.
// Espera a cena A-Frame inicializar antes de mexer no <a-sky>.
function autoSelectFirst() {
  if (currentItems.length > 0) {
    selectVideo(currentItems[0]);
  }
}

const catalog = await loadCatalog();
renderCatalog(catalog);

// Botão "Menu" (dentro do VR) reabre o menu enquanto um vídeo toca.
if (vrMenuToggle) {
  vrMenuToggle.addEventListener("click", showMenu);
}

if (scene && scene.hasLoaded) {
  autoSelectFirst();
} else if (scene) {
  scene.addEventListener("loaded", autoSelectFirst, { once: true });
}
