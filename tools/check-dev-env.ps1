Write-Host "Mergulho Ambiental 360 - local development check"
Write-Host ""

function Test-Command($Name) {
  $command = Get-Command $Name -ErrorAction SilentlyContinue
  if ($null -eq $command) {
    Write-Host "[missing] $Name"
    return $false
  }

  Write-Host "[ok] $Name -> $($command.Source)"
  return $true
}

$hasGit = Test-Command "git"
$hasGh = Test-Command "gh"

if ($hasGit) {
  Write-Host ""
  git --version
  git lfs version
  git status -sb
}

if ($hasGh) {
  Write-Host ""
  gh auth status
}

Write-Host ""
Write-Host "Unity checks:"
if (Test-Path "Packages/manifest.json") {
  Write-Host "[ok] Packages/manifest.json"
} else {
  Write-Host "[missing] Packages/manifest.json"
}

if (Test-Path "ProjectSettings/ProjectVersion.txt") {
  Write-Host "[ok] ProjectSettings/ProjectVersion.txt"
  Get-Content "ProjectSettings/ProjectVersion.txt"
} else {
  Write-Host "[missing] ProjectSettings/ProjectVersion.txt"
}

Write-Host ""
Write-Host "Next Unity step: open this folder in Unity Hub, then run Tools > Mergulho Ambiental 360 > Create or Refresh Base Scenes."
