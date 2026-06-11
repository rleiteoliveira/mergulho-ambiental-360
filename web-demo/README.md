# Web Demo

Demo web para o catálogo, o menu e um **player 360 em WebXR** (via A-Frame) do Mergulho
Ambiental 360. Roda no navegador do PC, no celular (giroscópio) e em headsets via WebXR.

Serve para:

- alinhar organização de vídeos e linguagem visual;
- testar a sensação de "olhar em volta" do 360 sem depender de headset;
- discutir com o cliente e usar como peça de portfólio.

> Não substitui a validação final no headset (conforto, input, performance). Veja "Limitações".

## Como rodar (Python)

```powershell
cd web-demo
python -m http.server 8080
```

Abra `http://localhost:8080`.

> Alternativa opcional com Docker e deploy (GitHub Pages) em [../docs/local-dev.md](../docs/local-dev.md).

## Por que não abrir o index.html diretamente

Abrir o `index.html` por duplo clique usa `file://`, e os navegadores **bloqueiam o
`fetch` local** do catálogo (CORS). A página cai no catálogo fallback (1 item). Sempre
use um servidor local e acesse via `http://localhost:8080`.

## Player 360 (como funciona)

- Usa **A-Frame** (um único `<script>` via CDN, sem build step) para projetar o vídeo
  numa esfera 360.
- **Celular:** mova o aparelho para olhar em volta (giroscópio / "magic window").
- **Desktop:** arraste com o mouse.
- **VR:** toque no ícone de óculos para entrar em modo imersivo (WebXR).
- Itens `Placeholder` mostram uma cena 360 **gerada por código** (sem baixar vídeo), com
  marcadores de direção e o nome da experiência — o suficiente para sentir o "olhar em volta".
- ⚠️ O giroscópio do celular exige **HTTPS** (é bloqueado em `http`). Veja "Testar no celular".

## Onde fica o catálogo

Catálogo canônico: `web-demo/src/video-catalog.json`. O `src/main.js` o carrega via
`fetch("src/video-catalog.json")`. Em caso de erro, usa um fallback embutido de 1 item.

Logs no console do navegador (F12):

- `[Mergulho360] Catalog loaded N (src/video-catalog.json)` — catálogo real;
- `[Mergulho360] Using fallback catalog ...` — fallback por erro;
- `[Mergulho360] Rendered cards N` — cards renderizados.

## Como adicionar um vídeo 360 local de teste

1. Coloque o arquivo em `web-demo/public/videos/`.

   > Vídeos pesados (`*.mp4/.mov/.mkv/.avi/.webm`) são **ignorados pelo Git** e não devem
   > ser commitados. Use apenas clipes curtos e autorizados, só para teste local.

2. Configure o item no catálogo para `LocalFile`:

   ```json
   {
     "id": "exemplo-local",
     "title": "Exemplo local",
     "description": "Vídeo 360 servido localmente.",
     "category": "Teste",
     "sourceType": "LocalFile",
     "localFileName": "nome-do-video.mp4",
     "streamingUrl": "",
     "thumbnailName": "",
     "durationLabel": "1 min",
     "isEnabled": true
   }
   ```

   O vídeo precisa ser **equiretangular (360 mono)** para a esfera ficar correta.

## Como configurar streaming

```json
{ "sourceType": "StreamingUrl", "streamingUrl": "https://exemplo.com/video360.mp4" }
```

Use `sourceType: "Placeholder"` (campos de fonte vazios) para itens sem vídeo ainda —
nesse caso mostra a cena 360 gerada por código.

## Testar no celular (HTTPS obrigatório)

O giroscópio só funciona em HTTPS. Duas formas:

- **Rápido (sem deploy):** sirva localmente (`python -m http.server 8080`) e exponha a
  porta por HTTPS — ex.: **VS Code → painel "Ports" → Forward a Port → 8080 →
  visibilidade Public**. Abra a URL `https://...devtunnels.ms` no celular.
- **Durável (portfólio):** deploy no GitHub Pages (abaixo) e abra a URL `github.io` no celular.

No **iPhone**, toque no botão de permissão de sensores de movimento que o A-Frame mostra.
No **Android** (ex.: Samsung Galaxy), o Chrome costuma liberar o giroscópio direto em HTTPS.

## Deploy (GitHub Pages)

O workflow [`.github/workflows/deploy-pages.yml`](../.github/workflows/deploy-pages.yml)
publica **apenas** esta pasta `web-demo/` no GitHub Pages a cada push na `main`.
Requer repositório público (ou GitHub Pro) e `Settings > Pages > Source = "GitHub Actions"`.
Passo a passo em [../docs/local-dev.md](../docs/local-dev.md).

## Limitações

- No celular sem suporte, é "magic window" (não estéreo). Para estéreo, use um Google
  Cardboard (~R$50) + o botão de VR.
- Conforto, input por controle e performance só se validam **no headset real**.
- A-Frame é carregado via CDN (precisa de internet). Para uso offline em exposição, será
  necessário empacotar a biblioteca localmente.

## Próxima etapa se web for o caminho

- Testar um vídeo 360 real curto e autorizado.
- Avaliar estéreo (360 3D) e legendas/áudio.
- Testar no headset disponível via WebXR (navegador do PC com o headset conectado).
- Avaliar empacotamento offline para exposição.
