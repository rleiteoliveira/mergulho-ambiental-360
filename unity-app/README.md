# Unity App

Base nativa da PoC para Meta Quest 3.

Esta pasta deve ser aberta como projeto Unity pelo Unity Hub. O objetivo é validar menu + catálogo + player 360, não construir um jogo completo.

## Como criar/abrir manualmente

1. Abra o Unity Hub.
2. Selecione `Add project from disk`.
3. Escolha a pasta `unity-app/`.
4. Use Unity 2022.3 LTS ou uma LTS validada para Meta Quest/OpenXR.
5. Instale Android Build Support na mesma versão do Unity.
6. Aguarde os pacotes serem restaurados.

## Pacotes recomendados

- XR Interaction Toolkit.
- OpenXR.
- XR Plug-in Management.
- Meta XR SDK, se recursos específicos da Meta forem necessários.
- Android Build Support.

## Cenas sugeridas

Criar ou gerar:

- `MainMenu`: menu com cards grandes.
- `Video360Player`: player com esfera 360 e controles básicos.

O scaffold atual também possui script de Editor para gerar cenas base:

```text
Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes
```

## Como adicionar uma esfera para vídeo 360

1. Crie uma esfera grande ao redor da câmera.
2. Inverta a escala no eixo X ou use normals invertidas.
3. Use material `Unlit/Texture`.
4. Configure o `VideoPlayer` para renderizar no material da esfera.
5. Posicione a câmera no centro.

## Como configurar VideoPlayer

- Use `UnityEngine.Video.VideoPlayer`.
- Para arquivo local, use catálogo com `sourceType = LocalFile`.
- Para URL autorizada, use `sourceType = StreamingUrl`.
- Para placeholder, use `sourceType = Placeholder`.

## Como testar sem headset

- Rodar no Unity Editor.
- Clicar no menu com mouse.
- Usar atalhos do player quando disponíveis.
- Confirmar que catálogo e seleção funcionam.

## Como testar no Quest 3

1. Configurar Android/OpenXR.
2. Gerar build Android.
3. Instalar no Quest 3 em Developer Mode.
4. Testar checklist em `../docs/validation-checklist.md`.

## Limites da PoC

- Sem backend.
- Sem multiplayer.
- Sem coleta de dados.
- Sem quiz.
- Sem assets finais.
- Sem promessa de conforto/performance até teste no headset real.
