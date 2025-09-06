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
- **Sidebar Lateral Fixa e Tematizada**: Navegação pela esquerda, sempre visível e com cores que acompanham o tema

### 🎨 Design e Experiência
- **Glassmorphism**: Efeitos modernos de vidro e blur
- **Animações Suaves**: Transições e efeitos CSS avançados
- **Efeitos de Partículas**: Elementos visuais imersivos
- **Navegação Intuitiva**: Menu fixo e atalhos de teclado
- **Notificações**: Sistema de feedback visual

## 🚀 Como Executar (detalhado)

### Pré-requisitos
- Node.js 18+ (ou 20+ recomendado)
  - Verifique: `node -v`
- npm (vem com o Node). Alternativas: `pnpm` ou `yarn` (opcional)

Notas do ambiente:
- Projeto em ESM (package.json com `"type": "module"`). Arquivos de config (`postcss.config.js`, `tailwind.config.js`, `vite.config.js`) usam `export default`.
- Tailwind roda via PostCSS/Vite. O processamento dos `@tailwind` acontece somente quando você usa `npm run dev` ou `npm run build`.
- Globs do Tailwind foram otimizados para não varrer `node_modules/` e `dist/`.

### Passo a passo (dev)
1) Instalar dependências

```powershell
# Windows PowerShell
npm install
```

2) Iniciar o servidor de desenvolvimento (Vite)

```powershell
npm run dev
```

3) Acessar no navegador a URL exibida (ex.: http://localhost:5173)

Rotas principais (abra direto no navegador):
- Login (início): `/login.html`
- Cadastro: `/register.html`
- Index (redireciona para login se não estiver logado): `/` ou `/index.html`

Observação sobre o fluxo: o site inicia no Login. O Index redireciona para `login.html` quando não há sessão ativa ("lembrar de mim" via localStorage ou sessão via sessionStorage).

4) Alterar a porta (opcional)

```powershell
npm run dev -- --port 5175
```

Scripts disponíveis

```json
{
  "dev": "vite --port 5173",
  "build": "vite build",
  "preview": "vite preview --port 5174 --open",
  "clean": "rimraf dist || rmdir /s /q dist"
}
```

### Build de produção

```powershell
npm run build        # gera dist/
npm run preview      # serve dist/ e abre o navegador (http://localhost:5174)
```

Não abra os arquivos HTML direto do sistema de arquivos quando usar Tailwind: o CSS com `@tailwind` é processado pelo Vite. Use `npm run dev` (durante o desenvolvimento) ou `npm run preview` (após o build) para ver os estilos corretamente.

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
- **Vite 5**: Dev server e build
- **TailwindCSS (via PostCSS + Autoprefixer)**: utilitários processados no build/dev
<!-- Ícones agora são emojis/SVG locais; Font Awesome removido -->
- **LocalStorage API**: Persistência de dados

## 🔐 Autenticação (client-side)

- Contas são salvas localmente em `localStorage.narutoUsers` com `email`, `nickname` e `passwordHash` (SHA-256 via WebCrypto; fallback simples se indisponível).
- Login aceita email OU nome ninja como identificador + senha.
- "Lembrar de mim":
  - Marcado: define `localStorage.narutoGameLogged = true` (permanece logado entre sessões).
  - Desmarcado: usa `sessionStorage.narutoSession = true` (apenas a aba/sessão atual).
- Rate-limit: após 3 tentativas de login falhas, bloqueia por 30s com mensagem de contagem regressiva.
- Realce de erros: campos de login com erro são destacados.
- Pós-cadastro: redireciona para `login.html?registered=1` e exibe aviso “Conta criada! Faça login para continuar.”
- Sidebar exibe `@nickname` do usuário logado e traz ações: "Trocar Personagem" e "Sair".

Aviso: este modelo é adequado para protótipos e uso local. Para produção, utilize um backend/autenticação segura.

### Dica para VS Code
- Recomenda-se instalar a extensão: Tailwind CSS IntelliSense (`bradlc.vscode-tailwindcss`).
- O workspace já contém `.vscode/settings.json` para o editor não marcar `@tailwind` como erro.

## 📱 Responsividade

O jogo é totalmente responsivo e funciona em:
- **Desktop** (1920x1080+)
- **Tablet** (768px+)
- **Mobile** (320px+)

## 🎨 Temas

O tema é alternado entre:

- Modo Claro (naruto-theme): gradientes vibrantes inspirados na Vila da Folha
- Modo Escuro (akatsuki-theme): gradientes escuros com vermelho (Akatsuki)

Persistência: a escolha é salva em `localStorage.narutoGameTheme` como `light` ou `dark`.

Páginas com toggle integrado: `index.html`, `login.html` e `register.html` (botão flutuante no canto superior direito). O ícone alterna entre `🌙` (modo Naruto/claro) e `☀️` (modo Akatsuki/escuro).

Observação: o tema agora controla o fundo de toda a página (não só o menu). Removemos gradientes fixos das páginas para que `naruto-theme`/`akatsuki-theme` façam efeito global, inclusive na sidebar.

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
├── login.html          # Login + seleção de personagem
├── register.html       # Cadastro
├── src/
│   └── styles/
│       ├── input.css   # Entrada do Tailwind
│       └── theme.css   # Estilos customizados
├── script.js           # Lógica do jogo
├── login.js            # Lógica do login e seleção
├── login.css           # Estilos do login
├── register.js         # Validação do cadastro e tema
├── register.css        # Estilos do cadastro
├── auth.js             # Utilitários de autenticação (CRUD local + hash de senha)
└── README.md           # Documentação

Arquivos de configuração (ESM): `vite.config.js`, `tailwind.config.js`, `postcss.config.js`.

## 🧪 Dicas & Troubleshooting

- CSS não carrega / página “quebrada” ao abrir arquivo .html diretamente:
  - Use `npm run dev` (desenvolvimento) ou `npm run preview` após `npm run build`.
  - Motivo: Tailwind `@tailwind` é processado pelo Vite/PostCSS.

- Porta em uso (EADDRINUSE):
  - Rode com outra porta: `npm run dev -- --port 5175`.

- Build antiga aparecendo no navegador:
  - Faça um hard-reload (Ctrl+F5) ou limpe o cache.
  - Opcional: `npm run clean` para remover `dist/` antes de um novo build.

- Tema não persiste entre páginas:
  - Verifique se o navegador permite LocalStorage.
  - Chave usada: `narutoGameTheme` (valores `light` | `dark`).
  - Confirme que o `<body>` possui o `id="body"` nas páginas e que as classes `naruto-theme`/`akatsuki-theme` não estão sendo sobrescritas por estilos antigos.

- Bloqueio de login (rate-limit):
  - Após 3 falhas, o login é bloqueado por 30s. Aguarde a contagem (mensagem na tela) ou limpe as chaves:
    - `localStorage.removeItem('narutoLoginAttempts')`
    - `localStorage.removeItem('narutoLoginBlockedUntil')`

- Sessão vs Lembrar de mim:
  - "Lembrar de mim": `localStorage.narutoGameLogged = true`
  - Sessão atual: `sessionStorage.narutoSession = true`
  - Para sair: use o botão "Sair" (limpa sessão e login), ou limpe manualmente:
    - `localStorage.removeItem('narutoGameLogged')`
    - `sessionStorage.removeItem('narutoSession')`
    - `localStorage.removeItem('narutoLoggedNickname')`

- Fluxo não avança do index:
  - O index verifica `narutoGameRegistered` e `narutoGameLogged`. Se estiverem ausentes, redireciona para cadastro/login.
  - Acesse diretamente `/register.html` para iniciar do começo.

## 🧹 Resetar estado (dev)

Você pode limpar o progresso/estado durante o desenvolvimento:

No console do navegador (F12) na tela principal:
```js
gameCommands.resetGame();
```

Ou removendo chaves específicas no console (qualquer página):
```js
localStorage.removeItem('narutoGameData');
localStorage.removeItem('narutoGameTheme');
localStorage.removeItem('narutoGameRegistered');
localStorage.removeItem('narutoGameLogged');
localStorage.removeItem('narutoGameCharacter');
```
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
1. Temas/cores: `src/styles/theme.css`
2. Tailwind (purge/scan): `tailwind.config.js`
3. Toggle e persistência: `script.js`, `login.js`, `register.js`

### Adicionar Habilidades
1. Expanda o objeto `skills` no player
2. Adicione elementos HTML para exibição
3. Implemente lógica de progressão

## 📄 Licença

Este é um projeto fan-made sem fins lucrativos, criado para fins educacionais e de entretenimento.

**Naruto** é uma criação de **Masashi Kishimoto**, publicado pela Shueisha e animado pelo Studio Pierrot.

## 👨‍💻 Desenvolvimento

Desenvolvido por **Gjramalho** com foco em:
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

## 📝 Changelog (2025-09-06)

- Tema passou a controlar o fundo de todas as páginas; removidos gradientes fixos do Login/Register.
- Sidebar esquerda fixa, sem menu superior, com estilos que mudam conforme o tema.
- Toggle de tema unificado nas três páginas, com ícone sincronizado (🌙/☀️) e persistência em `localStorage`.
- Build com Vite/Tailwind estável; configs em ESM e ordem correta de imports (`input.css` importa `theme.css` antes dos `@tailwind`).
- Autenticação local: `auth.js` com hash de senha e cadastro/login persistentes.
- Login inicia o fluxo; Index redireciona ao Login quando não logado.
- "Lembrar de mim" (localStorage) ou sessão da aba (sessionStorage).
- Rate-limit no login (3 falhas -> 30s de bloqueio) e destaque de campos com erro.
- Sidebar mostra `@nickname` e botões "Trocar Personagem" e "Sair".
