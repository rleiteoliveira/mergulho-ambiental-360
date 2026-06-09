# Web Demo

Demo web simples para comparar fluxo de catálogo e menu antes de investir em uma solução nativa completa.

Esta versão não é WebXR completo e não substitui teste no Meta Quest 3. Ela serve para:

- alinhar organização de vídeos;
- mostrar cards mockados;
- testar uma experiência visual simples;
- discutir com o cliente se web ajuda como apoio ou preview.

## Como rodar (Python)

A forma mais simples, sem instalar framework:

```powershell
cd web-demo
python -m http.server 8080
```

Depois abra no navegador:

```text
http://localhost:8080
```

> Alternativa opcional com Docker em [../docs/local-dev.md](../docs/local-dev.md).

## Por que não abrir o index.html diretamente

Abrir o arquivo `web-demo/index.html` com duplo clique usa o protocolo `file://`.
Nesse modo, a maioria dos navegadores **bloqueia o `fetch` local** do catálogo
por política de segurança (CORS). O resultado é que a página cai no **catálogo
fallback** (apenas 1 item) em vez de carregar o catálogo real.

Sempre use um servidor local (Python ou Docker) e acesse via `http://localhost:8080`.

## Onde fica o catálogo

O catálogo real (canônico) fica em:

```text
web-demo/src/video-catalog.json
```

O `web-demo/src/main.js` carrega esse arquivo via `fetch("src/video-catalog.json")`.
Se o `fetch` falhar (por exemplo, abrindo via `file://`), a demo usa um catálogo
fallback embutido com apenas 1 item, e o status mostra "Catálogo fallback".

Para depurar, abra o console do navegador (F12). Os logs ajudam a diferenciar:

- `[Mergulho360] Catalog loaded N (src/video-catalog.json)` — catálogo real carregado;
- `[Mergulho360] Using fallback catalog ...` — fallback por erro de `fetch`;
- `[Mergulho360] Rendered cards N` — número de cards renderizados.

## Como adicionar um vídeo local de teste

1. Coloque o arquivo de vídeo em:

   ```text
   web-demo/public/videos/
   ```

   > Vídeos pesados (`*.mp4`, `*.mov`, `*.mkv`, `*.avi`, `*.webm`) são **ignorados
   > pelo Git** (ver `.gitignore`). Eles servem apenas para teste local e **não**
   > devem ser commitados. A pasta é mantida no repositório por um `.gitkeep`.

2. Configure um item do catálogo em `web-demo/src/video-catalog.json` para usar o arquivo:

   ```json
   {
     "id": "exemplo-local",
     "title": "Exemplo local",
     "description": "Vídeo de teste servido localmente.",
     "category": "Teste",
     "sourceType": "LocalFile",
     "localFileName": "nome-do-video.mp4",
     "streamingUrl": "",
     "thumbnailName": "",
     "durationLabel": "1 min",
     "isEnabled": true
   }
   ```

## Como configurar streaming

Para apontar um item para uma URL externa autorizada:

```json
{
  "sourceType": "StreamingUrl",
  "streamingUrl": "https://exemplo.com/video.mp4"
}
```

Use `sourceType: "Placeholder"` (com `localFileName` e `streamingUrl` vazios) quando
o item ainda não tiver fonte de vídeo definida. Nesse caso o player mostra apenas a
descrição, sem reproduzir nada.

## Limitações

- O player é HTML5 comum (`<video>`). **Ainda não é player 360 / WebXR.**
- Vídeo 360 real exigirá biblioteca específica ou implementação com Three.js/A-Frame/WebXR.
- Placeholders não reproduzem vídeo real.
- Compatibilidade no Quest Browser precisa ser testada separadamente.

## Próxima etapa se web for relevante

- Testar A-Frame ou Three.js para esfera 360.
- Avaliar cache/offline.
- Testar no Quest Browser.
- Comparar estabilidade com o app Unity.
