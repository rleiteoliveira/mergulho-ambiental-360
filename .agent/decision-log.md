# Decision Log

| Decisão | Status | Motivo | Pendência |
| --- | --- | --- | --- |
| App Unity é hipótese principal. | Hipótese | Exposição infantil tende a exigir controle, estabilidade e offline. | Validar no Quest 3 real. |
| Web demo será apoio/comparação. | Definido para PoC | Ajuda a discutir catálogo e fluxo rapidamente. | Testar se cliente valoriza preview web. |
| Backend fora do escopo. | Definido para PoC | Catálogo local basta para validar menu + player. | Reavaliar só se houver atualização remota de conteúdo. |
| Quest 3 real é obrigatório para validação final. | Definido | Conforto, input, performance e legibilidade não são garantidos no Editor. | Agendar teste físico. |
| Catálogo da web demo canonizado em `web-demo/src/video-catalog.json`. | Aplicado | `main.js` passou a usar `fetch("src/video-catalog.json")` (caminho relativo ao `index.html`). | Remover o duplicado órfão `web-demo/video-catalog.json` (raiz) para evitar drift. |
| Docker é alternativa OPCIONAL para servir a web demo. | Definido para PoC | `web-demo/Dockerfile` (Nginx Alpine) + `docker-compose.yml` equivalem a `python -m http.server`. Sem Node/framework/backend. | Nenhuma; Python segue como caminho padrão. |
| WSL é ambiente auxiliar opcional. | Definido para PoC | Pode servir a web demo via `python3`; não muda a estratégia. | Unity deve ser aberto pelo Windows/Unity Hub, não pelo WSL. |
| Unity segue como hipótese principal; web demo como apoio/comparação. | Reafirmado (depois revisado, ver abaixo) | Exposição infantil tende a exigir controle, estabilidade e offline; a web ajuda no alinhamento de catálogo/fluxo. | Validar no headset real. |
| Player 360 implementado na web demo via WebXR (A-Frame). | Aplicado | Permite testar 360 sem headset (giroscópio no celular, arrastar no desktop) e roda em headsets pelo navegador. | Validar com vídeo 360 real curto e autorizado. |
| Web/WebXR passa a ser o caminho principal da PoC; Unity nativo vira opção futura. | Revisado | Aparelho de teste disponível é um headset PCVR (Oculus Rift de terceiro) e o dev só tem PC/celular; WebXR roda no navegador do PC com o headset e é portátil entre dispositivos. Também é peça de portfólio. | Confirmar modelo do Rift/PC; reabrir Unity só se a validação exigir. |
| Deploy da web demo via GitHub Pages + Actions. | Definido | Repo pode ser público; mostra CI/CD no portfólio; HTTPS grátis (necessário para giroscópio no celular). | Tornar o repo público e ativar Pages (Source = GitHub Actions). |

## Notas

### 2026-06-08 — Correção do carregamento do catálogo da web demo

- **Investigação:** o `fetch` relativo resolve contra o documento (`index.html`),
  servido na raiz de `web-demo/`. O código antigo `fetch("video-catalog.json")`
  buscava `web-demo/video-catalog.json` (raiz). Esse arquivo **existe** e é idêntico
  ao de `web-demo/src/`, então via `python -m http.server` a demo já carregava 4 itens;
  o fallback de 1 item aparecia ao abrir o `index.html` via `file://` (navegador
  bloqueia `fetch` local).
- **Decisão:** canonizar o catálogo em `web-demo/src/video-catalog.json` e apontar o
  `fetch` para `src/video-catalog.json` (correção mínima no JS, sem mover o arquivo).
- **Pendência registrada:** o arquivo `web-demo/video-catalog.json` (raiz) ficou órfão
  e duplicado. Recomenda-se removê-lo para evitar edições que não surtem efeito. Não foi
  removido nesta passagem por ser exclusão não solicitada — aguardando confirmação.
- **Observabilidade:** adicionados logs `console.info`/`console.warn` com prefixo
  `[Mergulho360]` para diferenciar catálogo real, fallback e nº de cards renderizados.

### 2026-06-11 — Virada para WebXR e deploy no GitHub Pages

- **Contexto novo:** o desenvolvedor não tem headset próprio e não pretende comprar. O
  aparelho de teste disponível é um **Oculus Rift (PCVR, de terceiro)**, com possibilidade de
  empréstimo futuro. Dev diário só com PC + celular. O projeto também é peça de **portfólio**.
- **Decisão:** liderar a PoC pela **web/WebXR** (A-Frame). Motivos: roda no PC/celular para o
  dev diário, roda no Rift pelo **navegador do PC** (sem build/instalação) e é portátil (o
  mesmo site roda em Rift/Quest/celular). Unity nativo vira opção futura (exigiria build
  PCVR/OpenXR e o headset em mãos para iterar).
- **Player 360:** implementado com A-Frame na web demo. Placeholder 360 gerado por código
  (sem baixar vídeo, sem asset protegido); vídeo real via `LocalFile`/`StreamingUrl`.
- **Deploy:** GitHub Pages via Actions (`.github/workflows/deploy-pages.yml`), publicando só
  `web-demo/`. Repo público (decisão do dev) → Pages grátis + HTTPS (necessário para o
  giroscópio no celular). Vercel permanece como alternativa.
- **Pendência:** confirmar modelo do Rift (CV1/S) e o PC do dono; ativar Pages.
