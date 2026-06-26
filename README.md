# SiteHibit — Site institucional Hibit

Site institucional da **Hibit** com backend .NET 8, frontend Angular, MySQL, RabbitMQ e deploy na King.host.

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) e npm
- [Angular CLI](https://angular.dev/tools/cli): `npm install -g @angular/cli`
- [Docker](https://docs.docker.com/engine/install/) instalado **no WSL** (não é necessário Docker nativo no Windows)

## Desenvolvimento com Docker no WSL

Neste projeto, **MySQL e RabbitMQ rodam no Docker dentro do WSL**. Os comandos `docker` e `docker compose` devem ser executados **no terminal WSL** (Ubuntu, etc.), não no PowerShell do Windows — a menos que você use o alias `docker.exe` do Docker Desktop apontando para o WSL.

### Caminho do projeto no WSL

Se o repositório está em `C:\Users\vladi\source\repos\SiteHibit`:

```bash
cd /mnt/c/Users/vladi/source/repos/SiteHibit
```

### Subir a infra (sempre no WSL)

```bash
cp .env.example .env
docker compose -f docker/docker-compose.yml --env-file .env up -d
docker compose -f docker/docker-compose.yml ps
```

### Onde rodar backend e frontend

| Aplicação | Ferramenta | Porta |
|-----------|------------|-------|
| Frontend Angular | VS Code — `npm start` em `frontend/hibit-web` | 4200 |
| Backend .NET | Visual Studio — projeto `Hibit.Api` | 5000 |
| MySQL + RabbitMQ | Docker no WSL | 3306 / 5672 |

Se o backend no Visual Studio não conectar ao MySQL/RabbitMQ no WSL, use o IP do WSL na connection string (ver abaixo).

### Se o backend no Windows não conectar ao MySQL/RabbitMQ no WSL

1. Confirme que os containers estão rodando no WSL: `docker compose -f docker/docker-compose.yml ps`
2. No WSL, teste: `curl -s -o /dev/null -w "%{http_code}" http://localhost:15672` (RabbitMQ UI)
3. Obtenha o IP do WSL:

   ```bash
   hostname -I | awk '{print $1}'
   ```

4. No `.env` / `appsettings.Development.json` do Windows, troque `localhost` pelo IP do WSL na connection string e no RabbitMQ, por exemplo:

   ```
   Server=172.x.x.x;Port=3306;...
   RabbitMQ__Host=172.x.x.x
   ```

### Ordem de subida

```
WSL: Docker (MySQL + RabbitMQ) → Visual Studio Backend (:5000) → VS Code Frontend (:4200)
```

## Configuração inicial

1. Clone o repositório e entre na pasta do projeto.

2. Copie as variáveis de ambiente:

   ```bash
   cp .env.example .env
   ```

   Edite `.env` com valores locais. Para gerar chave AES-256 e IV:

   ```bash
   # PowerShell — chave (32 bytes) e IV (16 bytes) em Base64
   [Convert]::ToBase64String((1..32 | ForEach-Object { Get-Random -Maximum 256 }))
   [Convert]::ToBase64String((1..16 | ForEach-Object { Get-Random -Maximum 256 }))
   ```

3. Suba MySQL e RabbitMQ **no terminal WSL** (ver seção acima):

   ```bash
   docker compose -f docker/docker-compose.yml --env-file .env up -d
   ```

   - MySQL: `localhost:3306` (root / `erika03`, database `hibit_dev`)
   - RabbitMQ AMQP: `localhost:5672` (admin / `admin123`)
   - RabbitMQ Management UI: http://localhost:15672

## Backend

```bash
cd backend
dotnet restore
dotnet run --project Hibit.Api
```

- API: http://localhost:5000
- Health: http://localhost:5000/health
- Login (JWT): `POST http://localhost:5000/api/auth/login`
- Swagger (Development): http://localhost:5000/swagger

Credenciais do usuário de aplicação (local): `hibit-app` / `hibit-app-2026` (ver `.env.example`).

## Frontend

```bash
cd frontend/hibit-web
npm install
npm start
```

- App: http://localhost:4200 (rodar no VS Code com `npm start`)
- Paleta: branco, preto, cinza e verde (ver `src/styles/_variables.scss`)

## Testes

```bash
cd backend
dotnet test
```

## Estrutura

```
SiteHibit/
├── backend/          # .NET 8 Clean Architecture
├── frontend/         # Angular (hibit-web)
├── docker/           # Docker Compose (MySQL + RabbitMQ)
├── .github/workflows # CI/CD
└── .agent.md         # Especificação do projeto
```

## Deploy (King.host)

A API .NET serve o frontend Angular a partir de `wwwroot` (SPA com fallback para `index.html`).

### Script automatizado

**Windows (PowerShell):**

```powershell
./scripts/publish-web-to-api.ps1
```

**Linux/macOS:**

```bash
chmod +x scripts/publish-web-to-api.sh
./scripts/publish-web-to-api.sh
```

O script:

1. Compila o Angular em modo production
2. Copia `dist/hibit-web/browser/*` para `backend/Hibit.Api/wwwroot/`
3. Executa `dotnet publish` e gera a pasta `publish/` na raiz do repositório

### Publicação manual na King.host

1. Execute o script de publish ou siga os passos manuais abaixo.
2. Faça upload do conteúdo de `publish/` para a hospedagem Windows (IIS / ASP.NET Core).
3. Configure no painel da King.host todas as variáveis listadas em `.env.example`:
   - `ConnectionStrings__DefaultConnection`
   - `RabbitMQ__*` (host, porta, usuário, senha, fila)
   - `Encryption__Key` e `Encryption__Iv` (AES-256, Base64)
   - `CORS_ORIGINS` (domínio de produção, ex.: `https://www.hibit.com.br`)
4. Garanta HTTPS ativo e, se possível, Cloudflare na frente do domínio.

### Passos manuais (alternativa)

```bash
cd frontend/hibit-web && npm ci && npm run build -- --configuration=production
# Copie dist/hibit-web/browser/* para backend/Hibit.Api/wwwroot/
cd backend && dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o ../publish
```

Consulte `.env.example` para a lista completa de variáveis necessárias em produção.
