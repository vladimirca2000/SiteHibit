#!/usr/bin/env pwsh
# Deprecated: use publish-frontend.ps1 and publish-api.ps1 for split deployment (/www/ + /API/).
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$wwwrootDir = Join-Path $repoRoot 'backend/Hibit.Api/wwwroot'
$publishWww = Join-Path $repoRoot 'publish/www'

& (Join-Path $PSScriptRoot 'publish-frontend.ps1')

Write-Host "Copying frontend build to $wwwrootDir ..."
if (Test-Path $wwwrootDir) {
    Get-ChildItem $wwwrootDir -Force | Where-Object { $_.Name -ne '.gitkeep' } | Remove-Item -Recurse -Force
} else {
    New-Item -ItemType Directory -Path $wwwrootDir | Out-Null
}

Copy-Item -Path (Join-Path $publishWww '*') -Destination $wwwrootDir -Recurse -Force

Write-Host 'Publishing .NET API...'
Push-Location (Join-Path $repoRoot 'backend')
try {
    dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o (Join-Path $repoRoot 'publish')
} finally {
    Pop-Location
}

Write-Host "Publish complete: $(Join-Path $repoRoot 'publish')"
