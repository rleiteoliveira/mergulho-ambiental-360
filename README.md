# Mergulho Ambiental 360

PoC de aplicacao VR para Meta Quest 3 voltada a educacao ambiental marinha em exposicoes com criancas. A primeira versao e um player 360 educativo: menu imersivo simples, cards grandes para escolha de videos e reproducao em uma esfera 360.

## Objetivo

Criar uma base Unity simples, estavel e evolutiva para selecionar e assistir experiencias 360 sobre temas como peixe-boi, plantas marinhas, Fernando de Noronha, recifes, manguezais, oceano, conservacao e biodiversidade local.

## Escopo da PoC

- Cena `AppStart` para bootstrap, carregamento de catalogo e entrada no menu.
- Cena `MainMenu` com titulo, subtitulo, cards de videos, configuracoes e sair.
- Cena `Video360Player` com `UnityEngine.Video.VideoPlayer`, esfera 360 invertida e controles basicos.
- Catalogo local mockado em `Assets/StreamingAssets/video_catalog_mock.json`.
- Scripts C# base para catalogo, menu, navegacao, player, input e UI.
- Estrutura preparada para OpenXR, XR Interaction Toolkit e build Android/Meta Quest 3.
- Sem backend, multiplayer, coleta de dados ou assets externos baixados automaticamente.

## O que funciona sem Quest 3

- Abrir o projeto no Unity Editor.
- Gerar as cenas base pelo menu `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
- Rodar `Assets/_Project/Scenes/AppStart.unity` no Editor.
- Navegar com mouse pela UI padrao.
- Testar atalhos no player: `Space` para play/pause, `Esc` para voltar, `N` para proximo video e `R` para reiniciar.
- Validar catalogo mockado e fluxo basico de cenas.

## O que precisa ser validado no Quest 3 real

A validacao final de conforto, performance, input, legibilidade e usabilidade infantil precisa ser feita no headset real. Em especial:

- FPS e estabilidade em video 360.
- Legibilidade da UI dentro do headset.
- Tamanho dos botoes para criancas e operacao por professor.
- Ray/pointer dos controladores.
- Audio e orientacao correta do video 360.
- Conforto, ausencia de tontura e tempo adequado de sessao.

## Como abrir no Unity

1. Abra o Unity Hub.
2. Selecione `Add project from disk`.
3. Escolha esta pasta do repositorio.
4. Use Unity 2022.3 LTS ou uma versao LTS mais recente validada para Meta Quest/OpenXR.
5. Aguarde a restauracao dos pacotes.
6. Execute `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
7. Abra `Assets/_Project/Scenes/AppStart.unity`.
8. Pressione Play.

## Ambiente local

Rode a checagem local:

```powershell
.\tools\check-dev-env.ps1
```

Veja detalhes em [docs/local-development.md](docs/local-development.md).

## Como adicionar videos 360

1. Tenha certeza de que o video e autorizado para uso no projeto.
2. Copie o arquivo para `Assets/StreamingAssets/Videos/`.
3. Edite `Assets/StreamingAssets/video_catalog_mock.json`.
4. Defina `videoSourceType` como `LocalFile`.
5. Preencha `localFileName` com o nome do arquivo.
6. Mantenha `isEnabled` como `true`.

Para streaming, defina `videoSourceType` como `StreamingUrl` e preencha `streamingUrl`. Evite URLs de terceiros sem autorizacao formal.

## Como gerar build Android/Quest

1. Instale Android Build Support no Unity Hub.
2. No Unity, abra `File > Build Settings`.
3. Selecione `Android` e clique em `Switch Platform`.
4. Confirme que as cenas `AppStart`, `MainMenu` e `Video360Player` estao na lista.
5. Configure XR/OpenXR em `Project Settings > XR Plug-in Management`.
6. Ative OpenXR para Android e configure recursos compativeis com Meta Quest.
7. Configure `Player Settings` para Android/Quest.
8. Gere APK/AAB ou faca Build and Run com o Quest 3 conectado em Developer Mode.

Veja detalhes em [docs/quest-build.md](docs/quest-build.md).

## Roadmap inicial

- Fase 0: setup e estrutura.
- Fase 1: menu + player 360.
- Fase 2: teste no Quest 3.
- Fase 3: identidade visual e videos reais.
- Fase 4: modo professor.
- Fase 5: quizzes, narracao e melhorias educativas.

## Limitacoes conhecidas

- As cenas sao geradas por script no Unity Editor; elas ainda nao existem ate o comando do menu ser executado.
- O catalogo inicial usa placeholders e nao reproduz videos reais.
- Ray/pointer VR esta preparado conceitualmente, mas precisa de configuracao e teste final com XR Interaction Toolkit/Meta XR no headset.
- Videos em `StreamingAssets` podem exigir ajustes de empacotamento em Android conforme tamanho, codec e estrategia de distribuicao.
- Nao ha backend, analytics, multiplayer, quiz ou coleta de dados nesta PoC.

## GitHub

Repositorio remoto privado:

https://github.com/rleiteoliveira/mergulho-ambiental-360
