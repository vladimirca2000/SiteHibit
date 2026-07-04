#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
FRONTEND="$ROOT/frontend/hibit-web"
DIST="$FRONTEND/dist/hibit-web/browser"
OUTPUT="$ROOT/publish/www"

echo "Building Angular frontend..."
cd "$FRONTEND"
npm ci
npm run build -- --configuration=production

if [ ! -d "$DIST" ]; then
  echo "Build output not found at $DIST" >&2
  exit 1
fi

echo "Copying frontend build to $OUTPUT ..."
rm -rf "$OUTPUT"
mkdir -p "$OUTPUT"
cp -r "$DIST"/* "$OUTPUT/"

echo "Frontend publish complete: $OUTPUT"
