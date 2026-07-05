#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$outputDir = Join-Path $repoRoot 'publish/api'

Write-Host 'Publishing .NET API...'
Push-Location (Join-Path $repoRoot 'backend')
try {
    dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o $outputDir
} finally {
    Pop-Location
}

$required = @('MYSQL_PASSWORD', 'RABBITMQ_PASSWORD', 'JWT_SECRET', 'APP_USER_PASSWORD', 'ENCRYPTION_KEY', 'ENCRYPTION_IV')
$hasAll = ($required | Where-Object { -not [string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($_)) }).Count -eq $required.Count

if ($hasAll) {
    & (Join-Path $PSScriptRoot 'generate-api-webconfig.ps1') -OutputDir $outputDir
} else {
    Write-Warning 'Variaveis de producao ausentes; web.config nao gerado. Defina MYSQL_PASSWORD, RABBITMQ_PASSWORD, JWT_SECRET, APP_USER_PASSWORD, ENCRYPTION_KEY, ENCRYPTION_IV.'
}

Write-Host "API publish complete: $outputDir"
