# Ambiente Local de Desenvolvimento

## Estado configurado

- Repositório Git local.
- Remoto GitHub `origin` configurado.
- Branch principal `main`.
- PoC de descoberta em branch local quando criada.
- Git LFS habilitado para assets binários comuns.
- `.gitattributes` para normalização de texto e LFS.
- `.editorconfig` para edição consistente.
- Script de verificação em `tools/check-dev-env.ps1`.

## Checagem rápida

No PowerShell, rode:

```powershell
.\tools\check-dev-env.ps1
```

## Estrutura atual

- `unity-app/`: projeto Unity nativo para Quest 3.
- `web-demo/`: demo web simples para comparação.
- `docs/`: documentação de decisão.
- `.agent/`: contexto para agentes.

## Fluxo recomendado

1. Rode a web demo para avaliar catálogo e linguagem visual.
2. Abra `unity-app/` no Unity Hub.
3. Use Unity 2022.3 LTS ou uma LTS validada para Meta Quest/OpenXR.
4. Configure Android Build Support e OpenXR.
5. Teste com placeholder no Editor.
6. Teste com um vídeo 360 real curto e autorizado.
7. Gere APK.
8. Teste no Quest 3 físico.

## Git LFS

Antes de trabalhar em outra máquina:

```powershell
git lfs install
git lfs pull
```

Mesmo com LFS, vídeos 360 reais podem ser grandes demais para o fluxo normal de repositório. Avalie caso a caso.
