# 🍃 Vila da Folha — Jogo Web Naruto

Jogo web educacional com tema Naruto, agora com **backend em C#/.NET 8** e banco de dados SQL Server.

## 🏗️ Arquitetura do Projeto

```
Naruto-game/
├── 📁 frontend/                    # Frontend JavaScript (HTML/CSS/JS + Tailwind)
├── 📁 backend/                    # Backend C# (.NET 8)
│   ├── 📁 src/
│   │   ├── NarutoGame.Core/       # Entidades e interfaces
│   │   ├── NarutoGame.Infrastructure/ # EF Core + Repositórios
│   │   ├── NarutoGame.Application/   # Services + DTOs
│   │   └── NarutoGame.API/        # Controllers + JWT
│   ├── docker-compose.yml         # Orquestração Docker
│   └── Dockerfile                 # Build da API
└── 📁 plans/                     # Documentação
```

## 🚀 Tecnologias

### Backend
- **.NET 8** + **ASP.NET Core Web API**
- **Entity Framework Core** + **SQL Server 2022**
- **JWT Bearer** Authentication
- **AutoMapper** + **FluentValidation**
- **Serilog** para logging
- **Swagger/OpenAPI** para documentação

### Frontend
- HTML5 + CSS3 + **Tailwind CSS**
- **JavaScript** ES6+
- **Vite** como build tool

### DevOps
- **Docker** + **Docker Compose**

## ⚡ Como Executar

### 1. Com Docker (Recomendado)

```powershell
# Navegue até a pasta backend
cd backend

# Inicie os containers (SQL Server + API)
docker-compose up -d

# A API estará disponível em: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### 2. Desenvolvimento Local

**Pré-requisitos:**
- .NET 8 SDK
- Node.js 18+
- SQL Server (ou SQL Server Express)

```powershell
# Backend
cd backend/src/NarutoGame.API
dotnet run

# Frontend (em outro terminal)
cd frontend
npm install
npm run dev
```

## 📡 Endpoints da API

### Autenticação
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Criar nova conta |
| POST | `/api/auth/login` | Autenticar e receber JWT |

### Jogador (Requer JWT)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/player/profile` | Perfil do jogador |
| PUT | `/api/player/profile` | Atualizar perfil |
| POST | `/api/player/xp` | Adicionar XP |
| PUT | `/api/player/skills` | Atualizar habilidades |

### Jogo
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/game/npcs` | Listar NPCs |
| GET | `/api/game/npcs/{key}` | Detalhes de NPC |
| GET | `/api/game/missions` | Listar missões |
| POST | `/api/game/missions/{id}/complete` | Completar missão |

### Sistema
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/health` | Health check |

## 🔐 Autenticação JWT

O token JWT deve ser enviado no header:
```
Authorization: Bearer {token}
```

Exemplo de login:
```json
POST /api/auth/login
{
  "identifier": "usuario@email.com",
  "password": "SuaSenh@123"
}
```

Resposta:
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2024-01-01T12:00:00Z",
  "user": {
    "id": "guid",
    "email": "usuario@email.com",
    "nickname": "usuario"
  }
}
```

## 🛡️ Rate Limiting

Para proteger contra ataques de força bruta, a API implementa rate limiting:

| Endpoint | Limite | Janela |
|----------|--------|--------|
| `/api/auth/*` (login, register) | 5 requisições | 15 minutos |
| Demais endpoints | 100 requisições | 1 minuto |

**Resposta quando bloqueado (HTTP 429):**
```json
{
  "error": "Muitas requisições, tente novamente mais tarde!"
}
```

## 🗄️ Estrutura do Banco de Dados

### Tabelas
- **Users** — Conta do jogador (email, senha hash)
- **Players** — Dados do personagem (nível, XP, vila, elemento)
- **PlayerStats** — Habilidades (ninjutsu, taijutsu, genjutsu)
- **Npcs** — Personagens não jogáveis
- **NpcDialogues** — Diálogos dos NPCs
- **Missions** — Missões disponíveis
- **PlayerMissions** — Missões completadas pelo jogador

## 🔧 Variáveis de Ambiente

```bash
# Backend: Copie o arquivo de exemplo
cd backend
cp .env.example .env

# Edite o .env com suas configurações:
# - SA_PASSWORD: Senha do SQL Server
# - JWT_SECRET: Chave secreta para JWT (mínimo 32 caracteres)
```

### Configurações necessárias

```env
# Connection String do SQL Server
# ⚠️ IMPORTANTE: Use senhas fortes diferentes em produção!
SA_PASSWORD=SuaSenhaForte@123

# Configurações JWT
# ⚠️ IMPORTANTE: Mínimo 32 caracteres!
JWT_SECRET=SuaChaveSecretaDeNoMinimo32Caracteres!

# Ambiente
ASPNETCORE_ENVIRONMENT=Development
```

### Desenvolvimento Local (sem Docker)

Para desenvolvimento local, as variáveis podem ser configuradas no `appsettings.Development.json` ou como variáveis de ambiente do sistema.

**⚠️ Aviso:** O `appsettings.json` agora contém apenas placeholders. Todos os segredos devem vir de variáveis de ambiente ou do `appsettings.Development.json`.

## 📁 Estrutura de Pastas (Backend)

```
backend/src/
├── NarutoGame.Core/
│   ├── Entities/       # User, Player, Npc, Mission, etc.
│   ├── Enums/          # ElementType, VillageType, SkillType
│   └── Interfaces/     # IUserRepository, IPlayerRepository, etc.
│
├── NarutoGame.Infrastructure/
│   ├── Data/           # DbContext, DbInitializer
│   └── Repositories/   # Implementações dos repositórios
│
├── NarutoGame.Application/
│   ├── DTOs/           # Data Transfer Objects
│   ├── Services/        # AuthService, PlayerService, GameService
│   └── Mappings/       # AutoMapper profiles
│
└── NarutoGame.API/
    ├── Controllers/     # AuthController, PlayerController, etc.
    ├── Configuration/   # Extensions
    └── Middleware/      # Exception handling
```

## 🧪 Testes

```powershell
# Executar testes unitários
cd backend
dotnet test
```

## 📚 Recursos

- Interface responsiva (desktop e mobile)
- Tema claro/escuro com persistência
- NPCs interativos com diálogos
- Sistema de XP, níveis e habilidades
- Missões diárias e rank-up

## ⚠️ Aviso de Segurança

Este é um projeto **educacional**. Para produção:
- Implemente HTTPS
- Use variáveis de ambiente para secrets
- Configure rate limiting
- Implemente refresh tokens
- Adicione validação de input server-side

---

🍃 *"Aquele que quebra as regras é escória, mas aquele que abandona seus amigos é pior que escória."* — Naruto Uzumaki
