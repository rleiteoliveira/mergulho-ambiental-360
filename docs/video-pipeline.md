# Pipeline de Vídeos 360

Este documento complementa `docs/video-requirements.md`.

## Pastas

Área organizada da PoC Unity:

```text
unity-app/Assets/_Project/StreamingAssets/Videos/
```

Pasta runtime convencional do Unity, se usada na implementação final:

```text
unity-app/Assets/StreamingAssets/Videos/
```

Web demo:

```text
web-demo/public/videos/
```

## Catálogo

Use `sourceType`:

```json
{
  "id": "recifes-demo",
  "title": "Recifes Demo",
  "description": "Vídeo autorizado para teste.",
  "category": "Recifes",
  "sourceType": "LocalFile",
  "localFileName": "recifes-demo.mp4",
  "streamingUrl": "",
  "thumbnailName": "recifes-demo.png",
  "durationLabel": "3 min",
  "isEnabled": true
}
```

Para streaming:

```json
{
  "id": "noronha-stream",
  "title": "Noronha Stream",
  "description": "Vídeo remoto autorizado.",
  "category": "Território",
  "sourceType": "StreamingUrl",
  "localFileName": "",
  "streamingUrl": "https://exemplo-autorizado/video360.mp4",
  "thumbnailName": "noronha.png",
  "durationLabel": "5 min",
  "isEnabled": true
}
```

## Cuidados

- Não baixar vídeos externos automaticamente.
- Não usar conteúdo sem autorização.
- Não commitar vídeos grandes sem decisão explícita.
- Validar codec, resolução, bitrate, orientação, áudio e conforto no Quest 3.
