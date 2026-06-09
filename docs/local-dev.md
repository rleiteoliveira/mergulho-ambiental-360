# Ambiente local de desenvolvimento (Windows 11)

Guia prático para rodar e estudar a PoC **Mergulho Ambiental 360** localmente.

A PoC tem duas frentes:

- `web-demo/`: demo web sem framework, para alinhar catálogo/menu (roda em qualquer máquina).
- `unity-app/`: app nativo Unity para Meta Quest 3 (hipótese principal da exposição).

> Esta é uma PoC de descoberta, não um produto final. Veja `.agent/decision-log.md`.

## 1. Pré-requisitos recomendados

Essenciais para a web demo:

- **Git** (com **Git LFS** para assets binários: imagens, áudio).
- **Python 3** (apenas para servir a web demo com `http.server`).

Para a parte Unity (Meta Quest 3):

- **Unity Hub**.
- **Unity 2022.3 LTS** (ou outra LTS validada para Meta Quest/OpenXR).
- **Android Build Support** (com OpenJDK e Android SDK/NDK) para a versão do Unity escolhida.
- **Visual Studio** ou **Visual Studio Code** (edição de scripts C#).

Opcionais:

- **Docker Desktop** — alternativa para servir a web demo sem instalar Python.
- **WSL** (Windows Subsystem for Linux) — ambiente Linux auxiliar.
- **GitHub CLI (`gh`)** — para abrir PRs.

> Docker e WSL são **opcionais**. A web demo funciona só com Python, e o Unity deve
> ser aberto pelo Windows/Unity Hub.

## 2. Como clonar

```powershell
git clone https://github.com/rleiteoliveira/mergulho-ambiental-360.git
cd mergulho-ambiental-360
git lfs install
git lfs pull
```

Em seguida, rode a checagem de ambiente (não quebra se Docker/WSL faltarem):

```powershell
.\tools\check-dev-env.ps1
```

## 3. Como rodar a web demo (Python)

```powershell
cd web-demo
python -m http.server 8080
```

Abra no navegador:

```text
http://localhost:8080
```

**Não abra o `index.html` por duplo clique** (`file://`): a maioria dos navegadores
bloqueia o `fetch` local do catálogo e a página cai no fallback de 1 item. Sempre use
um servidor local. Detalhes em [../web-demo/README.md](../web-demo/README.md).

## 4. Como rodar a web demo com Docker (opcional)

Mantemos um `Dockerfile` simples (Nginx Alpine) em `web-demo/` e um `docker-compose.yml`
na raiz. Não há Node, framework ou backend.

```powershell
docker compose up --build web-demo
```

Abra:

```text
http://localhost:8080
```

Para validar a configuração e construir a imagem sem subir o container:

```powershell
docker compose config
docker compose build web-demo
```

Para parar:

```powershell
docker compose down
```

> Vídeos pesados em `web-demo/public/videos/` são ignorados pelo Git **e** pelo build
> Docker (ver `web-demo/.dockerignore`), então a imagem é leve e funciona sem vídeos.

## 5. Usando WSL (opcional)

Não assuma que o WSL está instalado. Se estiver, é possível servir a web demo de dentro
do Linux apontando para o repositório no disco do Windows:

```bash
cd /mnt/c/caminho/do/repo/mergulho-ambiental-360/web-demo
python3 -m http.server 8080
```

Observações:

- Se a porta 8080 já estiver em uso, use outra, por exemplo `8081`:
  `python3 -m http.server 8081`.
- Acesse pelo navegador do **Windows** em `http://localhost:8080`
  (o WSL2 encaminha `localhost` automaticamente).
- Para o **Unity**, prefira abrir pelo **Windows/Unity Hub**, não pelo WSL.
  A toolchain Android/XR e o headset são gerenciados melhor no Windows.

## 6. Como abrir a parte Unity

1. Abra o **Unity Hub**.
2. **Add project from disk** e selecione a pasta `unity-app/`.
3. Use **Unity 2022.3 LTS** (ou uma LTS validada para Meta Quest/OpenXR).
4. Garanta **Android Build Support** instalado para essa versão.
5. Aguarde a restauração dos pacotes.
6. Gere as cenas base pelo menu:
   `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
7. Abra `unity-app/Assets/_Project/Scenes/AppStart.unity` quando as cenas existirem.

Pacotes XR recomendados: OpenXR, XR Plug-in Management, XR Interaction Toolkit e,
se necessário, Meta XR SDK.

## 7. Como validar rapidamente

- **Web demo**: mostra **4 cards** reais do catálogo (não apenas o fallback).
- **Unity**: o projeto abre sem erros de compilação.
- **Cenas base**: geradas pelo menu `Create or Refresh Base Scenes`.
- **Player**: aceita o placeholder no Editor.
- **Vídeo real**: usar apenas um vídeo 360 **curto e autorizado**, somente se autorizado.
  Não baixe nem commite vídeos pesados.

## 8. Git LFS

Git LFS está habilitado para assets binários comuns (imagens, áudio) via `.gitattributes`.
Antes de trabalhar em outra máquina:

```powershell
git lfs install
git lfs pull
```

Mesmo com LFS, vídeos 360 reais tendem a ser grandes demais para o fluxo normal do
repositório. Por isso `*.mp4/.mov/.mkv/.avi/.webm` são ignorados no `.gitignore`.
Avalie hospedagem dedicada caso a caso.

## O que ainda depende do Quest 3 físico

Conforto, input real, performance, legibilidade do menu para crianças e orientação do
vídeo 360 só podem ser validados no headset. Veja `docs/validation-checklist.md` e
`docs/quest-testing.md`.
