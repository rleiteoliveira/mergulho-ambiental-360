# Setup

## Pré-requisitos

- Windows 11.
- Git.
- Git LFS.
- GitHub CLI, se for publicar ou abrir PRs.
- Unity Hub.
- Unity 2022.3 LTS ou uma LTS validada com Meta Quest/OpenXR.
- Android Build Support para a versão do Unity escolhida.

## Estrutura

- `unity-app/`: projeto Unity.
- `web-demo/`: demo web sem framework.
- `docs/`: documentos de decisão e validação.
- `.agent/`: contexto para agentes.

## Como rodar web demo

```powershell
cd web-demo
python -m http.server 8080
```

Abra `http://localhost:8080`.

## Como abrir Unity

1. Abra Unity Hub.
2. Adicione a pasta `unity-app/`.
3. Aguarde a restauração de pacotes.
4. Execute `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`, se quiser gerar cenas base.
5. Abra `unity-app/Assets/_Project/Scenes/AppStart.unity`.
6. Rode no Editor.

## Pacotes XR recomendados

- OpenXR.
- XR Plug-in Management.
- XR Interaction Toolkit.
- Meta XR SDK, se houver necessidade de recursos específicos da Meta.

## Observação

Sem Quest 3 físico, validar apenas estrutura, fluxo básico, catálogo e fallback de interação. Conforto, input real, performance e legibilidade precisam do headset.
