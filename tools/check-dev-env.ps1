#Requires -Version 5.1
<#
  check-dev-env.ps1
  Checagem amigavel do ambiente local da PoC "Mergulho Ambiental 360".

  Regras:
    - Mostra [ ok ] / [warn] / [FAIL] / [info].
    - NAO falha so porque Docker ou WSL nao existem (sao opcionais).
    - Falha (exit 1) apenas se faltar algo ESSENCIAL do repositorio.

  Uso:
    .\tools\check-dev-env.ps1
    powershell -ExecutionPolicy Bypass -File .\tools\check-dev-env.ps1
#>

$ErrorActionPreference = 'Continue'
$script:essentialMissing = 0

# Resolve a raiz do repo a partir da pasta do script (tools/ -> raiz),
# para o script funcionar independente do diretorio atual.
$repoRoot = Split-Path -Parent $PSScriptRoot

function Write-Status {
  param(
    [ValidateSet('ok', 'warn', 'fail', 'info')] [string] $Level,
    [string] $Message
  )
  switch ($Level) {
    'ok'   { Write-Host "[ ok ] $Message" -ForegroundColor Green }
    'warn' { Write-Host "[warn] $Message" -ForegroundColor Yellow }
    'fail' { Write-Host "[FAIL] $Message" -ForegroundColor Red }
    'info' { Write-Host "[info] $Message" -ForegroundColor Cyan }
  }
}

function Test-Tool {
  param([string] $Name, [string] $Hint = '')
  $cmd = Get-Command $Name -ErrorAction SilentlyContinue
  if ($null -ne $cmd) {
    Write-Status ok "$Name -> $($cmd.Source)"
    return $true
  }
  $msg = "$Name nao encontrado"
  if ($Hint) { $msg = "$msg - $Hint" }
  Write-Status warn $msg
  return $false
}

function Test-RepoPath {
  param([string] $RelativePath, [switch] $Essential)
  $full = Join-Path $repoRoot $RelativePath
  if (Test-Path $full) {
    Write-Status ok $RelativePath
    return $true
  }
  if ($Essential) {
    Write-Status fail "$RelativePath ausente (essencial)"
    $script:essentialMissing++
  }
  else {
    Write-Status warn "$RelativePath ausente"
  }
  return $false
}

Write-Host ""
Write-Host "=== Mergulho Ambiental 360 - checagem de ambiente local ===" -ForegroundColor White
Write-Host "Repo: $repoRoot"
Write-Host ""

# --- Ferramentas (todas nao-fatais) ---
Write-Host "Ferramentas:" -ForegroundColor White
$hasGit    = Test-Tool 'git'    'necessario para versionamento'
$hasPython = Test-Tool 'python' 'necessario para rodar a web demo (python -m http.server)'
$hasDocker = Test-Tool 'docker' 'opcional - alternativa para servir a web demo'
$hasWsl    = Test-Tool 'wsl'    'opcional - ambiente Linux auxiliar'
$null      = Test-Tool 'gh'     'opcional - GitHub CLI para PRs'

if ($hasGit) {
  try { git lfs version | Out-Null; Write-Status ok 'git-lfs disponivel' }
  catch { Write-Status warn 'git-lfs nao instalado (rode: git lfs install)' }
}

if ($hasDocker) {
  $serverVersion = docker info --format '{{.ServerVersion}}' 2>$null
  if ($LASTEXITCODE -eq 0 -and $serverVersion) {
    Write-Status ok "docker daemon ativo (server $serverVersion)"
  }
  else {
    Write-Status warn 'docker instalado, mas o daemon nao respondeu (Docker Desktop pode estar parado)'
  }
}

Write-Host ""

# --- Arquivos do repositorio ---
Write-Host "Repositorio:" -ForegroundColor White
$null = Test-RepoPath 'web-demo'                       -Essential
$null = Test-RepoPath 'web-demo/index.html'            -Essential
$null = Test-RepoPath 'web-demo/src/main.js'           -Essential
$null = Test-RepoPath 'web-demo/src/video-catalog.json' -Essential
$null = Test-RepoPath 'unity-app/Packages/manifest.json' -Essential

$versionFile = Join-Path $repoRoot 'unity-app/ProjectSettings/ProjectVersion.txt'
if (Test-Path $versionFile) {
  Write-Status ok 'unity-app/ProjectSettings/ProjectVersion.txt'
  Get-Content $versionFile | ForEach-Object { Write-Status info $_ }
}
else {
  Write-Status warn 'unity-app/ProjectSettings/ProjectVersion.txt ausente'
}

Write-Host ""

# --- Proximos passos ---
Write-Host "Proximos passos:" -ForegroundColor White
Write-Host "  Web demo : cd web-demo; python -m http.server 8080  ->  http://localhost:8080"
Write-Host "  Docker   : docker compose up --build web-demo        ->  http://localhost:8080"
Write-Host "  Unity    : abra unity-app/ no Unity Hub e rode"
Write-Host "             Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes"
Write-Host ""

# --- Resumo ---
if ($script:essentialMissing -gt 0) {
  Write-Status fail "Faltam $($script:essentialMissing) item(ns) essencial(is) do repositorio."
  exit 1
}

Write-Status ok 'Ambiente essencial do repositorio OK.'
if (-not $hasPython -and -not $hasDocker) {
  Write-Status warn 'Sem Python e sem Docker: instale um dos dois para servir a web demo.'
}
exit 0
