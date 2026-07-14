#!/usr/bin/env bash
# Deprecated: use publish-frontend.sh and publish-api.sh for split deployment (/www/ + /API/).
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
WWWROOT="$ROOT/backend/Hibit.Api/wwwroot"
PUBLISH_WWW="$ROOT/publish/www"

bash "$ROOT/scripts/publish-frontend.sh"

echo "Copying frontend build to $WWWROOT ..."
find "$WWWROOT" -mindepth 1 ! -name '.gitkeep' -exec rm -rf {} + 2>/dev/null || true
mkdir -p "$WWWROOT"
cp -r "$PUBLISH_WWW"/* "$WWWROOT/"

echo "Publishing .NET API..."
cd "$ROOT/backend"
dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o "$ROOT/publish"

echo "Publish complete: $ROOT/publish"
