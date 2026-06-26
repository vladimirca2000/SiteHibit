# Docker — SiteHibit (WSL)

MySQL e RabbitMQ para desenvolvimento local. **Execute todos os comandos abaixo no terminal WSL.**

Frontend e backend **não** rodam no Docker — use VS Code (`npm start`, porta 4200) e Visual Studio (`dotnet run`, porta 5000).

## Pré-requisito

Docker Engine instalado e rodando no WSL (`docker info` deve responder sem erro).

## Subir

Na raiz do repositório:

```bash
cp .env.example .env
docker compose -f docker/docker-compose.yml --env-file .env up -d
```

## Verificar

```bash
docker compose -f docker/docker-compose.yml ps
docker compose -f docker/docker-compose.yml logs -f mysql rabbitmq
```

| Serviço | Porta | Acesso |
|---------|-------|--------|
| MySQL | 3306 | `root` / `erika03`, DB `hibit_dev` |
| RabbitMQ AMQP | 5672 | `admin` / `admin123` |
| RabbitMQ UI | 15672 | http://localhost:15672 |

## Desenvolvimento local (fora do Docker)

| Aplicação | Ferramenta | Porta |
|-----------|------------|-------|
| Frontend Angular | VS Code — `npm start` em `frontend/hibit-web` | 4200 |
| Backend .NET | Visual Studio — projeto `Hibit.Api` | 5000 |

## Parar

```bash
docker compose -f docker/docker-compose.yml down
```

Com remoção de volumes (apaga dados):

```bash
docker compose -f docker/docker-compose.yml down -v
```

## Backend no Windows + Docker no WSL

Se o Visual Studio não conectar em `localhost:3306`:

```bash
# No WSL — obter IP
hostname -I | awk '{print $1}'
```

Use esse IP em `ConnectionStrings__DefaultConnection` e `RabbitMQ__Host` no `.env` ou `appsettings.Development.json` do Windows.

Alternativa: rodar `dotnet run` **também dentro do WSL**.
