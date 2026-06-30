"""
deploy-pdv-update.py — Publica nova versao do PDV e ativa auto-update no servidor.

Uso:
    python scripts/deploy-pdv-update.py --version 1.1.0

Opcional:
    --password <senha>   Senha SSH (ou variavel de ambiente PDV_DEPLOY_PASSWORD)
    --notes "Texto"      Release notes (padrao: "Versao X.Y.Z")
    --no-build           Pula o dotnet publish (usa artifacts/releases/pdv-<version>/ existente)

O script:
  1. Executa dotnet publish (Release, win-x64, framework-dependent)
  2. Cria ZIP excluindo arquivos protegidos (pdv.sqlite, pdv.config.json, TechOne.PDV.Updater.exe, .pdb)
  3. Calcula SHA256 do ZIP
  4. Faz upload do ZIP para /var/www/downloads/ no servidor
  5. Atualiza appsettings.json no servidor (Enabled=true, Version, PackageUrl, Sha256)
  6. Reinicia container app_dotnet
  7. Verifica endpoint /updates/pdv/latest

Dependencias: pip install paramiko
"""

import argparse, hashlib, json, os, subprocess, sys, time, zipfile
from pathlib import Path

try:
    import paramiko
except ImportError:
    sys.exit("Instale paramiko: python -m pip install paramiko")

# ── Configuracao do servidor ───────────────────────────────────────────────
SERVER_HOST      = '85.31.61.191'
SERVER_USER      = 'root'
REMOTE_APP_DIR   = '/opt/T2TiRetaguardaSH'
REMOTE_DOWNLOADS = '/var/www/downloads'
BASE_URL         = 'https://retaguardash.techone-it.com.br'

# ── Arquivos excluidos do ZIP de update ────────────────────────────────────
PROTECTED_NAMES = {'pdv.sqlite', 'pdv.config.json', 'technone.pdv.updater.exe'}
SKIP_EXTENSIONS = {'.pdb'}

REPO_ROOT = Path(__file__).parent.parent


def main():
    parser = argparse.ArgumentParser(description='Deploy PDV update to production')
    parser.add_argument('--version', required=True, help='Versao a publicar, ex: 1.1.0')
    parser.add_argument('--password', help='Senha SSH (ou env PDV_DEPLOY_PASSWORD)')
    parser.add_argument('--notes', help='Release notes')
    parser.add_argument('--no-build', action='store_true', help='Pula dotnet publish')
    args = parser.parse_args()

    version  = args.version
    password = args.password or os.environ.get('PDV_DEPLOY_PASSWORD') or input('Senha SSH do servidor: ')
    notes    = args.notes or f'Versao {version}'

    releases_dir = REPO_ROOT / 'artifacts' / 'releases'
    releases_dir.mkdir(parents=True, exist_ok=True)
    publish_dir = releases_dir / f'pdv-{version}'
    zip_name    = f'pdv-{version}.zip'
    zip_path    = releases_dir / zip_name

    # ── 1. Build ───────────────────────────────────────────────────────────
    if args.no_build:
        print(f'[1/6] Build ignorado. Usando: {publish_dir}')
        if not publish_dir.exists():
            sys.exit(f'Diretorio nao encontrado: {publish_dir}')
    else:
        print(f'\n[1/6] Publicando PDV {version}...')
        result = subprocess.run([
            'dotnet', 'publish', str(REPO_ROOT / 'PDV.csproj'),
            '-c', 'Release',
            '-r', 'win-x64',
            '--self-contained', 'true',
            '-o', str(publish_dir),
        ], cwd=str(REPO_ROOT))
        if result.returncode != 0:
            sys.exit('Build falhou.')
        print('  Build concluido.')

    # ── 2. ZIP ─────────────────────────────────────────────────────────────
    print(f'\n[2/6] Criando ZIP: {zip_path.name}')
    included = 0
    with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as zf:
        for f in sorted(publish_dir.rglob('*')):
            if not f.is_file():
                continue
            if f.name.lower() in PROTECTED_NAMES:
                print(f'  [protegido, ignorado] {f.name}')
                continue
            if f.suffix.lower() in SKIP_EXTENSIONS:
                continue
            arc = f.relative_to(publish_dir)
            zf.write(f, arc)
            included += 1
    size_mb = zip_path.stat().st_size / 1024 / 1024
    print(f'  {included} arquivo(s), {size_mb:.1f} MB')

    # ── 3. SHA256 ──────────────────────────────────────────────────────────
    sha256 = hashlib.sha256(zip_path.read_bytes()).hexdigest().upper()
    print(f'\n[3/6] SHA256: {sha256}')

    # ── 4. Upload ──────────────────────────────────────────────────────────
    print(f'\n[4/6] Conectando ao servidor {SERVER_HOST}...')
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    client.connect(SERVER_HOST, username=SERVER_USER, password=password, timeout=15)

    sftp = client.open_sftp()
    remote_zip = f'{REMOTE_DOWNLOADS}/{zip_name}'
    print(f'  Enviando {size_mb:.1f} MB -> {remote_zip}')
    sftp.put(str(zip_path), remote_zip)
    sftp.close()
    print('  Upload concluido.')

    def run(cmd, timeout=30):
        _, out, err = client.exec_command(cmd, timeout=timeout)
        code = out.channel.recv_exit_status()
        return code, out.read().decode().strip(), err.read().decode().strip()

    # ── 5. Atualiza appsettings.json ───────────────────────────────────────
    pkg_url = f'{BASE_URL}/downloads/{zip_name}'
    print(f'\n[5/6] Atualizando appsettings.json no servidor...')

    # Baixa, faz merge localmente e reenvia via SFTP (evita problemas de escaping no heredoc)
    import tempfile, os
    tmp = tempfile.mktemp(suffix='.json')
    sftp2 = client.open_sftp()
    sftp2.get(f'{REMOTE_APP_DIR}/appsettings.json', tmp)
    with open(tmp, encoding='utf-8') as f:
        cfg = json.load(f)
    cfg['Updates'] = {
        'Pdv': {
            'Enabled':      True,
            'Version':      version,
            'PackageUrl':   pkg_url,
            'Sha256':       sha256,
            'Required':     False,
            'ReleaseNotes': notes,
        }
    }
    with open(tmp, 'w', encoding='utf-8') as f:
        json.dump(cfg, f, indent=2, ensure_ascii=False)
    sftp2.put(tmp, f'{REMOTE_APP_DIR}/appsettings.json')
    sftp2.close()
    os.unlink(tmp)
    print('  appsettings.json atualizado.')

    # ── 6. Restart app_dotnet ──────────────────────────────────────────────
    print(f'\n[6/6] Reiniciando app_dotnet...')
    run('docker restart t2ti-app-dotnet')
    time.sleep(5)

    # ── 7. Verificacao ─────────────────────────────────────────────────────
    print('\nVerificando endpoint...')
    _, o, _ = run(f'curl -s "{BASE_URL}/updates/pdv/latest?currentVersion=0.0.0"')
    try:
        resp = json.loads(o)
        ok = resp.get('enabled') and resp.get('version') == version
        status = 'OK' if ok else 'AVISO: resposta inesperada'
        print(f'  enabled={resp.get("enabled")} version={resp.get("version")} [{status}]')
    except Exception:
        print(f'  Resposta raw: {o}')

    client.close()

    print(f"""
Deploy concluido!
  Versao  : {version}
  ZIP     : {zip_path}
  URL     : {pkg_url}
  SHA256  : {sha256}

Os usuarios recebem a atualizacao automaticamente ao fechar o PDV.
Para desativar: edite /opt/T2TiRetaguardaSH/appsettings.json (Enabled: false) e reinicie app_dotnet.
""")


if __name__ == '__main__':
    main()
