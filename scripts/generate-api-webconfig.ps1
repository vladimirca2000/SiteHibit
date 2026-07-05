#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory = $true)]
    [string]$OutputDir
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-RequiredEnv([string]$Name) {
    $value = [Environment]::GetEnvironmentVariable($Name)
    if ([string]::IsNullOrWhiteSpace($value)) {
        throw "Variavel de ambiente obrigatoria ausente: $Name"
    }
    return $value
}

function Escape-Xml([string]$Value) {
    return [System.Security.SecurityElement]::Escape($Value)
}

$templatePath = Join-Path $PSScriptRoot 'web.config.production.template'
$targetDir = (Resolve-Path $OutputDir).Path
$targetPath = Join-Path $targetDir 'web.config'

$mysqlPassword = Get-RequiredEnv 'MYSQL_PASSWORD'
$connectionString = "Server=mysql.hibit.com.br;Port=3306;Database=hibit;User=hibit;Password=$mysqlPassword;"

$replacements = @{
    '{{CONNECTION_STRING}}' = Escape-Xml $connectionString
    '{{RABBITMQ_PASSWORD}}' = Escape-Xml (Get-RequiredEnv 'RABBITMQ_PASSWORD')
    '{{JWT_SECRET}}'        = Escape-Xml (Get-RequiredEnv 'JWT_SECRET')
    '{{APP_USER_PASSWORD}}' = Escape-Xml (Get-RequiredEnv 'APP_USER_PASSWORD')
    '{{ENCRYPTION_KEY}}'    = Escape-Xml (Get-RequiredEnv 'ENCRYPTION_KEY')
    '{{ENCRYPTION_IV}}'     = Escape-Xml (Get-RequiredEnv 'ENCRYPTION_IV')
}

$content = Get-Content -Path $templatePath -Raw -Encoding UTF8
foreach ($entry in $replacements.GetEnumerator()) {
    $content = $content.Replace($entry.Key, $entry.Value)
}

New-Item -ItemType Directory -Force -Path (Join-Path $targetDir 'logs') | Out-Null
Set-Content -Path $targetPath -Value $content -Encoding UTF8 -NoNewline
Write-Host "web.config gerado em: $targetPath"
