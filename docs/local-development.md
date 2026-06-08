# Ambiente Local de Desenvolvimento

## Estado configurado

- Repositorio Git local em `main`.
- Remoto GitHub `origin` configurado.
- Git LFS habilitado para assets binarios comuns de Unity.
- `.gitattributes` configurado para normalizar texto em LF e versionar assets grandes via LFS.
- `.editorconfig` configurado para C#, JSON, Markdown, PowerShell e assets textuais Unity.
- Script de verificacao em `tools/check-dev-env.ps1`.

## Checagem rapida

No PowerShell, rode:

```powershell
.\tools\check-dev-env.ps1
```

## Fluxo recomendado

1. Abra o projeto pelo Unity Hub.
2. Use Unity 2022.3 LTS ou uma LTS validada para Meta Quest/OpenXR.
3. Deixe o Unity restaurar pacotes.
4. Execute `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
5. Abra `Assets/_Project/Scenes/AppStart.unity`.
6. Rode no Editor antes de tentar build Android.
7. Faca commits pequenos por etapa.
8. Nao adicione videos reais grandes no Git sem decisao explicita.

## Git LFS

O projeto rastreia imagens, audio, videos e binarios comuns via Git LFS. Antes de trabalhar em outra maquina, instale Git LFS e rode:

```powershell
git lfs install
git lfs pull
```

Mesmo com LFS, videos 360 reais podem ser grandes demais para o fluxo normal de repositorio. Avalie caso a caso.

## Unity

Ao abrir o projeto, confirme nas configuracoes do Editor:

- Version Control Mode: Visible Meta Files.
- Asset Serialization: Force Text.
- Plataforma Android instalada.
- OpenXR configurado para Android antes do build Quest.

Essas configuracoes devem ser validadas no Unity Editor porque variam por versao e por pacotes instalados.
