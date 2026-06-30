# Retaguarda - Papel Na Integracao Com O PDV

## Objetivo

A retaguarda sera o servico central responsavel por controlar empresa, acesso, planos, configuracoes, sincronizacao e armazenamento dos dados operacionais gerados pelo PDV.

O PDV continua sendo offline-first: ele deve operar localmente mesmo sem internet, gravando no SQLite local. A retaguarda entra como fonte central para autenticacao, liberacao da empresa, download de dados mestres e recebimento das movimentacoes feitas no caixa.

## Responsabilidades Da Retaguarda

### 1. Controle De Empresa

A retaguarda deve manter o cadastro central das empresas autorizadas a usar o PDV.

Responsabilidades:

- cadastrar empresa;
- consultar empresa por CNPJ;
- registrar empresa;
- confirmar codigo de ativacao;
- controlar status da empresa;
- liberar ou bloquear uso do PDV conforme registro/plano;
- vincular empresa ao banco operacional correspondente.

Endpoints existentes relacionados:

- `GET /empresa/cnpj/{cnpj}`
- `POST /empresa/registra-empresa`
- `POST /empresa/confere-codigo-confirmacao`
- `POST /empresa/envia-email-confirmacao`

Pendencias:

- padronizar resposta;
- padronizar criptografia;
- definir contrato final para ativacao da empresa;
- definir status oficiais de empresa ativa, pendente, bloqueada e inativa.

## 2. Autenticacao E Usuarios

A retaguarda deve ser a fonte oficial de login do PDV.

Hoje o PDV usa login local/simulado. A retaguarda ainda nao possui endpoint real de login/token. Para a integracao ficar correta, a retaguarda precisa criar um modulo de autenticacao.

Responsabilidades esperadas:

- autenticar usuario;
- validar senha;
- retornar token ou sessao;
- retornar perfil do usuario;
- retornar empresa vinculada;
- retornar permissoes operacionais;
- bloquear acesso se empresa estiver irregular;
- permitir refresh/renovacao da sessao;
- permitir logout/inativacao de sessao.

Perfis minimos:

- operador;
- gerente;
- administrador.

Contrato sugerido:

- `POST /auth/login`
- `POST /auth/refresh`
- `POST /auth/logout`
- `GET /auth/me`

Dados esperados no retorno do login:

- token;
- expiracao;
- id do usuario;
- nome do usuario;
- perfil;
- CNPJ da empresa;
- status da empresa;
- permissoes;
- identificador do dispositivo, quando aplicavel.

## 3. Dois Bancos MySQL

A arquitetura da retaguarda trabalha com dois tipos de banco.

### Banco central

Banco central da retaguarda, hoje identificado como `retaguarda_sh`.

Responsavel por dados da software house e controle geral:

- empresa;
- plano;
- pagamentos;
- configuracao fiscal central;
- configuracao ACBR;
- registro/liberacao;
- dados administrativos.

Tabelas vistas no script atual:

- `EMPRESA`;
- `PDV_TIPO_PLANO`;
- `NFE_CONFIGURACAO`;
- `PDV_PLANO_PAGAMENTO`;
- `ACBR_MONITOR_PORTA`.

### Banco operacional por empresa

Banco separado por empresa/CNPJ.

Responsavel pelos dados operacionais sincronizados com o PDV:

- cliente;
- fornecedor;
- colaborador;
- produto;
- grupo/subgrupo/tipo/unidade;
- tributacao;
- financeiro;
- compras;
- vendas;
- movimento de caixa;
- sangria;
- suprimento;
- fechamento;
- documentos fiscais;
- tabelas auxiliares usadas pelo PDV.

Pendencia critica:

- o banco operacional ainda precisa ser criado/padronizado;
- o nome do banco precisa ser unico e consistente;
- o script de criacao precisa conter todas as tabelas esperadas pelo PDV;
- a retaguarda nao pode alternar entre nomes diferentes para o mesmo banco.

## 4. Sincronizacao

A retaguarda deve receber e devolver dados para manter o PDV local atualizado.

Endpoints existentes:

- `POST /sincroniza/upload`
- `POST /sincroniza/upload-movimento`
- `GET /sincroniza/download`

### Download

Fluxo esperado:

1. PDV informa CNPJ da empresa.
2. Retaguarda identifica o banco operacional da empresa.
3. Retaguarda retorna os dados mestres.
4. PDV grava/atualiza o SQLite local.

Dados esperados no download:

- clientes;
- fornecedores;
- colaboradores;
- produtos;
- unidades;
- formas/tipos de pagamento;
- grupos de produto;
- subgrupos;
- tributacao;
- configuracoes fiscais e operacionais;
- contas a pagar/receber quando aplicavel.

### Upload

Fluxo esperado:

1. PDV salva tudo localmente primeiro.
2. PDV registra pendencias de sincronizacao.
3. Quando houver conexao, envia os dados para a retaguarda.
4. Retaguarda grava no banco operacional da empresa.
5. Retaguarda retorna sucesso/erro por lote.
6. PDV marca os registros como sincronizados.

Dados esperados no upload:

- cadastros criados/alterados no PDV;
- vendas;
- itens da venda;
- pagamentos;
- abertura de caixa;
- suprimentos;
- sangrias;
- fechamento de caixa;
- compras;
- contas a pagar;
- contas a receber;
- documentos fiscais quando existirem.

## 5. Cadastros

A retaguarda deve receber os cadastros feitos no PDV e tambem fornecer cadastros vindos de outros sistemas.

Cadastros principais:

- cliente;
- fornecedor;
- colaborador;
- empresa;
- produto;
- grupo de produto;
- subgrupo de produto;
- tipo de produto;
- unidade;
- forma/tipo de pagamento;
- tributacao.

Comportamento esperado no PDV:

- usuario cadastra localmente;
- registro fica disponivel imediatamente;
- sincronizacao envia para retaguarda;
- se houver erro, registro permanece local e pendente;
- usuario consegue ver status de sincronizacao.

## 6. Operacao De Caixa

A retaguarda deve armazenar o historico operacional do caixa.

Eventos esperados:

- abertura de caixa;
- venda;
- cancelamento de venda, quando implementado;
- suprimento;
- sangria;
- fechamento;
- reabertura;
- totais por forma de pagamento;
- conciliacao financeira.

O PDV deve continuar validando regras criticas localmente, como impedir venda sem caixa aberto.

## 7. Fiscal E NFC-e

A retaguarda ja possui controllers relacionados a configuracao fiscal e ACBR.

Responsabilidades esperadas:

- guardar configuracoes fiscais;
- receber atualizacao de certificado;
- apoiar emissao fiscal;
- armazenar/integrar XML/PDF;
- controlar inutilizacao/cancelamento;
- apoiar contingencia.

Pendencias:

- definir se a emissao fiscal sera feita localmente no PDV, na retaguarda ou em modelo hibrido;
- padronizar retorno dos endpoints fiscais;
- definir como certificados e senhas serao protegidos.

## 8. Seguranca

A retaguarda precisa ser endurecida antes da integracao final.

Pontos a corrigir:

- criar autenticacao real;
- configurar autorizacao por perfil;
- remover senhas/chaves hardcoded;
- trocar credenciais fixas por variaveis de ambiente/secret;
- revisar criptografia atual;
- evitar SQL concatenado com dados de entrada;
- padronizar HTTPS;
- registrar auditoria de login e sincronizacao;
- revisar vulnerabilidade apontada no pacote NHibernate.

## 9. O Que O PDV Precisa Alterar Para Integrar

No PDV sera necessario:

- criar client HTTP para a retaguarda;
- configurar URL da API;
- trocar login local por login remoto;
- armazenar sessao/token localmente de forma segura;
- armazenar CNPJ/empresa ativa;
- armazenar identificador do dispositivo;
- criar servico de sincronizacao;
- criar outbox para operacao offline;
- criar status visual de sincronizacao;
- adaptar cadastros para salvar local e enfileirar envio;
- adaptar venda/caixa/financeiro para upload;
- adaptar importacao/download de dados mestres.

## 10. Ordem Recomendada De Implementacao

1. Padronizar bancos da retaguarda.
2. Criar script do banco operacional por empresa.
3. Criar login/token na retaguarda.
4. Criar contrato de empresa ativa.
5. Criar client HTTP no PDV.
6. Integrar login do PDV.
7. Integrar empresa/CNPJ/dispositivo.
8. Implementar download inicial para popular SQLite.
9. Implementar upload de cadastros.
10. Implementar upload de movimentos de caixa e vendas.
11. Implementar outbox/retry.
12. Criar testes ponta a ponta.

## 11. Criterio De Pronto

A integracao com a retaguarda sera considerada pronta quando:

- retaguarda subir localmente com MySQL;
- empresa puder ser cadastrada/consultada;
- banco operacional da empresa for criado corretamente;
- usuario conseguir logar no PDV via retaguarda;
- PDV baixar dados mestres da retaguarda;
- PDV salvar cadastros localmente e sincronizar;
- PDV enviar venda, caixa e financeiro para a retaguarda;
- dados aparecerem no MySQL correto;
- falha de internet nao impedir venda local;
- sincronizacao retomar automaticamente depois;
- build e smoke test passarem.

## 12. Resumo Executivo

A retaguarda sera o centro de controle e sincronizacao do PDV. Ela nao deve substituir a operacao local do caixa, mas deve garantir autenticacao, liberacao de empresa, dados mestres, armazenamento central das operacoes e integracao fiscal/financeira.

Antes de integrar de verdade, o ponto mais importante e estabilizar os dois bancos MySQL e criar login/token real. Depois disso, o PDV pode evoluir para salvar local, sincronizar com seguranca e operar mesmo offline.
