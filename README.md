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
# 🍃 Vila da Folha — Naruto-game

Protótipo cliente-only: jogo web em HTML/CSS/JS usando Vite + Tailwind. Feito para aprendizado e demonstração. Não use este código como sistema de autenticação/segurança em produção.

Resumo (rápido)
- SPA estática servida pelo Vite
- Tema persistente (localStorage) — `naruto-theme` / `akatsuki-theme`
- Autenticação local (LocalStorage) com hash de senha (WebCrypto) — apenas protótipo
- Sidebar: fixa em desktop; off-canvas (mobile) com botão hambúrguer e overlay

Funcionalidades principais
- Navegação: Vila (NPCs), Perfil, Créditos
- NPCs interativos com diálogos, XP e progressão
- Perfil personalizável (nickname, vila, elemento)
- Sistema de progresso (XP / níveis / skills)
- Notificações visuais e efeitos (glass, transições)

Pré-requisitos
- Node.js 18+ (ou 20+ recomendado)
- npm (ou pnpm/yarn)

Instalação e execução
1) Instale dependências

```powershell
npm install
```

2) Desenvolvimento (dev server)

```powershell
npm run dev
```

3) Build e preview (produção)

```powershell
npm run build
npm run preview
```

Scripts principais (package.json)
- `dev`: inicia Vite para desenvolvimento
- `build`: cria `dist/` otimizado
- `preview`: serve `dist/` para testes
- `clean`: remove `dist/` (Windows/Unix compatível via rimraf)

Arquivos-chave
- `index.html` — aplicação principal (carrega `script.js`)
- `login.html`, `register.html` — autenticação e seleção de personagem
- `script.js` — lógica principal (navegação, HUD, NPCs, notificações)
- `auth.js` — cadastro/login local (hash client-side)
- `login.js`, `register.js` — validações e fluxo de autenticação
- `src/styles/input.css`, `src/styles/theme.css` — Tailwind entry + temas

Autenticação (importante)
- Usuários são salvos em `localStorage.narutoUsers` como array de objetos: `{ email, nickname, passwordHash }`.
- Login aceita `email` ou `nickname` + `password`.
- Chaves usadas pelo app:
  - `localStorage.narutoGameLogged = 'true'` (quando "Lembrar de mim" ativo)
  - `sessionStorage.narutoSession = 'true'` (login temporário por aba)
  - `localStorage.narutoLoggedNickname` (nickname do usuário logado)
  - `localStorage.narutoGameTheme` (`light` | `dark`)
- Rate-limit: 3 tentativas falhas → bloqueio por 30s (controle via localStorage). 
- Aviso: hashing é feito no cliente (WebCrypto SHA-256). Isso é suficiente para protótipo, mas não substitui autenticação segura com backend.

Status do Ninja (localStorage)
- Dados principais do jogador são salvos em `localStorage.narutoGameData` com a estrutura:

```json
{
  "player": {
    "name": "nick",
    "level": 1,
    "xp": 0,
    "maxXp": 100,
    "village": "Indefinido",
    "element": "Indefinido",
    "chakra": 100,
    "skills": { "ninjutsu": 1, "taijutsu": 1, "genjutsu": 1 }
  },
  "isDarkMode": false,
  "currentSection": "vila"
}
```

-- O painel "Status do Ninja" está em `index.html` → seção Vila. Ele exibe: nickname (do login), vila, elemento, nível, XP e chakra. A vila e o elemento são definidos no cadastro (register) — atualmente não editáveis inline.

Theming e UI
- Tema armazenado em `localStorage.narutoGameTheme` e aplicado ao `<body>` como `naruto-theme` ou `akatsuki-theme`.
- Sidebar e elementos UI são estilizados por `src/styles/theme.css` e utilitários Tailwind.
- Mobile: existe botão hambúrguer (`#mobileMenuBtn`) que abre o menu off-canvas e um overlay (`#sidebarOverlay`).

Comandos úteis (console do navegador)
```js
// Adicionar XP
gameCommands.addXP(100)

// Subir de nível
gameCommands.levelUp()

// Resetar jogo (limpa progresso salvo)
gameCommands.resetGame()

// Alternar tema
gameCommands.toggleTheme()

// Navegar para seção
gameCommands.showSection('perfil')
```

Problemas comuns & soluções rápidas
- "CSS quebrado": rode o dev server (`npm run dev`) — Tailwind é processado pelo Vite.
- Porta ocupada: `npm run dev -- --port 5175`.
- Tema não persiste: verifique permissões de LocalStorage e se `id="body"` existe.
- Reset manual (console):
```js
localStorage.removeItem('narutoGameData')
localStorage.removeItem('narutoGameTheme')
localStorage.removeItem('narutoGameLogged')
localStorage.removeItem('narutoLoggedNickname')
```

Estrutura do projeto (resumida)
```
Naruto-game/
├─ index.html
├─ login.html
├─ register.html
├─ script.js
├─ auth.js
├─ login.js
├─ register.js
├─ src/styles/
│  ├─ input.css
│  └─ theme.css
└─ README.md
```

Observações finais
- Projeto focado em prototipagem visual e UX; código e autenticação são client-side.
- Remova dados de LocalStorage quando compartilhar ou publicar exemplos públicos.

Licença & créditos
- Projeto fan-made; conteúdo do anime pertence aos detentores originais.

