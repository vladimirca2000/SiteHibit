#!/usr/bin/env pwsh
# Deprecated: use publish-frontend.ps1 and publish-api.ps1 for split deployment (/www/ + /API/).
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$frontendDir = Join-Path $repoRoot 'frontend/hibit-web'
$wwwrootDir = Join-Path $repoRoot 'backend/Hibit.Api/wwwroot'
$distDir = Join-Path $frontendDir 'dist/hibit-web/browser'

Write-Host 'Building Angular frontend...'
Push-Location $frontendDir
try {
    npm ci
    npm run build -- --configuration=production
} finally {
    Pop-Location
}

if (-not (Test-Path $distDir)) {
    throw "Build output not found at $distDir"
}

Write-Host "Copying frontend build to $wwwrootDir ..."
if (Test-Path $wwwrootDir) {
    Get-ChildItem $wwwrootDir -Force | Where-Object { $_.Name -ne '.gitkeep' } | Remove-Item -Recurse -Force
} else {
    New-Item -ItemType Directory -Path $wwwrootDir | Out-Null
}

Copy-Item -Path (Join-Path $distDir '*') -Destination $wwwrootDir -Recurse -Force

Write-Host 'Publishing .NET API...'
Push-Location (Join-Path $repoRoot 'backend')
try {
    dotnet publish Hibit.Api/Hibit.Api.csproj -c Release -o (Join-Path $repoRoot 'publish')
} finally {
    Pop-Location
}

Write-Host "Publish complete: $(Join-Path $repoRoot 'publish')"
