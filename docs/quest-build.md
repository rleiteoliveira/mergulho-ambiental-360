# Build Android / Meta Quest 3

## Pre-requisitos

- Android Build Support instalado no Unity.
- Meta Quest Developer Hub instalado, se usado no fluxo da equipe.
- Quest 3 em Developer Mode.
- Cabo USB-C ou fluxo de deploy configurado.
- Projeto aberto e cenas geradas.

## Passos esperados

1. Abra `File > Build Settings`.
2. Selecione `Android`.
3. Clique em `Switch Platform`.
4. Confirme as cenas:
   - `Assets/_Project/Scenes/AppStart.unity`
   - `Assets/_Project/Scenes/MainMenu.unity`
   - `Assets/_Project/Scenes/Video360Player.unity`
5. Abra `Project Settings > XR Plug-in Management`.
6. Ative OpenXR para Android.
7. Configure recursos OpenXR/Meta Quest conforme a versao instalada dos pacotes.
8. Em `Player Settings`, revise:
   - Package Name.
   - Minimum API Level.
   - Scripting Backend.
   - Target Architectures.
   - Graphics APIs.
   - Orientation.
9. Gere build.
10. Instale e teste no Quest 3 real.

## Checklist de build

- Android selecionado como plataforma.
- Cenas adicionadas ao build.
- OpenXR ativo para Android.
- Input configurado para controladores.
- Videos reais presentes ou catalogo em modo placeholder.
- APK instala no Quest 3.
- App abre em VR.
- Menu aparece legivel.
- Player abre o video selecionado.
- Audio funciona.
- Voltar ao menu funciona.

## Validacao real

Mesmo que o Editor funcione, o build so deve ser considerado validado apos teste no headset real. Conforto, performance, input, escala da UI e legibilidade infantil dependem do dispositivo.
