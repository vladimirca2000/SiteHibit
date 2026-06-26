#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
FRONTEND="$ROOT/frontend/hibit-web"
WWWROOT="$ROOT/backend/Hibit.Api/wwwroot"
DIST="$FRONTEND/dist/hibit-web/browser"

echo "Building Angular frontend..."
cd "$FRONTEND"
npm ci
npm run build -- --configuration=production

if [ ! -d "$DIST" ]; then
  echo "Build output not found at $DIST" >&2
  exit 1
fi

echo "Copying frontend build to $WWWROOT ..."
find "$WWWROOT" -mindepth 1 ! -name '.gitkeep' -exec rm -rf {} + 2>/dev/null || true
mkdir -p "$WWWROOT"
cp -r "$DIST"/* "$WWWROOT/"

echo "Publishing .NET API..."
cd "$ROOT/backend"
dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o "$ROOT/publish"

echo "Publish complete: $ROOT/publish"
