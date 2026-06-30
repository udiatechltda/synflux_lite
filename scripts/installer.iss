#define AppName "Sistema PDV"
#define AppVersion "1.0.0"
#define AppPublisher "TechOne IT"
#define AppExeName "PDV.exe"
#define AppIcon "..\Assets\Images\Png\techone-pdv.ico"
#define SourceDir "..\artifacts\installer\publish"
#define OutputDir "..\artifacts\installer"

[Setup]
AppId={{B4F2A1C3-7E8D-4F9A-B3C2-1A2B3C4D5E6F}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\TechOne\PDV
DefaultGroupName=TechOne\PDV
AllowNoIcons=yes
OutputDir={#OutputDir}
OutputBaseFilename=PDV-Setup-{#AppVersion}
SetupIconFile={#AppIcon}
UninstallDisplayIcon={app}\{#AppExeName}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64os
ArchitecturesInstallIn64BitMode=x64os
PrivilegesRequired=admin
MinVersion=10.0
DisableWelcomePage=no
DisableDirPage=no
DisableProgramGroupPage=yes
CloseApplications=yes
CloseApplicationsFilter=PDV.exe
RestartApplications=no

[Languages]
Name: "pt"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Atalhos adicionais:"; Flags: checkedonce

[Dirs]
; Concede permissao de escrita para o grupo Users — necessario para o auto-updater (roda sem elevacao)
Name: "{app}"; Permissions: users-modify

[Files]
; Todos os arquivos da publicacao
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\{#AppExeName}"
Name: "{group}\Desinstalar {#AppName}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "Iniciar {#AppName} agora"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Remove apenas os arquivos do app — preserva dados do usuario em %ProgramData%\TechOne\PDV
Type: filesandordirs; Name: "{app}"
