# Guia Para Subir Local - PDV + Retaguarda

Este guia e para rodar a retaguarda local junto com o PDV WPF.

Fluxo recomendado para desenvolvimento:

- MySQL da retaguarda via Docker;
- API da retaguarda via `dotnet run`;
- PDV apontando para `http://localhost:5010` pelo arquivo `pdv.config.json`.

## 1. Subir A Retaguarda Local

Abra um PowerShell no repositorio da retaguarda:

```powershell
cd "D:\DEVELOPER\REPOS TECH ONE\retaguardash"
```

Suba somente o MySQL:

```powershell
docker compose -f T2TiRetaguardaSH\docker-compose.yml up -d db_mysql
```

Confira se o banco ficou healthy:

```powershell
docker ps --filter "name=t2ti-db-mysql"
```

Resultado esperado:

```text
t2ti-db-mysql ... healthy ...
```

Depois rode a API:

```powershell
$env:ASPNETCORE_URLS="http://localhost:5010"
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:RETORNA_SENHA_TEMPORARIA="true"
$env:ConnectionStrings__DefaultConnection="Server=localhost;Port=3306;Database=retaguarda_sh;Uid=t2ti_user;Pwd=123456;"
$env:ConnectionStrings__AdminConnection="Server=localhost;Port=3306;Uid=root;Pwd=MySql@2025;"

dotnet run --project T2TiRetaguardaSH\T2TiRetaguardaSH.csproj --urls "http://localhost:5010"
```

Resultado esperado:

```text
Now listening on: http://localhost:5010
Application started
```

Teste rapido da API:

```powershell
Invoke-WebRequest http://localhost:5010/auth/me
```

Resultado esperado:

- pode retornar `401 Unauthorized`;
- isso significa que a API respondeu e esta viva;
- `401` nesse endpoint e normal sem token.

## 2. Configurar O PDV Para Usar A API Local

Abra outro PowerShell no repositorio do PDV:

```powershell
cd "D:\DEVELOPER\REPOS TECH ONE\pdv"
```

Antes de rodar local, confira o arquivo:

```text
pdv.config.json
```

Para teste local, ele deve ficar assim:

```json
{
  "retaguardaUrl": "http://localhost:5010"
}
```

Se estiver apontando para producao:

```json
{
  "retaguardaUrl": "https://retaguardash.techone-it.com.br"
}
```

troque temporariamente para `http://localhost:5010`.

## 3. Rodar O PDV Local

No PowerShell do PDV:

```powershell
dotnet run --project PDV.csproj
```

Resultado esperado:

- abre a tela de login;
- cadastro inicial chama a retaguarda local;
- login chama a retaguarda local;
- os dados locais do PDV continuam no SQLite;
- sincronizacao envia snapshots para a retaguarda local quando houver conexao.

## 4. Banco Local Da Retaguarda

Para acessar o MySQL local no DBeaver:

```text
Host: localhost
Porta: 3306
Database: retaguarda_sh
Usuario: root
Senha: MySql@2025
```

Ou com usuario da aplicacao:

```text
Host: localhost
Porta: 3306
Database: retaguarda_sh
Usuario: t2ti_user
Senha: 123456
```

Se o DBeaver mostrar `Public Key Retrieval is not allowed`, adicione nas propriedades do driver:

```text
allowPublicKeyRetrieval=true
useSSL=false
```

## 5. Banco Local Do PDV

O SQLite local do projeto fica em:

```text
D:\DEVELOPER\REPOS TECH ONE\pdv\pdv.sqlite
```

No DBeaver:

1. Nova conexao.
2. Escolher `SQLite`.
3. Selecionar o arquivo `pdv.sqlite`.
4. Testar conexao.

## 6. Limpar Ambiente Local

Use isso apenas quando precisar testar cadastro do zero.

Pare a API se ela estiver rodando.

Depois, no PowerShell da retaguarda:

```powershell
cd "D:\DEVELOPER\REPOS TECH ONE\retaguardash"
docker compose -f T2TiRetaguardaSH\docker-compose.yml down -v
docker compose -f T2TiRetaguardaSH\docker-compose.yml up -d db_mysql
```

Isso recria o banco MySQL local da retaguarda.

Para limpar o SQLite do PDV, apague o arquivo local somente se tiver certeza de que pode perder os dados de teste:

```powershell
Remove-Item "D:\DEVELOPER\REPOS TECH ONE\pdv\pdv.sqlite"
```

Depois rode o PDV novamente; as migracoes recriam o banco local.

## 7. Checklist Rapido De Teste

1. API local responde em `http://localhost:5010`.
2. `pdv.config.json` aponta para `http://localhost:5010`.
3. PDV abre.
4. Cadastro inicial salva na retaguarda local.
5. E-mail/codigo de confirmacao funciona no fluxo local configurado.
6. Login funciona depois da confirmacao.
7. Abrir caixa redireciona para Caixa/Venda.
8. Venda conclui na tela de pagamento.
9. Venda aparece em Movimento.
10. Sangria e suprimento aparecem em Movimento.
11. Fechamento mostra totais por forma de pagamento.

## 8. Voltar Para Producao

Antes de gerar executavel ou testar contra o servidor, volte o `pdv.config.json` para:

```json
{
  "retaguardaUrl": "https://retaguardash.techone-it.com.br"
}
```
