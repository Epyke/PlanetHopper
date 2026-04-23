# Planet Runner

Um endless runner onde encarnas o primeiro astronauta a pisar a lua e tens de fugir o mais longe possível antes de seres apanhado pelo OVNI (não há OVNI).

---

## 👥 Elementos do Grupo

| Nome | Número |
|---|---|
| Henrique Fernandes | 33393 |
| Diogo Gomes | - |

---

## Versão do Unity

**Unity 6000.3.9f1**

---

## Descrição do Jogo

**Planet Runner** é um endless runner 3D ambientado na Lua. O jogador controla o primeiro astronauta a pisar o solo lunar, que após a sua chegada é perseguido por um OVNI (elemento narrativo, o OVNI não foi implementado).

### Funcionalidades Implementadas

- **Geração procedural do mapa** com 3 vetores e curvas aleatórias, garantindo percursos únicos em cada sessão.
- **5 tipos de obstáculos** distintos que surgem aleatoriamente ao longo do percurso.
- **Dificuldade incremental** a velocidade do jogo aumenta progressivamente com o tempo jogado.
- **Sistema de pontuação** baseado na distância percorrida.
- **Leaderboard online** integrado via serviço **LootLocker**, que sincroniza as pontuações num servidor remoto.
- **Loja de personagens** com sistema de moedas e equipamento de skins.
- **Música de fundo** distinta para o menu principal, durante o jogo e no ecrã de Game Over.
- **Resolução adaptada para mobile** (rácio 16:9).

---

## Jogabilidade

### Objetivo
Percorre a maior distância possível sem colidir com obstáculos. Quanto mais longe chegares, maior será a tua pontuação no leaderboard.

### Controlos

| Ação | Teclado | Teclas Alternativas |
|---|---|---|
| Mover para a esquerda | `A` | `←` (seta esquerda) |
| Mover para a direita | `D` | `→` (seta direita) |
| Saltar | `W` | `↑` (seta cima) |
| Deslizar (*slide*) | `S` | `↓` (seta baixo) |

### Regras
- O jogo termina quando o astronauta colide com um obstáculo.
- A velocidade aumenta ao longo do tempo, tornando o jogo progressivamente mais difícil.
- A pontuação é calculada com base na distância percorrida.

---

## Abertura do Projeto

**Atenção - Janela do LootLocker:** Ao abrir o projeto, pode aparecer uma janela de configuração do serviço LootLocker (leaderboard). **As credenciais não são obrigatórias para jogar e testar o jogo.** Pode fechar ou ignorar essa janela - apenas o leaderboard online não funcionará sem elas.

---

## Assets Multimédia

### Animações

As animações do astronauta foram criadas manualmente, o que envolveu a construção de uma armature no Blender e a animação do modelo 3D fornecido.

### Modelos 3D

| Asset | Fonte | Formato | Justificação |
|---|---|---|---|
| Muros / Rochas (obstáculos) | [Procedural Rocks Low Poly – Sketchfab](https://sketchfab.com/3d-models/procedural-rocks-low-poly-378ccb37411449a6a13033da40414b0c) | `.fbx` / `.glb` | Low poly adequado para um runner com geração procedural; baixo custo computacional. |
| Pedras (obstáculos) | [Low Poly Rocks – Sketchfab](https://sketchfab.com/3d-models/low-poly-rocks-9823ec262054408dbe26f6ddb9c0406e) | `.fbx` / `.glb` | Estilo consistente com os muros; variedade visual nos obstáculos. |
| Personagem (Astronauta) | [Astronaut – Sketchfab](https://sketchfab.com/3d-models/astronaut-23b856b4b6324d179bb340ee182a5e18) | `.fbx` / `.glb` | Modelo sem rig |

### Audio

| Asset | Fonte | Utilização | Justificação |
|---|---|---|---|
| Música do Menu | [YouTube](https://www.youtube.com/watch?v=QmEpVIjWTw8) | Menu principal | Ambiente calmo e espacial para o ecrã inicial. |
| Música em Jogo | [YouTube](https://www.youtube.com/watch?v=BS712Udp0YU) | Sessão de jogo | Ritmo acelerado que acompanha e intensifica a jogabilidade. |
| Som de Game Over | [YouTube](https://www.youtube.com/watch?v=_asNhzXq72w) | Ecrã de Game Over | Feedback sonoro imediato e reconhecível para o fim da sessão. |

Os ficheiros de audio foram descarregados em **.mp3** e importados como `AudioClip` no Unity.

### Resolução / Aspect Ratio
O jogo foi desenvolvido com resolução **16:9**, orientada para dispositivos móveis.

---

## Observações e Lacunas

- **OVNI não implementado:** A narrativa do jogo inclui um OVNI perseguidor, mas este inimigo não foi criado.