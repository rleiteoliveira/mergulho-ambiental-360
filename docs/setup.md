# Setup

## Pre-requisitos

- Windows 11.
- Unity Hub.
- Unity 2022.3 LTS ou versao LTS mais recente validada com Meta Quest/OpenXR.
- Android Build Support instalado pelo Unity Hub.
- Git.

## Pacotes XR recomendados

O `Packages/manifest.json` ja inclui:

- OpenXR.
- XR Plug-in Management.
- XR Interaction Toolkit.
- Input System.
- Universal Render Pipeline.

O Meta XR SDK pode ser adicionado depois quando a validacao em headset com recursos especificos da Meta for necessaria. Evite adicionar pacotes grandes sem necessidade na PoC.

## Como configurar

1. Abra a pasta do repositorio no Unity Hub.
2. Aguarde o Unity restaurar os pacotes.
3. Rode `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
4. Abra `Assets/_Project/Scenes/AppStart.unity`.
5. Pressione Play.

## Como rodar no Editor

- Use mouse para clicar nos cards e botoes.
- No player:
  - `Space`: play/pause.
  - `Esc`: voltar ao menu.
  - `N`: proximo video.
  - `R`: reiniciar.

## Observacoes

- Os videos reais ainda nao fazem parte da PoC.
- O catalogo inicial e mockado e usa `Placeholder`.
- A validacao de input VR precisa ser feita no Quest 3 real.
