# Naruto-game — Jogo Web (Vila da Folha)

Projeto educacional com backend em C# (.NET 8) e frontend em JavaScript (Vite + Tailwind). Este repositório contém a API, o cliente web e recursos para executar o jogo localmente ou em containers.

## Estrutura do repositório

```
Naruto-game/
├── frontend/                # Cliente web (Vite, Tailwind)
├── backend/                 # API C# (.NET 8) + EF Core
└── assets/                  # Fontes, imagens e recursos estáticos
```

## Tecnologias

- Backend: .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server, Serilog
- Autenticação: JWT Bearer
- Frontend: HTML, CSS (Tailwind), JavaScript (ES6), Vite
- DevOps: Docker, Docker Compose

## Quickstart

Recomendo usar Docker para uma experiência reprodutível.

1) Usando Docker Compose

```powershell
cd backend
docker-compose up -d
```

A API ficará disponível em http://localhost:5000 (Swagger em /swagger).

2) Executando localmente (desenvolvimento)

Pré-requisitos: .NET 8 SDK, Node.js 18+, SQL Server ou SQL Server Express.

```powershell
# Backend
cd backend/src/NarutoGame.API
dotnet run

# Frontend (novo terminal)
cd frontend
npm install
npm run dev
```

## Endpoints essenciais (resumo)

- POST `/api/auth/register` — registrar conta
- POST `/api/auth/login` — autenticar e obter JWT
- GET `/api/player/profile` — perfil do jogador (requere JWT)
- PUT `/api/player/profile` — atualizar perfil (requere JWT)
- POST `/api/player/xp` — adicionar XP (requere JWT)
- GET `/api/game/missions` — listar missões
- POST `/api/game/missions/{id}/complete` — completar missão
- GET `/health` — health check

Envie o token no header `Authorization: Bearer {token}`.

## Variáveis de ambiente

Use um arquivo `.env` local (não comite segredos). Exemplos:

```
SA_PASSWORD=SuaSenhaForte@123
JWT_SECRET=SuaChaveSecretaDeNoMinimo32Caracteres!
ASPNETCORE_ENVIRONMENT=Development
```

Configure a connection string do SQL Server conforme necessário.

## Testes

Executar testes unitários do backend:

```powershell
cd backend
dotnet test
```

## Contribuição

Contribuições são bem-vindas. Abra um issue para discutir mudanças grandes e envie PRs com descrições claras. Não inclua segredos em commits.

## Avisos de segurança

Este é um projeto para aprendizado. Antes de qualquer implantação em produção:

- habilite HTTPS e revisão de configuração do servidor
- armazene segredos em variáveis de ambiente ou gerenciadores de segredos
- implemente refresh tokens e políticas de segurança adicionais

---

Se preferir, posso criar READMEs separados para `backend/` e `frontend/` com instruções mais detalhadas.



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
