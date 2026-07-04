#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUTPUT="$ROOT/publish/api"

echo "Publishing .NET API..."
cd "$ROOT/backend"
dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o "$OUTPUT"

echo "API publish complete: $OUTPUT"
