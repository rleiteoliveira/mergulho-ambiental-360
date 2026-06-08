# Performance

## Metas

- Manter FPS estavel no Meta Quest 3.
- Minimizar travamentos no carregamento do video.
- Usar UI simples e legivel.
- Evitar pos-processamento pesado.
- Evitar assets, materiais e texturas desnecessariamente grandes.

## Videos 360

Videos muito grandes podem impactar performance, carregamento e armazenamento. Antes de validar a experiencia:

- Teste diferentes resolucoes e bitrates.
- Prefira duracoes curtas para exposicoes infantis.
- Comprima sem destruir legibilidade do conteudo.
- Teste codec e audio no headset real.
- Avalie carregamento local versus streaming.

## Cena e renderizacao

- Use materiais simples.
- Reduza draw calls quando possivel.
- Evite luzes dinamicas desnecessarias.
- Evite sombras pesadas.
- Evite particulas e efeitos visuais na PoC.
- Use uma esfera/cupula 360 simples.

## UI

- Botoes grandes.
- Alto contraste.
- Poucos elementos simultaneos.
- Texto curto.
- Evitar animacoes excessivas.

## Validacao

A performance real deve ser medida no Quest 3. O Editor nao substitui teste de FPS, input, conforto e estabilidade em headset.
