# Requisitos de Vídeo

A PoC deve aceitar placeholders. Não é necessário ter vídeos reais para validar estrutura de catálogo e menu.

## Regras

- Não baixar vídeos de terceiros automaticamente.
- Não commitar vídeos pesados sem decisão explícita.
- Vídeos reais precisam de autorização de uso.
- Usar fontes mockadas enquanto não houver conteúdo autorizado.

## Pastas

Para Unity:

```text
unity-app/Assets/_Project/StreamingAssets/Videos/
```

Para a web demo:

```text
web-demo/public/videos/
```

Observação técnica: em Unity, a pasta runtime convencional é `Assets/StreamingAssets`. A pasta dentro de `_Project` fica como área organizada da PoC e pode ser ajustada quando o pipeline real de build for definido.

## Catálogo

O catálogo deve indicar:

- `id`;
- `title`;
- `description`;
- `category`;
- `sourceType`;
- `localFileName`;
- `streamingUrl`;
- `thumbnailName`;
- `durationLabel`;
- `isEnabled`.

## Pontos a validar

- Resolução.
- Codec.
- Bitrate.
- Tamanho de arquivo.
- Áudio.
- Duração.
- Orientação.
- Monoscópico ou estereoscópico.
- Carregamento local vs streaming.
- Impacto em armazenamento e performance no Quest 3.
