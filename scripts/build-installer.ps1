# build-installer.ps1 — Gera o instalador PDV-Setup-X.Y.Z.exe
#
# Uso:
#   .\scripts\build-installer.ps1
#   .\scripts\build-installer.ps1 -Version 1.1.0
#
# Saida: artifacts\installer\PDV-Setup-X.Y.Z.exe

param(
    [string]$Version = ""
)

$ErrorActionPreference = "Stop"
$Root = Split-Path $PSScriptRoot -Parent
$ISCC = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"

# ── Valida dependencias ────────────────────────────────────────────────────
if (-not (Test-Path $ISCC)) {
    Write-Error "Inno Setup nao encontrado em '$ISCC'. Instale em: https://jrsoftware.org/isinfo.php"
}

# ── Versao ─────────────────────────────────────────────────────────────────
if (-not $Version) {
    # Le versao do PDV.csproj
    $csproj = [xml](Get-Content "$Root\PDV.csproj")
    $Version = $csproj.Project.PropertyGroup.Version | Select-Object -First 1
    if (-not $Version) { $Version = "1.0.0" }
}
Write-Host "Versao: $Version"

# ── 1. Publish (self-contained, win-x64) ──────────────────────────────────
$PublishDir = "$Root\artifacts\installer\publish"
Write-Host "`n[1/3] Publicando PDV (self-contained)..."
& dotnet publish "$Root\PDV.csproj" `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -p:Version=$Version `
    -o $PublishDir
if ($LASTEXITCODE -ne 0) { Write-Error "dotnet publish falhou." }

# ── 2. Compila instalador ──────────────────────────────────────────────────
Write-Host "`n[2/3] Compilando instalador..."
$IssFile = "$Root\scripts\installer.iss"

# Substitui versao no .iss em tempo de compilacao
& $ISCC $IssFile `
    "/DAppVersion=$Version" `
    "/DSourceDir=$PublishDir" `
    "/DOutputDir=$Root\artifacts\installer"
if ($LASTEXITCODE -ne 0) { Write-Error "ISCC falhou." }

# ── 3. Resultado ───────────────────────────────────────────────────────────
$installer = Get-Item "$Root\artifacts\installer\PDV-Setup-$Version.exe" -ErrorAction SilentlyContinue
if ($installer) {
    $mb = [math]::Round($installer.Length / 1MB, 1)
    Write-Host "`n[3/3] Instalador gerado com sucesso!"
    Write-Host "  Arquivo : $($installer.FullName)"
    Write-Host "  Tamanho : $mb MB"
} else {
    Write-Error "Instalador nao encontrado apos compilacao."
}
