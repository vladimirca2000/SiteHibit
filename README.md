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
| MySQL | Docker no WSL | 3306 |
| RabbitMQ | Docker no WSL | 5672 / 15672 (UI) |

Se o backend no Visual Studio não conectar ao MySQL no WSL, use o IP do WSL na connection string (ver `docker/README.md`).

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

3. Suba MySQL **no terminal WSL**:

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
├── scripts/          # Publish local
├── .github/workflows # CI/CD
└── .agent.md         # Especificação do projeto
```

## Branches e fluxo de deploy

| Branch | Uso |
|--------|-----|
| `Development` | Desenvolvimento e PRs |
| `master` | Produção — merge dispara deploy automático |

Fluxo: PR **`Development` → `master`** (aprovado e mergeado) → GitHub Actions publica na King.host.

Repositório: `git@github.com:vladimirca2000/SiteHibit.git`

## Deploy (King.host)

Em produção na King.host (Windows), frontend e API ficam em **`/www/`** (Angular) e **`/www/API/`** (ASP.NET Core 8). A URL pública da API continua `https://hibit.com.br/API/` via `ASPNETCORE_PATHBASE=/API`.

| Caminho FTP | Conteúdo | URL pública |
|-------------|----------|-------------|
| `/www/` | Angular (site estático) | `https://hibit.com.br/` |
| `/www/API/` | ASP.NET Core 8 (API REST) | `https://hibit.com.br/API/` |

A API **não** fica na raiz FTP `/API/`. Variáveis sensíveis vão no **`web.config`** de `/www/API/` (gerado no deploy).

O frontend chama a API em `/API` (ex.: `/API/api/auth/login`, `/API/health`).

### Scripts de publish local

**Frontend:**

```powershell
./scripts/publish-frontend.ps1
```

```bash
chmod +x scripts/publish-frontend.sh
./scripts/publish-frontend.sh
```

Gera `publish/www/`.

**API:**

```powershell
./scripts/publish-api.ps1
```

```bash
chmod +x scripts/publish-api.sh
./scripts/publish-api.sh
```

Gera `publish/api/`.

### GitHub Actions + King.host Git Webhook

Workflow: [`.github/workflows/deploy-production.yml`](.github/workflows/deploy-production.yml)

Dispara em **push na branch `master`** (após merge do PR de `Development`).

O pipeline **compila** Angular e API e publica branches de release no GitHub:

| Branch | Conteúdo | Destino King.host |
|--------|----------|-------------------|
| `release-www` | Angular build (estático) | `/www/` |
| `release-api` | `dotnet publish` + `web.config` IIS | **`/www/API/`** (dentro de www) |

A King.host sincroniza cada branch com o FTP via **Git Webhook** no painel ([documentação](https://king.host/wiki/artigo/como-integrar-github-ao-painel-kinghost/)).

**Secrets** (Settings → Environments → **production** → Environment secrets):

| Secret | Descrição |
|--------|-----------|
| `APP_USER_PASSWORD` | Senha do usuário `hibit-app` (login no site). Mesmo valor usado no seed do banco e no build Angular. Padrão: `hibit-app-2026` |
| `MYSQL_PASSWORD` | Senha MySQL (`hibit` em `mysql.hibit.com.br`) |
| `RABBITMQ_PASSWORD` | Senha RabbitMQ (`admin` em `rabbit.hibit.com.br`) |
| `JWT_SECRET` | Chave JWT (mín. 32 caracteres) |
| `ENCRYPTION_KEY` | AES-256 key (Base64, 32 bytes) |
| `ENCRYPTION_IV` | AES IV (Base64, 16 bytes) |
| `FTP_SERVER` | Host FTP (`ftp.hibit.com.br`) |
| `FTP_USERNAME` | Usuário FTP |
| `FTP_PASSWORD` | Senha FTP |

**Senha da aplicação (`APP_USER_PASSWORD`):** a migration só cria a tabela `usuarios`. Na **primeira subida** da API em produção, o `DatabaseInitializer` grava o usuário `hibit-app` com hash da senha configurada em `AppUser__Password` (via `web.config`). O frontend usa o **mesmo valor** (injetado no build). Não é senha da King.host.

### Configurar Git Webhook na King.host (2 integrações)

1. Painel → **Git Webhook** → **Habilitar** → **Conectar ao GitHub**
2. Autorize o repositório `vladimirca2000/SiteHibit`
3. **Integração 1 (frontend):** branch `release-www`, diretório **`/www/`**
4. **Integração 2 (API):** branch `release-api`, diretório **`/www/API/`** (dentro de www, não `/API/` na raiz FTP)

A branch `master` continua com o código-fonte; o deploy compilado vai para `release-www` e `release-api`.

### Variáveis no painel King.host (API em `/API/`)

Configure no painel da hospedagem — **senhas apenas no painel, nunca no Git**.

Hosts e estrutura estão em `backend/Hibit.Api/appsettings.Production.json`. Sobrescreva os valores sensíveis com variáveis de ambiente:

```
ConnectionStrings__DefaultConnection=Server=mysql.hibit.com.br;Port=3306;Database=hibit;User=hibit;Password=SUA_SENHA_MYSQL
RabbitMQ__Host=rabbit.hibit.com.br
RabbitMQ__Port=5672
RabbitMQ__User=admin
RabbitMQ__Password=SUA_SENHA_RABBIT
RabbitMQ__Queue=hibit.contact
RabbitMQ__VirtualHost=/
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_PATHBASE=/API
CORS_ORIGINS=https://hibit.com.br,https://www.hibit.com.br
Jwt__Secret=***
Jwt__Issuer=Hibit.Api
Jwt__Audience=Hibit.Web
AppUser__Username=hibit-app
AppUser__Password=***
Encryption__Key=***
Encryption__Iv=***
```

### Checklist King.host

1. Configurar **Git Webhook** (GitHub) — ver [wiki King.host](https://king.host/wiki/artigo/como-integrar-github-ao-painel-kinghost/)
2. Webhook frontend: `release-www` → `/www/`
3. Webhook API: `release-api` → **`/www/API/`**
4. Configurar ASP.NET Core 8 em **hibit.com.br** (caminho **`\API`**, não `\www\API`)
5. MySQL: `mysql.hibit.com.br` / database `hibit` / user `hibit`
6. RabbitMQ: `rabbit.hibit.com.br` / user `admin` / fila `hibit.contact`
7. HTTPS ativo no domínio

Consulte `.env.example` para a lista completa de variáveis.
