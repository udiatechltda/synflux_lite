# Relatorio De Reescrita Do ACBrMonitor Em C#/.NET

## Objetivo

Este relatorio avalia a reescrita do ACBrMonitor em C#/.NET, criando um novo servico fiscal independente, moderno e desacoplado do PDV.

O objetivo correto e:

```text
Substituir o ACBrMonitor por um servico fiscal proprio em C#/.NET.
```

Nao e apenas integrar com o ACBrMonitor existente. Tambem nao e colocar a regra fiscal dentro do PDV. A proposta e criar um novo componente fiscal, separado, que faca o papel que hoje o ACBrMonitor faz.

Arquitetura desejada:

```text
PDV WPF
  |
  v
Servico Fiscal Tech One em C#/.NET
  |
  v
SEFAZ / SAT / MFE / Provedores fiscais
```

## O Que Sera Reescrito

O ACBrMonitor hoje funciona como um motor fiscal externo. Ele recebe comandos e executa operacoes fiscais, como emissao, cancelamento, inutilizacao, geracao de XML e DANFE.

A reescrita em C# deve recriar essas responsabilidades em um servico proprio.

Escopo funcional esperado:

- emissao de NFC-e;
- emissao de NF-e, se entrar no escopo;
- cancelamento;
- inutilizacao de numeracao;
- contingencia offline;
- transmissao posterior de contingencia;
- consulta de status;
- geracao e validacao de XML;
- assinatura digital;
- uso de certificado A1;
- avaliacao posterior de certificado A3;
- comunicacao com SEFAZ;
- geracao de DANFE/PDF;
- armazenamento de XML autorizado;
- exportacao/download de XML por periodo;
- logs fiscais;
- auditoria de eventos;
- tratamento de rejeicoes SEFAZ;
- configuracao fiscal por empresa;
- ambiente homologacao/producao;
- serie, numero, CSC, id CSC e certificado por empresa.

## Importante Sobre Licenca

O ACBr e open source, mas nao e recomendado copiar codigo fonte do ACBr para dentro do novo produto sem avaliacao juridica.

A abordagem recomendada e uma **reescrita limpa**:

- estudar comportamento e documentacao;
- usar documentacao publica da SEFAZ;
- usar manuais fiscais oficiais;
- criar implementacao propria em C#;
- nao copiar classes, funcoes ou trechos de codigo do ACBr;
- usar o ACBrMonitor apenas como referencia funcional/comparativa durante testes.

Em resumo:

```text
Pode reescrever a funcionalidade.
Nao e recomendado copiar o codigo.
```

## Tecnologia Recomendada

### Linguagem Principal

Recomendacao: **C# com .NET 8 ou superior**.

Motivos:

- stack ja alinhada ao PDV WPF;
- stack ja alinhada a retaguarda .NET;
- bom suporte a APIs;
- bom suporte a Windows Service;
- bom suporte a Worker Service;
- bom suporte a XML;
- bom suporte a certificado digital;
- bom suporte a criptografia;
- boa manutencao pelo time atual;
- boa base para logs, filas e observabilidade.

### Formato Do Novo Produto

O novo ACBrMonitor em C# deve nascer como servico independente.

Componentes sugeridos:

```text
TechOne.Fiscal.Api
TechOne.Fiscal.Worker
TechOne.Fiscal.Domain
TechOne.Fiscal.Infrastructure
TechOne.Fiscal.Sefaz
TechOne.Fiscal.Danfe
TechOne.Fiscal.Admin
```

Responsabilidades:

| Componente | Responsabilidade |
|---|---|
| `Fiscal.Api` | API consumida por PDV, retaguarda e sistemas externos |
| `Fiscal.Worker` | fila, processamento, retry e contingencia |
| `Fiscal.Domain` | modelos fiscais, regras e validacoes |
| `Fiscal.Infrastructure` | banco, arquivos, certificados, storage |
| `Fiscal.Sefaz` | comunicacao com webservices da SEFAZ |
| `Fiscal.Danfe` | geracao de DANFE/PDF/impressao |
| `Fiscal.Admin` | painel de configuracao e diagnostico |

## Arquitetura Proposta

Fluxo de emissao:

```text
PDV envia venda
  |
  v
Servico Fiscal recebe requisicao
  |
  v
Valida dados fiscais
  |
  v
Monta XML NFC-e/NF-e
  |
  v
Assina XML com certificado
  |
  v
Envia para SEFAZ
  |
  v
Recebe autorizacao/rejeicao
  |
  v
Armazena XML/protocolo
  |
  v
Gera DANFE/PDF
  |
  v
Retorna resultado ao PDV
```

Fluxo de contingencia:

```text
PDV solicita emissao
  |
  v
Servico Fiscal identifica falha SEFAZ/internet
  |
  v
Gera NFC-e em contingencia
  |
  v
Armazena pendencia fiscal
  |
  v
Imprime DANFE em contingencia
  |
  v
Worker tenta transmitir depois
```

## Comparativo Com O ACBrMonitor

| Item | ACBrMonitor atual | Novo servico C# |
|---|---|---|
| Tecnologia | Delphi/Lazarus Object Pascal | C#/.NET |
| Operacao | App externo com TXT/socket | API/Worker/Service |
| Integracao | Arquivo texto, socket ou comandos | REST/gRPC/fila |
| Logs | Arquivos e retorno do monitor | Logs estruturados e auditoria |
| Deploy | Instalador/app local | Servico Windows, container ou servidor |
| Manutencao | Depende do ecossistema ACBr | Time Tech One |
| Fiscal | Ja maduro | Precisa ser implementado e homologado |
| Risco inicial | Baixo, por ja existir | Alto, por reescrita fiscal |

## Escopo Minimo Para MVP

Para uma primeira versao utilizavel, o escopo deve ser limitado.

Recomendacao de MVP:

- NFC-e modelo 65;
- certificado A1;
- ambiente de homologacao e producao;
- emissao normal;
- cancelamento;
- inutilizacao;
- DANFE NFC-e;
- XML autorizado;
- consulta de status;
- logs;
- tratamento de rejeicoes mais comuns;
- contingencia offline;
- transmissao posterior de contingencia.

Fora do MVP:

- NF-e completa modelo 55;
- SAT;
- MFE;
- NFS-e;
- CT-e;
- MDF-e;
- certificado A3;
- multiplos provedores municipais;
- todos os cenarios de importacao/exportacao/combustivel/medicamento/veiculo.

## Estimativa De Tempo

### Cenario 1 - MVP NFC-e Em C# Para Homologacao

Prazo estimado: **3 a 4 meses**.

Entregas:

- estrutura do servico fiscal;
- API de emissao;
- modelo base NFC-e;
- geracao XML;
- assinatura com certificado A1;
- envio para ambiente de homologacao;
- retorno de autorizacao/rejeicao;
- armazenamento de XML;
- DANFE simples;
- logs basicos;
- primeiros testes com SEFAZ homologacao.

Risco: medio/alto.

### Cenario 2 - MVP NFC-e Pronto Para Piloto Em Producao

Prazo estimado: **6 a 9 meses**.

Entregas:

- emissao NFC-e em producao;
- cancelamento;
- inutilizacao;
- contingencia;
- transmissao posterior;
- DANFE adequado;
- tratamento de rejeicoes comuns;
- auditoria;
- painel de acompanhamento;
- configuracao por empresa;
- testes manuais e automatizados;
- piloto controlado com poucos clientes.

Risco: alto.

### Cenario 3 - Substituto Robusto Do ACBrMonitor Para NFC-e

Prazo estimado: **9 a 12 meses**.

Entregas:

- NFC-e madura;
- operacao multiempresa;
- logs e suporte;
- contingencia robusta;
- compatibilidade com varios estados;
- validacoes fiscais mais completas;
- monitoramento;
- suporte a atualizacoes fiscais;
- documentacao operacional;
- processo de homologacao interna.

Risco: alto.

### Cenario 4 - Substituto Amplo Do ACBrMonitor

Prazo estimado: **18 a 30 meses ou mais**.

Escopo:

- NFC-e;
- NF-e;
- SAT;
- MFE;
- NFS-e;
- CT-e;
- MDF-e;
- distribuicao de documentos fiscais;
- certificado A1 e A3;
- multiplas UFs;
- multiplos provedores;
- rotinas fiscais avancadas.

Risco: muito alto.

## Estimativa Por Fase

| Fase | Descricao | Prazo |
|---|---|---:|
| 1 | Discovery fiscal e desenho tecnico | 2 a 4 semanas |
| 2 | Arquitetura do servico fiscal C# | 2 a 3 semanas |
| 3 | Modelagem NFC-e e XML base | 4 a 6 semanas |
| 4 | Assinatura digital e certificado A1 | 3 a 5 semanas |
| 5 | Comunicacao SEFAZ homologacao | 4 a 6 semanas |
| 6 | DANFE NFC-e | 3 a 5 semanas |
| 7 | Cancelamento e inutilizacao | 3 a 5 semanas |
| 8 | Contingencia e transmissao posterior | 4 a 8 semanas |
| 9 | Integracao com PDV | 3 a 5 semanas |
| 10 | Testes, homologacao e piloto | 6 a 10 semanas |

As fases podem ter alguma paralelizacao, mas o caminho critico fiscal reduz bastante o ganho de paralelismo.

## Equipe Recomendada

Para reescrever o ACBrMonitor em C# com qualidade:

- 1 arquiteto/backend senior C#;
- 1 desenvolvedor C# senior focado em fiscal/XML/SEFAZ;
- 1 desenvolvedor C# pleno para API, banco e worker;
- 1 desenvolvedor PDV para integracao;
- 1 QA fiscal;
- 1 pessoa com conhecimento fiscal/contabil;
- apoio DevOps para instalacao, logs, certificados e deploy.

Nao recomendado:

- deixar estagiarios responsaveis pelo nucleo fiscal;
- iniciar com NF-e, NFC-e, SAT, MFE e NFS-e ao mesmo tempo;
- copiar codigo do ACBr diretamente;
- colocar regra fiscal dentro do PDV.

Estagiarios podem apoiar em:

- telas administrativas;
- documentacao;
- scripts de teste;
- validacao manual;
- logs;
- consulta de XML;
- painel de acompanhamento.

## Principais Riscos

| Risco | Impacto |
|---|---|
| Rejeicoes SEFAZ por XML incorreto | Alto |
| Assinatura digital incorreta | Alto |
| Mudancas de nota tecnica | Alto |
| Diferencas por UF | Alto |
| Contingencia mal implementada | Alto |
| DANFE fora do padrao | Medio/alto |
| Certificado A3 | Alto |
| Falta de especialista fiscal | Alto |
| Copia indevida de codigo ACBr | Alto |
| Subestimar escopo fiscal | Muito alto |

## Decisao Tecnica Recomendada

Se a decisao for realmente reescrever o ACBrMonitor, a recomendacao e:

1. criar um novo repositorio/solucao para `TechOne.Fiscal`;
2. implementar primeiro somente NFC-e;
3. trabalhar com C#/.NET 8+;
4. usar API + Worker + banco fiscal proprio;
5. manter o PDV apenas como consumidor;
6. usar ACBrMonitor apenas como referencia de comportamento e comparacao em homologacao;
7. nao copiar codigo do ACBr;
8. validar tudo em ambiente de homologacao antes de qualquer piloto;
9. so depois expandir para NF-e, SAT, MFE e outros documentos.

## Resumo Executivo

Reescrever o ACBrMonitor em C# e possivel, mas e um projeto fiscal grande e de alto risco.

Estimativa objetiva:

| Objetivo | Prazo |
|---|---:|
| Prova de conceito NFC-e em C# | 2 a 3 meses |
| MVP NFC-e homologacao | 3 a 4 meses |
| Piloto NFC-e producao | 6 a 9 meses |
| NFC-e robusta substituindo ACBrMonitor | 9 a 12 meses |
| Substituto amplo do ACBrMonitor | 18 a 30 meses ou mais |

Conclusao: a linguagem mais indicada e **C#/.NET**, mas o projeto deve ser tratado como um **novo servico fiscal independente**, nao como parte do PDV. A reescrita deve comecar pequena, por NFC-e, com foco em homologacao, assinatura digital, XML, SEFAZ, DANFE, cancelamento, inutilizacao e contingencia.

## Fontes Consultadas

- Documentacao ACBrMonitor - apresentacao: https://acbr.sourceforge.io/ACBrMonitor/Apresentacao.html
- Documentacao ACBrMonitor - SVN/codigo fonte: https://acbr.sourceforge.io/ACBrMonitor/ComousaroSVN.html
- Documentacao ACBrMonitor - compilacao: https://acbr.sourceforge.io/ACBrMonitor/ComocompilaroACBrMonitor.html
- Documentacao ACBrMonitor - licenca: https://acbr.sourceforge.io/ACBrMonitor/Licenca.html
- Codigo oficial ACBr no SourceForge: https://sourceforge.net/p/acbr/code/HEAD/tree/trunk2/
- ACBr API oficial: https://www.acbr.api.br/
- Documentacao ACBr API: https://dev.acbr.api.br/docs/
