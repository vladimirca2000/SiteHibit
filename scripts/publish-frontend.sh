#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
FRONTEND="$ROOT/frontend/hibit-blazor"
PROJECT="$FRONTEND/Hibit.Web.csproj"
TEMP="$ROOT/publish/www-build"
OUTPUT="$ROOT/publish/www"

echo "Building Blazor WebAssembly frontend..."
rm -rf "$TEMP"
dotnet publish "$PROJECT" -c Release -o "$TEMP"

STATIC="$TEMP/wwwroot"
if [ ! -f "$STATIC/index.html" ]; then
  echo "Build output not found at $STATIC (missing index.html)" >&2
  exit 1
fi

echo "Copying static site to $OUTPUT ..."
rm -rf "$OUTPUT"
mkdir -p "$OUTPUT"
cp -r "$STATIC"/* "$OUTPUT/"

if [ ! -f "$OUTPUT/web.config" ]; then
  echo "Build output missing web.config at $OUTPUT" >&2
  exit 1
fi

if [ ! -d "$OUTPUT/_framework" ]; then
  echo "Build output missing _framework at $OUTPUT" >&2
  exit 1
fi

echo "Frontend publish complete: $OUTPUT"
