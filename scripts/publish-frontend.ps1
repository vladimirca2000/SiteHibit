#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$frontendDir = Join-Path $repoRoot 'frontend/hibit-blazor'
$project = Join-Path $frontendDir 'Hibit.Web.csproj'
$tempDir = Join-Path $repoRoot 'publish/www-build'
$outputDir = Join-Path $repoRoot 'publish/www'

Write-Host 'Building Blazor WebAssembly frontend...'
if (Test-Path $tempDir) {
    Remove-Item -Path $tempDir -Recurse -Force
}
dotnet publish $project -c Release -o $tempDir

$staticRoot = Join-Path $tempDir 'wwwroot'
if (-not (Test-Path (Join-Path $staticRoot 'index.html'))) {
    throw "Build output not found at $staticRoot (missing index.html)"
}

Write-Host "Copying static site to $outputDir ..."
if (Test-Path $outputDir) {
    Remove-Item -Path $outputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outputDir | Out-Null
Copy-Item -Path (Join-Path $staticRoot '*') -Destination $outputDir -Recurse -Force

if (-not (Test-Path (Join-Path $outputDir 'web.config'))) {
    throw "Build output missing web.config at $outputDir"
}
if (-not (Test-Path (Join-Path $outputDir '_framework'))) {
    throw "Build output missing _framework at $outputDir"
}

Write-Host "Frontend publish complete: $outputDir"
