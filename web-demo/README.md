# Web Demo

Demo web simples para comparar fluxo de catálogo e menu antes de investir em uma solução nativa completa.

Esta versão não é WebXR completo e não substitui teste no Meta Quest 3. Ela serve para:

- alinhar organização de vídeos;
- mostrar cards mockados;
- testar uma experiência visual simples;
- discutir com o cliente se web ajuda como apoio ou preview.

## Como rodar

```powershell
cd web-demo
python -m http.server 8080
```

Abra:

```text
http://localhost:8080
```

## Limitações

- O player é HTML5 comum.
- Vídeo 360 real exigirá biblioteca específica ou implementação com Three.js/A-Frame/WebXR.
- Placeholders não reproduzem vídeo real.
- Compatibilidade em Quest Browser precisa ser testada separadamente.

## Próxima etapa se web for relevante

- Testar A-Frame ou Three.js para esfera 360.
- Avaliar cache/offline.
- Testar no Quest Browser.
- Comparar estabilidade com o app Unity.
