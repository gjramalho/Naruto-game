# 🍃 Vila da Folha - Jogo Web Naruto

Um jogo web interativo inspirado no universo de Naruto, desenvolvido com HTML5, CSS3, JavaScript e TailwindCSS.

## 🎮 Características

### ✨ Funcionalidades Principais
- **Interface Responsiva**: Compatível com desktop e mobile
- **Dark/Light Mode**: Alternância de tema com persistência
- **NPCs Interativos**: Converse com Naruto, Sasuke, Sakura e Kakashi
- **Sistema de Progressão**: Ganhe XP, suba de nível e desenvolva habilidades
- **Perfil Personalizável**: Crie seu ninja com nome, vila e elemento
- **Armazenamento Local**: Progresso salvo automaticamente

### 🎨 Design e Experiência
- **Glassmorphism**: Efeitos modernos de vidro e blur
- **Animações Suaves**: Transições e efeitos CSS avançados
- **Efeitos de Partículas**: Elementos visuais imersivos
- **Navegação Intuitiva**: Menu fixo e atalhos de teclado
- **Notificações**: Sistema de feedback visual

## 🚀 Como Executar

1. **Clone ou baixe o projeto**
2. **Abra o arquivo `index.html` em qualquer navegador moderno**
3. **Explore a Vila da Folha e interaja com os personagens!**

Não é necessário servidor web - o jogo roda diretamente no navegador.

## 🎯 Como Jogar

### Navegação
- **Vila**: Página principal com NPCs interativos
- **Perfil**: Customize seu ninja e veja suas habilidades
- **Créditos**: Informações sobre o desenvolvimento

### Interações
- **Clique nos NPCs** para conversar e ganhar XP
- **Configure seu perfil** na seção Perfil
- **Use atalhos de teclado**:
  - `1` - Vila
  - `2` - Perfil  
  - `3` - Créditos
  - `Ctrl+T` - Alternar tema
  - `ESC` - Fechar diálogos

### Sistema de Progressão
- **Ganhe XP** interagindo com personagens
- **Suba de nível** automaticamente
- **Desenvolva habilidades**: Ninjutsu, Taijutsu, Genjutsu
- **Personalize** seu ninja com diferentes vilas e elementos

## 🛠️ Tecnologias Utilizadas

- **HTML5**: Estrutura semântica
- **CSS3**: Animações e efeitos visuais
- **JavaScript ES6+**: Lógica do jogo e interatividade
- **TailwindCSS**: Framework de estilo utilitário
- **Font Awesome**: Ícones
- **LocalStorage API**: Persistência de dados

## 📱 Responsividade

O jogo é totalmente responsivo e funciona em:
- **Desktop** (1920x1080+)
- **Tablet** (768px+)
- **Mobile** (320px+)

## 🎨 Temas

### Modo Claro (Padrão)
- Gradiente azul/verde representando a Vila da Folha
- Cores vibrantes inspiradas no anime

### Modo Escuro
- Gradiente escuro com tons de cinza
- Melhor para uso noturno

## 🎮 Comandos de Console (Desenvolvimento)

Abra o console do navegador (F12) e use:

```javascript
// Adicionar XP
gameCommands.addXP(100);

// Subir de nível
gameCommands.levelUp();

// Resetar jogo
gameCommands.resetGame();

// Alternar tema
gameCommands.toggleTheme();

// Navegar para seção
gameCommands.showSection('perfil');
```

## 🔧 Estrutura do Projeto

```
Naruto-game/
├── index.html          # Página principal
├── styles.css          # Estilos customizados
├── script.js           # Lógica do jogo
└── README.md           # Documentação
```

## 🎯 Funcionalidades Futuras (Expansões Possíveis)

- **Sistema de Missões**: Tarefas para completar
- **Batalhas**: Sistema de combate por turnos
- **Inventário**: Itens e equipamentos ninja
- **Múltiplas Vilas**: Explorar outras vilas ninja
- **Multiplayer**: Interação entre jogadores
- **Sons e Música**: Trilha sonora do anime
- **Mais NPCs**: Personagens adicionais
- **Sistema de Clãs**: Uchiha, Hyuga, etc.

## 🏆 Conquistas Implementáveis

- **Primeiro Diálogo**: Converse com um NPC
- **Nível 10**: Alcance o nível 10
- **Mestre das Habilidades**: Maximize uma habilidade
- **Explorador**: Visite todas as seções
- **Ninja Dedicado**: Jogue por 30 minutos

## 🎨 Personalização

### Adicionar Novos NPCs
1. Edite o objeto `npcs` em `script.js`
2. Adicione o card HTML correspondente
3. Configure diálogos e características

### Modificar Temas
1. Edite as classes CSS em `styles.css`
2. Ajuste o método `toggleTheme()` em `script.js`

### Adicionar Habilidades
1. Expanda o objeto `skills` no player
2. Adicione elementos HTML para exibição
3. Implemente lógica de progressão

## 📄 Licença

Este é um projeto fan-made sem fins lucrativos, criado para fins educacionais e de entretenimento.

**Naruto** é uma criação de **Masashi Kishimoto**, publicado pela Shueisha e animado pelo Studio Pierrot.

## 👨‍💻 Desenvolvimento

Desenvolvido por **Cline AI Assistant** como demonstração de desenvolvimento web moderno com foco em:
- **UX/UI Design**
- **Programação Orientada a Objetos**
- **Responsividade**
- **Performance**
- **Acessibilidade**

## 🐛 Relatório de Bugs

Se encontrar algum problema:
1. Verifique o console do navegador (F12)
2. Teste em modo incógnito
3. Limpe o localStorage: `gameCommands.resetGame()`

## 🌟 Contribuições

Este projeto serve como base para aprendizado e pode ser expandido com:
- Novas funcionalidades
- Melhorias visuais
- Otimizações de performance
- Correções de bugs

---

**Que a Vontade do Fogo esteja com você! 🔥**
