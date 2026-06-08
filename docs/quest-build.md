# Build Android / Meta Quest 3

## Pré-requisitos

- `unity-app/` aberto no Unity.
- Android Build Support instalado.
- Quest 3 em Developer Mode.
- OpenXR configurado para Android.
- Vídeo 360 real curto e autorizado para teste técnico.

## Passos esperados

1. Abrir `unity-app/` no Unity.
2. Abrir `File > Build Settings`.
3. Selecionar `Android`.
4. Clicar em `Switch Platform`.
5. Confirmar cenas:
   - `Assets/_Project/Scenes/AppStart.unity`
   - `Assets/_Project/Scenes/MainMenu.unity`
   - `Assets/_Project/Scenes/Video360Player.unity`
6. Abrir `Project Settings > XR Plug-in Management`.
7. Ativar OpenXR para Android.
8. Revisar `Player Settings`.
9. Gerar APK.
10. Instalar no Quest 3.
11. Validar `docs/validation-checklist.md`.

## Checklist de build

- [ ] Android selecionado.
- [ ] Cenas adicionadas.
- [ ] OpenXR ativo.
- [ ] Input de controlador configurado.
- [ ] Vídeo real ou placeholder disponível.
- [ ] APK instala.
- [ ] App abre.
- [ ] Menu e player funcionam.

## Importante

Não considerar o app validado sem teste no Quest 3 real.
