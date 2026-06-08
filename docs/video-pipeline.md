# Pipeline de Videos 360

## Preparacao

Use apenas videos com autorizacao clara de uso. Nao inclua conteudo protegido por direitos autorais sem permissao formal.

Recomendacoes iniciais:

- Video equiretangular 360.
- Codec H.264 ou H.265, conforme suporte e teste no Quest 3.
- Audio AAC quando aplicavel.
- Duracao curta para exposicoes infantis.
- Arquivos otimizados para reduzir carregamento, armazenamento e risco de queda de FPS.

## Onde colocar arquivos

Para a PoC:

```text
Assets/StreamingAssets/Videos/
```

O catalogo mockado fica em:

```text
Assets/StreamingAssets/video_catalog_mock.json
```

## Configuracao no catalogo

Exemplo com arquivo local:

```json
{
  "id": "recifes-demo",
  "title": "Recifes Demo",
  "description": "Video autorizado para teste.",
  "category": "Recifes",
  "videoSourceType": "LocalFile",
  "localFileName": "recifes-demo.mp4",
  "streamingUrl": "",
  "thumbnail": "recifes-demo.png",
  "durationLabel": "3 min",
  "isEnabled": true
}
```

Exemplo com streaming:

```json
{
  "id": "noronha-stream",
  "title": "Noronha Stream",
  "description": "Video remoto autorizado.",
  "category": "Territorio",
  "videoSourceType": "StreamingUrl",
  "localFileName": "",
  "streamingUrl": "https://exemplo-autorizado/video360.mp4",
  "thumbnail": "noronha.png",
  "durationLabel": "5 min",
  "isEnabled": true
}
```

## LocalFile, StreamingAssets e StreamingUrl

- `LocalFile`: arquivo dentro de `Assets/StreamingAssets/Videos`.
- `StreamingAssets`: pasta Unity empacotada no build. Em Android, arquivos grandes podem exigir estrategia propria de copia, download autorizado ou armazenamento externo.
- `StreamingUrl`: URL remota. Exige rede, autorizacao de uso e testes de estabilidade.
- `Placeholder`: item visivel no menu sem reproducao real.

## Cuidados

- Nao baixe videos externos automaticamente.
- Nao trate videos de terceiros como assets finais.
- Nao commite arquivos grandes reais sem decisao explicita.
- Teste orientacao, escala e costura do video 360 no headset real.
