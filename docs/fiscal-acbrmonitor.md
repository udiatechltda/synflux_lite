# Fiscal NFC-e Com ACBrMonitor

## Arquitetura Atual

O ACBrMonitor deve rodar no mesmo servidor da retaguarda/API.

Fluxo:

1. PDV salva a venda no SQLite.
2. PDV valida dados fiscais obrigatorios da empresa, itens e pagamentos.
3. PDV consulta o status da SEFAZ pela retaguarda.
4. Se a SEFAZ estiver indisponivel, PDV marca a emissao como contingencia.
5. PDV monta o INI da NFC-e a partir da venda, itens, empresa e pagamentos.
6. PDV grava `NFE_CABECALHO`, `NFE_DETALHE` e `NFE_INFORMACAO_PAGAMENTO`.
7. PDV chama a retaguarda em `POST /acbr-monitor/v2/emite-nfce`.
8. Retaguarda grava o INI em `C:\ACBrMonitor\{cnpj}\ini\nfce\`.
9. Retaguarda chama o ACBrMonitor por `ent.txt`.
10. ACBrMonitor gera XML, transmite para SEFAZ e gera DANFE/PDF.
11. Retaguarda retorna status, protocolo, caminho do XML e caminho do PDF.
12. PDV atualiza o status fiscal da nota.

## Variaveis Do PDV

```powershell
$env:PDV_RETAGUARDA_URL="http://localhost:5010"
$env:PDV_FISCAL_ENABLED="true"
$env:PDV_FISCAL_AMBIENTE="homologacao"
```

Opcional:

```powershell
$env:PDV_FISCAL_STRICT="true"
```

Com `PDV_FISCAL_STRICT=true`, uma falha fiscal bloqueia a finalizacao operacional da venda. Sem strict, a venda fica salva e a NFC-e fica com status `ERRO` para tratamento posterior.

## Variaveis Da Retaguarda

Para teste local sem ACBrMonitor real:

```powershell
$env:ACBR_MONITOR_MOCK="true"
```

Para emissao real:

```powershell
$env:ACBR_MONITOR_MOCK="false"
```

No modo real, o servidor precisa ter a estrutura:

```text
C:\ACBrMonitor\{cnpj}\
C:\ACBrMonitor\{cnpj}\ini\nfce\
C:\ACBrMonitor\{cnpj}\LOG_NFe\
C:\ACBrMonitor\{cnpj}\DFes\
```

O serviço cria essas pastas quando possível, mas o ACBrMonitor e o certificado fiscal precisam estar configurados no servidor.

## Teste Automatizado Local

PDV:

```powershell
dotnet run --project PDV.csproj -- --smoke-fiscal-nfce
```

Resultado esperado:

```text
NFC-e OK: numero=1, chave=..., status=AUTORIZADA.
```

Retaguarda/API em modo mock:

```powershell
$env:ACBR_MONITOR_MOCK="true"
dotnet run --project T2TiRetaguardaSH\T2TiRetaguardaSH.csproj --urls "http://localhost:5010"
```

Depois chamar:

```http
POST http://localhost:5010/acbr-monitor/v2/emite-nfce
```

Corpo:

```json
{
  "numero": "1",
  "cnpj": "11222333000181",
  "nfceIniBase64": "...",
  "contingencia": false
}
```

Endpoints fiscais disponiveis:

```http
GET  /acbr-monitor/v2/status-sefaz?uf=SP&cnpj=11222333000181
POST /acbr-monitor/v2/emite-nfce
POST /acbr-monitor/v2/transmite-contingencia
POST /acbr-monitor/v2/cancela-nfce
POST /acbr-monitor/v2/inutiliza-numero
```

## O Que Ja Esta Implementado

- Geracao de INI NFC-e pelo PDV.
- Persistencia em `NFE_CABECALHO`.
- Persistencia em `NFE_DETALHE`.
- Persistencia em `NFE_INFORMACAO_PAGAMENTO`.
- Numeracao fiscal controlada por `NFE_NUMERO`, com trava em memoria para evitar duplicidade local.
- Emissao via retaguarda com ACBrMonitor no servidor.
- Endpoint JSON novo em `acbr-monitor/v2/emite-nfce`.
- Consulta de status SEFAZ antes da emissao.
- Entrada automatica em contingencia quando a SEFAZ esta indisponivel.
- Transmissao posterior de NFC-e em contingencia.
- Cancelamento de NFC-e pela retaguarda.
- Inutilizacao de numero fiscal pela retaguarda.
- Persistencia de protocolo, mensagem fiscal, caminho PDF/XML e status final no `NFE_CABECALHO`.
- Modo mock para teste local sem certificado/SEFAZ.
- Idempotencia: se a venda ja tem NFC-e autorizada, nao emite de novo.

## Validacoes Antes De Emitir

O PDV bloqueia a emissao fiscal quando faltam dados estruturais:

- empresa sem CNPJ, razao social, UF, codigo IBGE, CRT ou endereco;
- item sem quantidade, valor, CST/CSOSN ou NCM em modo real;
- pagamento sem valor ou tipo de pagamento fiscal em modo real;
- total da venda menor ou igual a zero;
- CSC/IdToken NFC-e ausente em modo real quando a configuracao fiscal existir.

Em modo mock, algumas validacoes tributarias sao flexibilizadas para permitir teste local sem certificado/SEFAZ.

## Pontos Que Dependem Do Ambiente Real

- Certificado A1/A3 instalado/configurado no ACBrMonitor.
- CSC/IdToken NFC-e configurado.
- Ambiente SEFAZ homologacao/producao configurado.
- Impressora/DANFE configurada no servidor.
- Permissao de escrita em `C:\ACBrMonitor\{cnpj}`.
