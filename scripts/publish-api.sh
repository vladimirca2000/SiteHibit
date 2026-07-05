#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUTPUT="$ROOT/publish/api"

echo "Publishing .NET API..."
cd "$ROOT/backend"
dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o "$OUTPUT"

if [[ -n "${MYSQL_PASSWORD:-}" && -n "${RABBITMQ_PASSWORD:-}" && -n "${JWT_SECRET:-}" && -n "${APP_USER_PASSWORD:-}" && -n "${ENCRYPTION_KEY:-}" && -n "${ENCRYPTION_IV:-}" ]]; then
  bash "$(dirname "$0")/generate-api-webconfig.sh" "$OUTPUT"
else
  echo "Aviso: variaveis de producao ausentes; web.config nao gerado." >&2
fi

echo "API publish complete: $OUTPUT"
