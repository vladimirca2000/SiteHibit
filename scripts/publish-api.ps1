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

Write-Host "API publish complete: $outputDir"
