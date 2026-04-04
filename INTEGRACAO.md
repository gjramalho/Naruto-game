# Integração Frontend + Backend Docker

Este documento descreve como rodar o projeto Naruto Game com integração completa entre frontend e backend usando Docker.

## Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                    NARUTO GAME                              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐  │
│  │   Frontend   │    │   Backend    │    │   Database   │  │
│  │   (Vite)     │◄──►│   (.NET API) │◄──►│ (SQL Server) │  │
│  │   Port: 5173 │    │   Port: 5000 │    │   Port: 1433 │  │
│  └──────────────┘    └──────────────┘    └──────────────┘  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Pré-requisitos

- Docker Desktop instalado
- Docker Compose instalado
- Git (opcional)

## Configuração

### 1. Criar arquivo .env

```bash
# Copiar o exemplo
cp .env.example .env

# Editar o arquivo .env com suas configurações
```

Variáveis obrigatórias:
- `SA_PASSWORD`: Senha do SQL Server (mínimo 8 caracteres, com maiúscula, minúscula e número)
- `JWT_SECRET`: Chave secreta para JWT (mínimo 32 caracteres)

### 2. Iniciar os serviços

```bash
# Iniciar todos os serviços (frontend + backend + database)
docker-compose up -d

# Verificar status dos serviços
docker-compose ps

# Ver logs
docker-compose logs -f
```

### 3. Acessar a aplicação

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger (apenas em Development)

## Funcionamento

### Frontend (Vite)
- Roda em http://localhost:5173
- Usa `api-config.js` para configurar URL da API
- Usa `api-service.js` para comunicação com backend
- Usa `auth.js` modificado para autenticação via API

### Backend (.NET API)
- Roda em http://localhost:5000
- CORS configurado para aceitar requisições do frontend
- JWT para autenticação
- Rate limiting configurado

### Database (SQL Server)
- Roda em localhost:1433
- Dados persistidos em volume Docker

## Endpoints da API

### Autenticação
- `POST /api/auth/register` - Registra novo usuário
- `POST /api/auth/login` - Autentica usuário e retorna JWT

### Jogador (requer autenticação)
- `GET /api/player/profile` - Obtém perfil do jogador
- `PUT /api/player/profile` - Atualiza perfil do jogador
- `POST /api/player/xp` - Adiciona XP ao jogador
- `PUT /api/player/skills` - Atualiza habilidades do jogador

### Jogo
- `GET /api/game/npcs` - Lista todos os NPCs
- `GET /api/game/npcs/{key}` - Obtém NPC específico
- `GET /api/game/missions` - Lista todas as missões
- `GET /api/game/missions/daily` - Lista missões diárias
- `POST /api/game/missions/{missionId}/complete` - Completa uma missão

## Comandos Úteis

```bash
# Parar todos os serviços
docker-compose down

# Parar e remover volumes (cuidado: dados serão perdidos)
docker-compose down -v

# Reconstruir containers
docker-compose up -d --build

# Ver logs de um serviço específico
docker-compose logs -f frontend
docker-compose logs -f api
docker-compose logs -f sqlserver

# Acessar shell do container frontend
docker-compose exec frontend sh

# Acessar shell do container API
docker-compose exec api bash
```

## Troubleshooting

### Frontend não conecta com backend
1. Verifique se o backend está rodando: `docker-compose ps`
2. Verifique os logs do backend: `docker-compose logs api`
3. Verifique se o CORS está configurado corretamente no backend

### Erro de autenticação
1. Verifique se o JWT_SECRET está configurado no .env
2. Verifique se o token está sendo enviado no header Authorization
3. Verifique os logs do backend para erros de validação

### SQL Server não inicia
1. Verifique se a senha atende aos requisitos (mínimo 8 caracteres, maiúscula, minúscula, número)
2. Verifique se a porta 1433 não está em uso
3. Verifique os logs: `docker-compose logs sqlserver`

### Portas em uso
Se as portas 5173, 5000 ou 1433 estiverem em uso, modifique o `docker-compose.yml`:
```yaml
ports:
  - "5174:5173"  # Frontend
  - "5001:5000"  # Backend
  - "1434:1433"  # Database
```

## Desenvolvimento sem Docker

Se preferir rodar sem Docker:

### Frontend
```bash
npm install
npm run dev
```

### Backend
```bash
cd backend
dotnet restore
dotnet run --project src/NarutoGame.API
```

### Database
Instale SQL Server localmente ou use Docker apenas para o banco:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

## Arquivos Criados para Integração

- `api-config.js` - Configuração da URL da API
- `api-service.js` - Serviço para comunicação com backend
- `auth.js` - Modificado para usar API backend
- `script.js` - Modificado para salvar/carregar dados via API
- `docker-compose.yml` - Configuração Docker para frontend + backend
- `.env.example` - Exemplo de variáveis de ambiente
- `INTEGRACAO.md` - Este documento
