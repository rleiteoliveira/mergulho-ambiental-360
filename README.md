# Mergulho Ambiental 360

PoC rápida para validar a viabilidade de uma experiência de educação ambiental em Meta Quest 3 baseada em seleção e reprodução de vídeos 360.

Esta PoC não é um produto completo. O foco é descobrir o que é realmente necessário, o que é assertivo para a proposta e quais decisões dependem de teste no headset real ou alinhamento com o cliente.

## Contexto

O cliente usa vídeos e experiências em exposições para crianças. Hoje usa um jogo comprado, mas quer algo próprio para educação ambiental marinha, com ideia de mergulho, peixe-boi, plantas marinhas, Fernando de Noronha e biodiversidade local.

A hipótese inicial é uma aplicação VR interativa para Meta Quest 3 com menu para selecionar e assistir vídeos 360. Ainda assim, precisamos comparar se o melhor caminho é:

- app nativo Unity para Quest 3;
- site/WebXR simples;
- solução híbrida.

## O que esta PoC pretende validar

- Se o app nativo Unity é realmente necessário.
- Se uma web demo poderia resolver parte da demanda.
- Como organizar vídeos 360 por catálogo.
- Como seria a experiência básica de menu + player.
- Quais pontos só podem ser validados no Quest 3 físico.
- Quais perguntas ainda precisam ser feitas ao cliente.

## Unity app vs web demo

`web-demo/` inclui um **player 360 em WebXR** (A-Frame): catálogo, menu e esfera 360 navegável no PC, no celular (giroscópio) e em headsets pelo navegador. É o caminho desenvolvido primeiro na PoC, por ser testável sem hardware próprio e portátil entre dispositivos (inclusive headsets PCVR via navegador do PC).

`unity-app/` segue como opção para um app nativo futuro (mais controle, uso offline e integração XR), mas exige build por dispositivo e o headset em mãos para iterar.

Recomendação atual (fase de PoC/portfólio): liderar pela web/WebXR e abrir o Unity nativo só se a validação indicar necessidade. Mudança registrada em [.agent/decision-log.md](.agent/decision-log.md).

## Como clonar e iniciar

```powershell
git clone https://github.com/rleiteoliveira/mergulho-ambiental-360.git
cd mergulho-ambiental-360
git lfs install
git lfs pull
.\tools\check-dev-env.ps1
```

## Como rodar a web demo

Sem instalar framework:

```powershell
cd web-demo
python -m http.server 8080
```

Depois abra:

```text
http://localhost:8080
```

Também é possível abrir `web-demo/index.html` diretamente, mas alguns navegadores bloqueiam `fetch` local (a página cai no catálogo fallback de 1 item). Use sempre o servidor simples.

Alternativa opcional com Docker (sem Python):

```powershell
docker compose up --build web-demo
```

Para testar no celular ou publicar, há deploy no **GitHub Pages** via Actions ([.github/workflows/deploy-pages.yml](.github/workflows/deploy-pages.yml)).

Detalhes de ambiente, Docker, WSL e deploy em [docs/local-dev.md](docs/local-dev.md).

## Como abrir a parte Unity

1. Abra o Unity Hub.
2. Adicione a pasta `unity-app/` como projeto.
3. Use Unity 2022.3 LTS ou uma LTS validada para Meta Quest/OpenXR.
4. Instale Android Build Support.
5. Aguarde os pacotes serem restaurados.
6. Se quiser gerar cenas base, use `Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes`.
7. Abra `unity-app/Assets/_Project/Scenes/AppStart.unity` quando as cenas existirem.

Se Unity CLI não estiver disponível, o projeto ainda pode ser estudado pela estrutura, scripts e documentação.

## O que precisa ser testado no Quest 3 real

- Instalação do APK.
- Abertura do app no headset.
- Legibilidade do menu.
- Tamanho dos botões para crianças.
- Interação por controle/ray.
- Orientação correta do vídeo 360.
- Áudio.
- Performance.
- Conforto.
- Uso offline, se exigido.
- Operação por professor/monitor.

## Próximos passos

1. Rodar a web demo para alinhar menu e catálogo.
2. Abrir `unity-app/` no Unity.
3. Testar um vídeo 360 real curto e autorizado.
4. Gerar build Android.
5. Instalar no Quest 3.
6. Validar o checklist.
7. Responder perguntas do cliente.
8. Decidir Unity, web ou híbrido.

## Documentos principais

- [Ambiente local de desenvolvimento](docs/local-dev.md)
- [Visão do produto](docs/product-vision.md)
- [Opções técnicas](docs/technical-options.md)
- [Perguntas para cliente](docs/client-questions.md)
- [Checklist de validação](docs/validation-checklist.md)
- [Requisitos de vídeo](docs/video-requirements.md)
- [Riscos](docs/risks.md)
- [Roadmap](docs/roadmap.md)
