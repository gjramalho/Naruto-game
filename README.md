# 🍃 Vila da Folha — Jogo Web Naruto

Protótipo cliente (front-end) desenvolvido com HTML5, CSS3, JavaScript e Tailwind CSS. O projeto é educacional e serve como demonstração de interfaces e mecânicas simples de jogo.

## Aviso de segurança

Este projeto armazena usuários e dados no navegador (localStorage) e realiza hashing do lado do cliente apenas para fins de demonstração. Não utilize este código para autenticação, armazenamento de credenciais ou qualquer solução em produção.

Antes de publicar ou demonstrar, recomenda-se limpar o localStorage (ver seção "Reset manual").

## Recursos principais

- Interface responsiva (desktop e mobile)
- Tema claro/escuro com persistência
- NPCs interativos com diálogos e sistema de progressão
- Perfil personalizável (nickname, vila, elemento)
- Sistema de XP, níveis e habilidades

## Requisitos

- Node.js 18+ (20+ recomendado)
- npm (ou pnpm / yarn)

## Instalação

```powershell
npm install
```

## Desenvolvimento

```powershell
npm run dev
```

## Build para produção

```powershell
npm run build
npm run preview
```

## Arquivos principais

- `index.html` — entrada da aplicação
- `login.html`, `register.html` — fluxo de autenticação local
- `script.js` — lógica principal da aplicação (navegação, UI, NPCs)
- `auth.js` — funções de cadastro/login (hash local)
- `src/styles/input.css`, `src/styles/theme.css` — configurações e tema do Tailwind

## Autenticação e armazenamento (importante)

- Usuários são armazenados em `localStorage.narutoUsers` como um array de objetos: `{ email, nickname, passwordHash }`.
- Flags e chaves usadas pela aplicação:
  - `localStorage.narutoGameLogged` = 'true'
  - `sessionStorage.narutoSession` = 'true'
  - `localStorage.narutoLoggedNickname`
  - `localStorage.narutoGameTheme` (`light` | `dark`)

### Exemplo do estado salvo

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

## Comandos úteis (console do navegador)

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

## Problemas comuns e soluções rápidas

- CSS sem aplicar: rode o servidor de desenvolvimento (`npm run dev`) — Tailwind é processado via Vite.
- Porta ocupada: `npm run dev -- --port 5175`.
- Tema não persiste: verifique permissões de localStorage e se o elemento `id="body"` está presente.

## Reset manual (console)

Use estes comandos no console do navegador para limpar dados locais de demonstração:

```js
localStorage.removeItem('narutoGameData')
localStorage.removeItem('narutoGameTheme')
localStorage.removeItem('narutoGameLogged')
localStorage.removeItem('narutoLoggedNickname')
localStorage.removeItem('narutoUsers')
```

## Estrutura do projeto (resumida)

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

## Licença e créditos

- Projeto fan-made; conteúdo do anime pertence aos respectivos detentores dos direitos autorais.

Ao publicar o repositório, recomenda-se adicionar um arquivo `LICENSE` (por exemplo, MIT) e certificar-se de que nenhuma credencial foi comitada.

