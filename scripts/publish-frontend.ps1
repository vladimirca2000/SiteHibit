#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$frontendDir = Join-Path $repoRoot 'frontend/hibit-web'
$distDir = Join-Path $frontendDir 'dist/hibit-web/browser'
$outputDir = Join-Path $repoRoot 'publish/www'

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

Write-Host "Copying frontend build to $outputDir ..."
if (Test-Path $outputDir) {
    Remove-Item -Path $outputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outputDir | Out-Null
Copy-Item -Path (Join-Path $distDir '*') -Destination $outputDir -Recurse -Force

Write-Host "Frontend publish complete: $outputDir"
