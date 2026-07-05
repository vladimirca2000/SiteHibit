#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Uso: $0 <diretorio-publish-api>" >&2
  exit 1
fi

OUTPUT_DIR="$1"
TEMPLATE="$(cd "$(dirname "$0")" && pwd)/web.config.production.template"
TARGET="$OUTPUT_DIR/web.config"

require_env() {
  local name="$1"
  if [[ -z "${!name:-}" ]]; then
    echo "Variavel de ambiente obrigatoria ausente: $name" >&2
    exit 1
  fi
}

xml_escape() {
  python3 -c 'import html,sys; print(html.escape(sys.argv[1], quote=False))' "$1"
}

require_env MYSQL_PASSWORD
require_env RABBITMQ_PASSWORD
require_env JWT_SECRET
require_env APP_USER_PASSWORD
require_env ENCRYPTION_KEY
require_env ENCRYPTION_IV

CONNECTION_STRING="Server=mysql.hibit.com.br;Port=3306;Database=hibit;User=hibit;Password=${MYSQL_PASSWORD};"

mkdir -p "$OUTPUT_DIR/logs"
content="$(cat "$TEMPLATE")"
content="${content//\{\{CONNECTION_STRING\}\}/$(xml_escape "$CONNECTION_STRING")}"
content="${content//\{\{RABBITMQ_PASSWORD\}\}/$(xml_escape "$RABBITMQ_PASSWORD")}"
content="${content//\{\{JWT_SECRET\}\}/$(xml_escape "$JWT_SECRET")}"
content="${content//\{\{APP_USER_PASSWORD\}\}/$(xml_escape "$APP_USER_PASSWORD")}"
content="${content//\{\{ENCRYPTION_KEY\}\}/$(xml_escape "$ENCRYPTION_KEY")}"
content="${content//\{\{ENCRYPTION_IV\}\}/$(xml_escape "$ENCRYPTION_IV")}"

printf '%s' "$content" > "$TARGET"
echo "web.config gerado em: $TARGET"
