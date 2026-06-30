using PDV.Services;
using PDV.Services.Interfaces;
﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDV.Migrations
{
    /// <inheritdoc />
    public partial class FullSchemaSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CARDAPIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    ModoPreparo = table.Column<string>(type: "TEXT", nullable: true),
                    InfoAlergico = table.Column<string>(type: "TEXT", nullable: true),
                    Ingredientes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARDAPIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CARDAPIO_PERGUNTA_PADRAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCardapio = table.Column<int>(type: "INTEGER", nullable: true),
                    Pergunta = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARDAPIO_PERGUNTA_PADRAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CARDAPIO_RESPOSTA_PADRAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCardapioPerguntaPadrao = table.Column<int>(type: "INTEGER", nullable: true),
                    Resposta = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARDAPIO_RESPOSTA_PADRAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CFOP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<int>(type: "INTEGER", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Aplicacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFOP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Fantasia = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    CpfCnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Rg = table.Column<string>(type: "TEXT", nullable: true),
                    OrgaoRg = table.Column<string>(type: "TEXT", nullable: true),
                    DataEmissaoRg = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Sexo = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    TipoPessoa = table.Column<string>(type: "TEXT", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Contato = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoIbgeCidade = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoIbgeUf = table.Column<int>(type: "INTEGER", nullable: true),
                    FidelidadeAviso = table.Column<string>(type: "TEXT", nullable: true),
                    FidelidadeQuantidade = table.Column<int>(type: "INTEGER", nullable: true),
                    FidelidadeValor = table.Column<double>(type: "REAL", nullable: true),
                    FiadoValorTeto = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE_FIADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorPendente = table.Column<double>(type: "REAL", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE_FIADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COLABORADOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ComissaoVista = table.Column<double>(type: "REAL", nullable: true),
                    ComissaoPrazo = table.Column<double>(type: "REAL", nullable: true),
                    NivelAutorizacao = table.Column<string>(type: "TEXT", nullable: true),
                    EntregadorVeiculo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLABORADOR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdMesa = table.Column<int>(type: "INTEGER", nullable: true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEmpresaDeliveryPedido = table.Column<int>(type: "INTEGER", nullable: true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: true),
                    DataChegada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraChegada = table.Column<string>(type: "TEXT", nullable: true),
                    DataSaida = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraSaida = table.Column<string>(type: "TEXT", nullable: true),
                    ValorSubtotal = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadePessoas = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorPorPessoa = table.Column<double>(type: "REAL", nullable: true),
                    Situacao = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoCompartilhado = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdComanda = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalComplemento = table.Column<double>(type: "REAL", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    GerouPedidoCozinha = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA_DETALHE_COMPLEMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdComandaDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeProduto = table.Column<string>(type: "TEXT", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA_DETALHE_COMPLEMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA_OBSERVACAO_PADRAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA_OBSERVACAO_PADRAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMANDA_PEDIDO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdComanda = table.Column<int>(type: "INTEGER", nullable: true),
                    IdCozinha = table.Column<int>(type: "INTEGER", nullable: true),
                    EntrouNaFila = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SaiuDaFila = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstimativaMinutos = table.Column<int>(type: "INTEGER", nullable: true),
                    Posicao = table.Column<int>(type: "INTEGER", nullable: true),
                    Prioridade = table.Column<string>(type: "TEXT", nullable: true),
                    InicioPreparo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FimPreparo = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA_PEDIDO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMPRA_PEDIDO_CABECALHO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdFornecedor = table.Column<int>(type: "INTEGER", nullable: true),
                    DataPedido = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataPrevisaoEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataPrevisaoPagamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LocalEntrega = table.Column<string>(type: "TEXT", nullable: true),
                    LocalCobranca = table.Column<string>(type: "TEXT", nullable: true),
                    Contato = table.Column<string>(type: "TEXT", nullable: true),
                    ValorSubtotal = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    FormaPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    GeraFinanceiro = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeParcelas = table.Column<int>(type: "INTEGER", nullable: true),
                    DiaPrimeiroVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IntervaloEntreParcelas = table.Column<int>(type: "INTEGER", nullable: true),
                    DiaFixoParcela = table.Column<string>(type: "TEXT", nullable: true),
                    DataRecebimentoItens = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraRecebimentoItens = table.Column<string>(type: "TEXT", nullable: true),
                    AtualizouEstoque = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroDocumentoEntrada = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPRA_PEDIDO_CABECALHO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COMPRA_PEDIDO_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCompraPedidoCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: true),
                    ValorSubtotal = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    Cst = table.Column<string>(type: "TEXT", nullable: true),
                    Csosn = table.Column<string>(type: "TEXT", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPRA_PEDIDO_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONTADOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoCrc = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoIbgeCidade = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoIbgeUf = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTADOR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONTAS_PAGAR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdFornecedor = table.Column<int>(type: "INTEGER", nullable: true),
                    IdCompraPedidoCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValorAPagar = table.Column<double>(type: "REAL", nullable: true),
                    TaxaJuro = table.Column<double>(type: "REAL", nullable: true),
                    TaxaMulta = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorJuro = table.Column<double>(type: "REAL", nullable: true),
                    ValorMulta = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorPago = table.Column<double>(type: "REAL", nullable: true),
                    NumeroDocumento = table.Column<string>(type: "TEXT", nullable: true),
                    Historico = table.Column<string>(type: "TEXT", nullable: true),
                    StatusPagamento = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAS_PAGAR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CONTAS_RECEBER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataRecebimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValorAReceber = table.Column<double>(type: "REAL", nullable: true),
                    TaxaJuro = table.Column<double>(type: "REAL", nullable: true),
                    TaxaMulta = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorJuro = table.Column<double>(type: "REAL", nullable: true),
                    ValorMulta = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorRecebido = table.Column<double>(type: "REAL", nullable: true),
                    NumeroDocumento = table.Column<string>(type: "TEXT", nullable: true),
                    Historico = table.Column<string>(type: "TEXT", nullable: true),
                    StatusRecebimento = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAS_RECEBER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COZINHA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    ImpressoraNome = table.Column<string>(type: "TEXT", nullable: true),
                    ImpressoraEndereco = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COZINHA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DELIVERY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdComanda = table.Column<int>(type: "INTEGER", nullable: true),
                    IdTaxaEntrega = table.Column<int>(type: "INTEGER", nullable: true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeCliente = table.Column<string>(type: "TEXT", nullable: true),
                    TelefonePrincipal = table.Column<string>(type: "TEXT", nullable: true),
                    TelefoneRecado = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    ValorFrete = table.Column<double>(type: "REAL", nullable: true),
                    ValorRecebido = table.Column<double>(type: "REAL", nullable: true),
                    ValorAReceber = table.Column<double>(type: "REAL", nullable: true),
                    ValorSolicitadoTroco = table.Column<double>(type: "REAL", nullable: true),
                    PrevisaoPreparo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InicioPreparo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrevisaoEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SaiuParaEntrega = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Entregue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PrevisaoRetirada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProntoParaRetirada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Retirou = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DELIVERY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DELIVERY_ACERTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataAcerto = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraAcerto = table.Column<string>(type: "TEXT", nullable: true),
                    ValorRecebido = table.Column<double>(type: "REAL", nullable: true),
                    ValorPagoEntregador = table.Column<double>(type: "REAL", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DELIVERY_ACERTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DELIVERY_ACERTO_COMANDA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDeliveryAcerto = table.Column<int>(type: "INTEGER", nullable: true),
                    IdDelivery = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DELIVERY_ACERTO_COMANDA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_ALIQUOTAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalizadorParcial = table.Column<string>(type: "TEXT", nullable: true),
                    EcfIcmsSt = table.Column<string>(type: "TEXT", nullable: true),
                    PafPSt = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_ALIQUOTAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_DOCUMENTOS_EMITIDOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_DOCUMENTOS_EMITIDOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_E3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    MfAdicional = table.Column<string>(type: "TEXT", nullable: true),
                    TipoEcf = table.Column<string>(type: "TEXT", nullable: true),
                    MarcaEcf = table.Column<string>(type: "TEXT", nullable: true),
                    ModeloEcf = table.Column<string>(type: "TEXT", nullable: true),
                    DataEstoque = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraEstoque = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_E3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_IMPRESSORA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    Identificacao = table.Column<string>(type: "TEXT", nullable: true),
                    Mc = table.Column<string>(type: "TEXT", nullable: true),
                    Md = table.Column<string>(type: "TEXT", nullable: true),
                    Vr = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Marca = table.Column<string>(type: "TEXT", nullable: true),
                    Modelo = table.Column<string>(type: "TEXT", nullable: true),
                    ModeloAcbr = table.Column<string>(type: "TEXT", nullable: true),
                    ModeloDocumentoFiscal = table.Column<string>(type: "TEXT", nullable: true),
                    Versao = table.Column<string>(type: "TEXT", nullable: true),
                    Le = table.Column<string>(type: "TEXT", nullable: true),
                    Lef = table.Column<string>(type: "TEXT", nullable: true),
                    Mfd = table.Column<string>(type: "TEXT", nullable: true),
                    LacreNaMfd = table.Column<string>(type: "TEXT", nullable: true),
                    Docto = table.Column<string>(type: "TEXT", nullable: true),
                    DataInstalacaoSb = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraInstalacaoSb = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_IMPRESSORA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_LOG_TOTAIS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoPagamento = table.Column<int>(type: "INTEGER", nullable: true),
                    Produto = table.Column<int>(type: "INTEGER", nullable: true),
                    R01 = table.Column<int>(type: "INTEGER", nullable: true),
                    R02 = table.Column<int>(type: "INTEGER", nullable: true),
                    R03 = table.Column<int>(type: "INTEGER", nullable: true),
                    R04 = table.Column<int>(type: "INTEGER", nullable: true),
                    R05 = table.Column<int>(type: "INTEGER", nullable: true),
                    R06 = table.Column<int>(type: "INTEGER", nullable: true),
                    R07 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_LOG_TOTAIS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_R01",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    CnpjEmpresa = table.Column<string>(type: "TEXT", nullable: true),
                    CnpjSh = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadualSh = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipalSh = table.Column<string>(type: "TEXT", nullable: true),
                    DenominacaoSh = table.Column<string>(type: "TEXT", nullable: true),
                    NomePafEcf = table.Column<string>(type: "TEXT", nullable: true),
                    VersaoPafEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Md5PafEcf = table.Column<string>(type: "TEXT", nullable: true),
                    DataInicial = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFinal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VersaoEr = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroLaudoPaf = table.Column<string>(type: "TEXT", nullable: true),
                    RazaoSocialSh = table.Column<string>(type: "TEXT", nullable: true),
                    EnderecoSh = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroSh = table.Column<string>(type: "TEXT", nullable: true),
                    ComplementoSh = table.Column<string>(type: "TEXT", nullable: true),
                    BairroSh = table.Column<string>(type: "TEXT", nullable: true),
                    CidadeSh = table.Column<string>(type: "TEXT", nullable: true),
                    CepSh = table.Column<string>(type: "TEXT", nullable: true),
                    UfSh = table.Column<string>(type: "TEXT", nullable: true),
                    TelefoneSh = table.Column<string>(type: "TEXT", nullable: true),
                    ContatoSh = table.Column<string>(type: "TEXT", nullable: true),
                    PrincipalExecutavel = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_R01", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_R02",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvOperador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfImpressora = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfCaixa = table.Column<int>(type: "INTEGER", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Crz = table.Column<int>(type: "INTEGER", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    Cro = table.Column<int>(type: "INTEGER", nullable: true),
                    DataMovimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    VendaBruta = table.Column<double>(type: "REAL", nullable: true),
                    GrandeTotal = table.Column<double>(type: "REAL", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_R02", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_R03",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEcfR02 = table.Column<int>(type: "INTEGER", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    TotalizadorParcial = table.Column<string>(type: "TEXT", nullable: true),
                    ValorAcumulado = table.Column<double>(type: "REAL", nullable: true),
                    Crz = table.Column<int>(type: "INTEGER", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_R03", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_R06",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvOperador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfImpressora = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfCaixa = table.Column<int>(type: "INTEGER", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    Gnf = table.Column<int>(type: "INTEGER", nullable: true),
                    Grg = table.Column<int>(type: "INTEGER", nullable: true),
                    Cdc = table.Column<int>(type: "INTEGER", nullable: true),
                    Denominacao = table.Column<string>(type: "TEXT", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_R06", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_R07",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEcfR06 = table.Column<int>(type: "INTEGER", nullable: true),
                    Ccf = table.Column<int>(type: "INTEGER", nullable: true),
                    MeioPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    ValorPagamento = table.Column<double>(type: "REAL", nullable: true),
                    Estorno = table.Column<string>(type: "TEXT", nullable: true),
                    ValorEstorno = table.Column<double>(type: "REAL", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_R07", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_RECEBIMENTO_NAO_FISCAL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    DataRecebimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_RECEBIMENTO_NAO_FISCAL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_RELATORIO_GERENCIAL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvConfiguracao = table.Column<int>(type: "INTEGER", nullable: true),
                    X = table.Column<int>(type: "INTEGER", nullable: true),
                    MeiosPagamento = table.Column<int>(type: "INTEGER", nullable: true),
                    DavEmitidos = table.Column<int>(type: "INTEGER", nullable: true),
                    IdentificacaoPaf = table.Column<int>(type: "INTEGER", nullable: true),
                    ParametrosConfiguracao = table.Column<int>(type: "INTEGER", nullable: true),
                    Outros = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_RELATORIO_GERENCIAL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_SINTEGRA_60A",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEcfSintegra60M = table.Column<int>(type: "INTEGER", nullable: true),
                    SituacaoTributaria = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_SINTEGRA_60A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECF_SINTEGRA_60M",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumeroSerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroEquipamento = table.Column<int>(type: "INTEGER", nullable: true),
                    ModeloDocumentoFiscal = table.Column<string>(type: "TEXT", nullable: true),
                    CooInicial = table.Column<int>(type: "INTEGER", nullable: true),
                    CooFinal = table.Column<int>(type: "INTEGER", nullable: true),
                    Crz = table.Column<int>(type: "INTEGER", nullable: true),
                    Cro = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorVendaBruta = table.Column<double>(type: "REAL", nullable: true),
                    ValorGrandeTotal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECF_SINTEGRA_60M", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RazaoSocial = table.Column<string>(type: "TEXT", nullable: true),
                    NomeFantasia = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    TipoRegime = table.Column<string>(type: "TEXT", nullable: true),
                    Crt = table.Column<string>(type: "TEXT", nullable: true),
                    DataConstituicao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    AliquotaPis = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaCofins = table.Column<double>(type: "REAL", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Fone = table.Column<string>(type: "TEXT", nullable: true),
                    Contato = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoIbgeCidade = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoIbgeUf = table.Column<int>(type: "INTEGER", nullable: true),
                    Logotipo = table.Column<string>(type: "TEXT", nullable: true),
                    Registrado = table.Column<bool>(type: "INTEGER", nullable: true),
                    NaturezaJuridica = table.Column<string>(type: "TEXT", nullable: true),
                    EmailPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    Simei = table.Column<bool>(type: "INTEGER", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA_CNAE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Principal = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA_CNAE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA_DELIVERY_PEDIDO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodigoPedidoEmpresa = table.Column<string>(type: "TEXT", nullable: true),
                    ConteudoJson = table.Column<string>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true),
                    DataSolicitacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraSolicitacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA_DELIVERY_PEDIDO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA_SEGMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Denominacao = table.Column<string>(type: "TEXT", nullable: true),
                    Divisoes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA_SEGMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ENTREGADOR_ROTA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    DataRota = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraSaida = table.Column<string>(type: "TEXT", nullable: true),
                    EstimativaMinutos = table.Column<int>(type: "INTEGER", nullable: true),
                    HoraPrevistoRetorno = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENTREGADOR_ROTA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ENTREGADOR_ROTA_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEntregadorRota = table.Column<int>(type: "INTEGER", nullable: true),
                    IdDelivery = table.Column<int>(type: "INTEGER", nullable: true),
                    PosicaoNaFila = table.Column<int>(type: "INTEGER", nullable: true),
                    Latitude = table.Column<int>(type: "INTEGER", nullable: true),
                    Longitude = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENTREGADOR_ROTA_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIDELIDADE_HISTORICO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    IdFidelidadeUtilizado = table.Column<int>(type: "INTEGER", nullable: true),
                    DataConsumo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraConsumo = table.Column<string>(type: "TEXT", nullable: true),
                    ValorConsumo = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIDELIDADE_HISTORICO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIDELIDADE_UTILIZADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataUtilizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraUtilizacao = table.Column<string>(type: "TEXT", nullable: true),
                    ValorUtilizado = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIDELIDADE_UTILIZADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FORNECEDOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Fantasia = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    CpfCnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Rg = table.Column<string>(type: "TEXT", nullable: true),
                    OrgaoRg = table.Column<string>(type: "TEXT", nullable: true),
                    DataEmissaoRg = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Sexo = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    TipoPessoa = table.Column<string>(type: "TEXT", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Celular = table.Column<string>(type: "TEXT", nullable: true),
                    Contato = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoIbgeCidade = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoIbgeUf = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORNECEDOR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IBPT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ncm = table.Column<string>(type: "TEXT", nullable: true),
                    Ex = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    NacionalFederal = table.Column<double>(type: "REAL", nullable: true),
                    ImportadosFederal = table.Column<double>(type: "REAL", nullable: true),
                    Estadual = table.Column<double>(type: "REAL", nullable: true),
                    Municipal = table.Column<double>(type: "REAL", nullable: true),
                    VigenciaInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VigenciaFim = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Chave = table.Column<string>(type: "TEXT", nullable: true),
                    Versao = table.Column<string>(type: "TEXT", nullable: true),
                    Fonte = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IBPT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LOG_IMPORTACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataImportacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraImportacao = table.Column<string>(type: "TEXT", nullable: true),
                    Erro = table.Column<string>(type: "TEXT", nullable: true),
                    Registro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG_IMPORTACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MESA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeCadeiras = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeCadeirasCrianca = table.Column<int>(type: "INTEGER", nullable: true),
                    Disponivel = table.Column<string>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MESA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFCE_PLANO_PAGAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataSolicitacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TipoPlano = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    StatusPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoTransacao = table.Column<string>(type: "TEXT", nullable: true),
                    MetodoPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoTipoPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    DataPlanoExpira = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFCE_PLANO_PAGAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_ACESSO_XML",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_ACESSO_XML", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CABECALHO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributOperacaoFiscal = table.Column<int>(type: "INTEGER", nullable: true),
                    UfEmitente = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoNumerico = table.Column<string>(type: "TEXT", nullable: true),
                    NaturezaOperacao = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoModelo = table.Column<string>(type: "TEXT", nullable: true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    DataHoraEmissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataHoraEntradaSaida = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TipoOperacao = table.Column<string>(type: "TEXT", nullable: true),
                    LocalDestino = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMunicipio = table.Column<int>(type: "INTEGER", nullable: true),
                    FormatoImpressaoDanfe = table.Column<string>(type: "TEXT", nullable: true),
                    TipoEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    ChaveAcesso = table.Column<string>(type: "TEXT", nullable: true),
                    DigitoChaveAcesso = table.Column<string>(type: "TEXT", nullable: true),
                    Ambiente = table.Column<string>(type: "TEXT", nullable: true),
                    FinalidadeEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    ConsumidorOperacao = table.Column<string>(type: "TEXT", nullable: true),
                    ConsumidorPresenca = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessoEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    VersaoProcessoEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    DataEntradaContingencia = table.Column<DateTime>(type: "TEXT", nullable: true),
                    JustificativaContingencia = table.Column<string>(type: "TEXT", nullable: true),
                    BaseCalculoIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsDesonerado = table.Column<double>(type: "REAL", nullable: true),
                    TotalIcmsFcpUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    TotalIcmsInterestadualUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    TotalIcmsInterestadualUfRemetente = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalFcp = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalFcpSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalFcpStRetido = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalProdutos = table.Column<double>(type: "REAL", nullable: true),
                    ValorFrete = table.Column<double>(type: "REAL", nullable: true),
                    ValorSeguro = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorImpostoImportacao = table.Column<double>(type: "REAL", nullable: true),
                    ValorIpi = table.Column<double>(type: "REAL", nullable: true),
                    ValorIpiDevolvido = table.Column<double>(type: "REAL", nullable: true),
                    ValorPis = table.Column<double>(type: "REAL", nullable: true),
                    ValorCofins = table.Column<double>(type: "REAL", nullable: true),
                    ValorDespesasAcessorias = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalTributos = table.Column<double>(type: "REAL", nullable: true),
                    ValorServicos = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorPisIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorCofinsIssqn = table.Column<double>(type: "REAL", nullable: true),
                    DataPrestacaoServico = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValorDeducaoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    OutrasRetencoesIssqn = table.Column<double>(type: "REAL", nullable: true),
                    DescontoIncondicionadoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    DescontoCondicionadoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    TotalRetencaoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    RegimeEspecialTributacao = table.Column<string>(type: "TEXT", nullable: true),
                    ValorRetidoPis = table.Column<double>(type: "REAL", nullable: true),
                    ValorRetidoCofins = table.Column<double>(type: "REAL", nullable: true),
                    ValorRetidoCsll = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoIrrf = table.Column<double>(type: "REAL", nullable: true),
                    ValorRetidoIrrf = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoPrevidencia = table.Column<double>(type: "REAL", nullable: true),
                    ValorRetidoPrevidencia = table.Column<double>(type: "REAL", nullable: true),
                    InformacoesAddFisco = table.Column<string>(type: "TEXT", nullable: true),
                    InformacoesAddContribuinte = table.Column<string>(type: "TEXT", nullable: true),
                    ComexUfEmbarque = table.Column<string>(type: "TEXT", nullable: true),
                    ComexLocalEmbarque = table.Column<string>(type: "TEXT", nullable: true),
                    ComexLocalDespacho = table.Column<string>(type: "TEXT", nullable: true),
                    CompraNotaEmpenho = table.Column<string>(type: "TEXT", nullable: true),
                    CompraPedido = table.Column<string>(type: "TEXT", nullable: true),
                    CompraContrato = table.Column<string>(type: "TEXT", nullable: true),
                    Qrcode = table.Column<string>(type: "TEXT", nullable: true),
                    UrlChave = table.Column<string>(type: "TEXT", nullable: true),
                    StatusNota = table.Column<string>(type: "TEXT", nullable: true),
                    IdPdvVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CABECALHO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CANA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Safra = table.Column<string>(type: "TEXT", nullable: true),
                    MesAnoReferencia = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CANA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CANA_DEDUCOES_SAFRA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCana = table.Column<int>(type: "INTEGER", nullable: true),
                    Decricao = table.Column<string>(type: "TEXT", nullable: true),
                    ValorDeducao = table.Column<double>(type: "REAL", nullable: true),
                    ValorFornecimento = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalDeducao = table.Column<double>(type: "REAL", nullable: true),
                    ValorLiquidoFornecimento = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CANA_DEDUCOES_SAFRA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CANA_FORNECIMENTO_DIARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCana = table.Column<int>(type: "INTEGER", nullable: true),
                    Dia = table.Column<string>(type: "TEXT", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeTotalMes = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeTotalAnterior = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeTotalGeral = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CANA_FORNECIMENTO_DIARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CONFIGURACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CertificadoDigitalSerie = table.Column<string>(type: "TEXT", nullable: true),
                    CertificadoDigitalCaminho = table.Column<string>(type: "TEXT", nullable: true),
                    CertificadoDigitalSenha = table.Column<string>(type: "TEXT", nullable: true),
                    TipoEmissao = table.Column<int>(type: "INTEGER", nullable: true),
                    FormatoImpressaoDanfe = table.Column<int>(type: "INTEGER", nullable: true),
                    ProcessoEmissao = table.Column<int>(type: "INTEGER", nullable: true),
                    VersaoProcessoEmissao = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoLogomarca = table.Column<string>(type: "TEXT", nullable: true),
                    SalvarXml = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoSalvarXml = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoSchemas = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoArquivoDanfe = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoSalvarPdf = table.Column<string>(type: "TEXT", nullable: true),
                    WebserviceUf = table.Column<string>(type: "TEXT", nullable: true),
                    WebserviceAmbiente = table.Column<int>(type: "INTEGER", nullable: true),
                    WebserviceProxyHost = table.Column<string>(type: "TEXT", nullable: true),
                    WebserviceProxyPorta = table.Column<int>(type: "INTEGER", nullable: true),
                    WebserviceProxyUsuario = table.Column<string>(type: "TEXT", nullable: true),
                    WebserviceProxySenha = table.Column<string>(type: "TEXT", nullable: true),
                    WebserviceVisualizar = table.Column<string>(type: "TEXT", nullable: true),
                    EmailServidorSmtp = table.Column<string>(type: "TEXT", nullable: true),
                    EmailPorta = table.Column<int>(type: "INTEGER", nullable: true),
                    EmailUsuario = table.Column<string>(type: "TEXT", nullable: true),
                    EmailSenha = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAssunto = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAutenticaSsl = table.Column<string>(type: "TEXT", nullable: true),
                    EmailTexto = table.Column<string>(type: "TEXT", nullable: true),
                    NfceIdCsc = table.Column<string>(type: "TEXT", nullable: true),
                    NfceCsc = table.Column<string>(type: "TEXT", nullable: true),
                    NfceModeloImpressao = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImprimirItensUmaLinha = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImprimirDescontoPorItem = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImprimirQrcodeLateral = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImprimirGtin = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImprimirNomeFantasia = table.Column<string>(type: "TEXT", nullable: true),
                    NfceImpressaoTributos = table.Column<string>(type: "TEXT", nullable: true),
                    NfceMargemSuperior = table.Column<double>(type: "REAL", nullable: true),
                    NfceMargemInferior = table.Column<double>(type: "REAL", nullable: true),
                    NfceMargemDireita = table.Column<double>(type: "REAL", nullable: true),
                    NfceMargemEsquerda = table.Column<double>(type: "REAL", nullable: true),
                    NfceResolucaoImpressao = table.Column<int>(type: "INTEGER", nullable: true),
                    RespTecCnpj = table.Column<string>(type: "TEXT", nullable: true),
                    RespTecContato = table.Column<string>(type: "TEXT", nullable: true),
                    RespTecEmail = table.Column<string>(type: "TEXT", nullable: true),
                    RespTecFone = table.Column<string>(type: "TEXT", nullable: true),
                    RespTecIdCsrt = table.Column<string>(type: "TEXT", nullable: true),
                    RespTecHashCsrt = table.Column<string>(type: "TEXT", nullable: true),
                    NfceTamanhoFonteItem = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CONFIGURACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CTE_REFERENCIADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    ChaveAcesso = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CTE_REFERENCIADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_CUPOM_FISCAL_REFERENCIADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    ModeloDocumentoFiscal = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroOrdemEcf = table.Column<int>(type: "INTEGER", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    DataEmissaoCupom = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumeroCaixa = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroSerieEcf = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_CUPOM_FISCAL_REFERENCIADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DECLARACAO_IMPORTACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroDocumento = table.Column<string>(type: "TEXT", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LocalDesembaraco = table.Column<string>(type: "TEXT", nullable: true),
                    UfDesembaraco = table.Column<string>(type: "TEXT", nullable: true),
                    DataDesembaraco = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ViaTransporte = table.Column<string>(type: "TEXT", nullable: true),
                    ValorAfrmm = table.Column<double>(type: "REAL", nullable: true),
                    FormaIntermediacao = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    UfTerceiro = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoExportador = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DECLARACAO_IMPORTACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DESTINATARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    EstrangeiroIdentificacao = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMunicipio = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeMunicipio = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPais = table.Column<int>(type: "INTEGER", nullable: true),
                    NomePais = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    IndicadorIe = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    Suframa = table.Column<int>(type: "INTEGER", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DESTINATARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DET_ESPECIFICO_ARMAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    TipoArma = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroSerieArma = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroSerieCano = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DET_ESPECIFICO_ARMAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DET_ESPECIFICO_COMBUSTIVEL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoAnp = table.Column<int>(type: "INTEGER", nullable: true),
                    DescricaoAnp = table.Column<string>(type: "TEXT", nullable: true),
                    PercentualGlp = table.Column<double>(type: "REAL", nullable: true),
                    PercentualGasNacional = table.Column<double>(type: "REAL", nullable: true),
                    PercentualGasImportado = table.Column<double>(type: "REAL", nullable: true),
                    ValorPartida = table.Column<double>(type: "REAL", nullable: true),
                    Codif = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeTempAmbiente = table.Column<double>(type: "REAL", nullable: true),
                    UfConsumo = table.Column<string>(type: "TEXT", nullable: true),
                    CideBaseCalculo = table.Column<double>(type: "REAL", nullable: true),
                    CideAliquota = table.Column<double>(type: "REAL", nullable: true),
                    CideValor = table.Column<double>(type: "REAL", nullable: true),
                    EncerranteBico = table.Column<int>(type: "INTEGER", nullable: true),
                    EncerranteBomba = table.Column<int>(type: "INTEGER", nullable: true),
                    EncerranteTanque = table.Column<int>(type: "INTEGER", nullable: true),
                    EncerranteValorInicio = table.Column<double>(type: "REAL", nullable: true),
                    EncerranteValorFim = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DET_ESPECIFICO_COMBUSTIVEL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DET_ESPECIFICO_MEDICAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoAnvisa = table.Column<string>(type: "TEXT", nullable: true),
                    MotivoIsencao = table.Column<string>(type: "TEXT", nullable: true),
                    PrecoMaximoConsumidor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DET_ESPECIFICO_MEDICAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DET_ESPECIFICO_VEICULO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    TipoOperacao = table.Column<string>(type: "TEXT", nullable: true),
                    Chassi = table.Column<string>(type: "TEXT", nullable: true),
                    Cor = table.Column<string>(type: "TEXT", nullable: true),
                    DescricaoCor = table.Column<string>(type: "TEXT", nullable: true),
                    PotenciaMotor = table.Column<string>(type: "TEXT", nullable: true),
                    Cilindradas = table.Column<string>(type: "TEXT", nullable: true),
                    PesoLiquido = table.Column<string>(type: "TEXT", nullable: true),
                    PesoBruto = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroSerie = table.Column<string>(type: "TEXT", nullable: true),
                    TipoCombustivel = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroMotor = table.Column<string>(type: "TEXT", nullable: true),
                    CapacidadeMaximaTracao = table.Column<string>(type: "TEXT", nullable: true),
                    DistanciaEixos = table.Column<string>(type: "TEXT", nullable: true),
                    AnoModelo = table.Column<string>(type: "TEXT", nullable: true),
                    AnoFabricacao = table.Column<string>(type: "TEXT", nullable: true),
                    TipoPintura = table.Column<string>(type: "TEXT", nullable: true),
                    TipoVeiculo = table.Column<string>(type: "TEXT", nullable: true),
                    EspecieVeiculo = table.Column<string>(type: "TEXT", nullable: true),
                    CondicaoVin = table.Column<string>(type: "TEXT", nullable: true),
                    CondicaoVeiculo = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMarcaModelo = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoCorDenatran = table.Column<string>(type: "TEXT", nullable: true),
                    LotacaoMaxima = table.Column<int>(type: "INTEGER", nullable: true),
                    Restricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DET_ESPECIFICO_VEICULO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroItem = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoProduto = table.Column<string>(type: "TEXT", nullable: true),
                    Gtin = table.Column<string>(type: "TEXT", nullable: true),
                    NomeProduto = table.Column<string>(type: "TEXT", nullable: true),
                    Ncm = table.Column<string>(type: "TEXT", nullable: true),
                    Nve = table.Column<string>(type: "TEXT", nullable: true),
                    Cest = table.Column<string>(type: "TEXT", nullable: true),
                    IndicadorEscalaRelevante = table.Column<string>(type: "TEXT", nullable: true),
                    CnpjFabricante = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoBeneficioFiscal = table.Column<string>(type: "TEXT", nullable: true),
                    ExTipi = table.Column<int>(type: "INTEGER", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    UnidadeComercial = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeComercial = table.Column<double>(type: "REAL", nullable: true),
                    NumeroPedidoCompra = table.Column<string>(type: "TEXT", nullable: true),
                    ItemPedidoCompra = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroFci = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroRecopi = table.Column<string>(type: "TEXT", nullable: true),
                    ValorUnitarioComercial = table.Column<double>(type: "REAL", nullable: true),
                    ValorBrutoProduto = table.Column<double>(type: "REAL", nullable: true),
                    GtinUnidadeTributavel = table.Column<string>(type: "TEXT", nullable: true),
                    UnidadeTributavel = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeTributavel = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnitarioTributavel = table.Column<double>(type: "REAL", nullable: true),
                    ValorFrete = table.Column<double>(type: "REAL", nullable: true),
                    ValorSeguro = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorOutrasDespesas = table.Column<double>(type: "REAL", nullable: true),
                    EntraTotal = table.Column<string>(type: "TEXT", nullable: true),
                    ValorTotalTributos = table.Column<double>(type: "REAL", nullable: true),
                    PercentualDevolvido = table.Column<double>(type: "REAL", nullable: true),
                    ValorIpiDevolvido = table.Column<double>(type: "REAL", nullable: true),
                    InformacoesAdicionais = table.Column<string>(type: "TEXT", nullable: true),
                    ValorSubtotal = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_COFINS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    CstCofins = table.Column<string>(type: "TEXT", nullable: true),
                    BaseCalculoCofins = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaCofinsPercentual = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeVendida = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaCofinsReais = table.Column<double>(type: "REAL", nullable: true),
                    ValorCofins = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_COFINS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_COFINS_ST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    VBcCofinsSt = table.Column<double>(type: "REAL", nullable: true),
                    PcofinsAliqSt = table.Column<double>(type: "REAL", nullable: true),
                    VCofinsSt = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_COFINS_ST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_ICMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    OrigemMercadoria = table.Column<string>(type: "TEXT", nullable: true),
                    CstIcms = table.Column<string>(type: "TEXT", nullable: true),
                    Csosn = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBcIcms = table.Column<string>(type: "TEXT", nullable: true),
                    PercentualReducaoBcIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcIcms = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsOperacao = table.Column<double>(type: "REAL", nullable: true),
                    PercentualDiferimento = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsDiferido = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcms = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoFcp = table.Column<double>(type: "REAL", nullable: true),
                    PercentualFcp = table.Column<double>(type: "REAL", nullable: true),
                    ValorFcp = table.Column<double>(type: "REAL", nullable: true),
                    ModalidadeBcIcmsSt = table.Column<string>(type: "TEXT", nullable: true),
                    PercentualMvaIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    PercentualReducaoBcIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorBaseCalculoIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoFcpSt = table.Column<double>(type: "REAL", nullable: true),
                    PercentualFcpSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorFcpSt = table.Column<double>(type: "REAL", nullable: true),
                    UfSt = table.Column<string>(type: "TEXT", nullable: true),
                    PercentualBcOperacaoPropria = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcIcmsStRetido = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaSuportadaConsumidor = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsSubstituto = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsStRetido = table.Column<double>(type: "REAL", nullable: true),
                    BaseCalculoFcpStRetido = table.Column<double>(type: "REAL", nullable: true),
                    PercentualFcpStRetido = table.Column<double>(type: "REAL", nullable: true),
                    ValorFcpStRetido = table.Column<double>(type: "REAL", nullable: true),
                    MotivoDesoneracaoIcms = table.Column<string>(type: "TEXT", nullable: true),
                    ValorIcmsDesonerado = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaCreditoIcmsSn = table.Column<double>(type: "REAL", nullable: true),
                    ValorCreditoIcmsSn = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcIcmsStDestino = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsStDestino = table.Column<double>(type: "REAL", nullable: true),
                    PercentualReducaoBcEfetivo = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcEfetivo = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIcmsEfetivo = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsEfetivo = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_ICMS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_ICMS_UFDEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorBcIcmsUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcFcpUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    PercentualFcpUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaInternaUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaInteresdatualUfEnvolvidas = table.Column<double>(type: "REAL", nullable: true),
                    PercentualProvisorioPartilhaIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsFcpUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    ValorInterestadualUfDestino = table.Column<double>(type: "REAL", nullable: true),
                    ValorInterestadualUfRemetente = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_ICMS_UFDEST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_II",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorBcIi = table.Column<double>(type: "REAL", nullable: true),
                    ValorDespesasAduaneiras = table.Column<double>(type: "REAL", nullable: true),
                    ValorImpostoImportacao = table.Column<double>(type: "REAL", nullable: true),
                    ValorIof = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_II", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_IPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    CnpjProdutor = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoSeloIpi = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeSeloIpi = table.Column<int>(type: "INTEGER", nullable: true),
                    EnquadramentoLegalIpi = table.Column<string>(type: "TEXT", nullable: true),
                    CstIpi = table.Column<string>(type: "TEXT", nullable: true),
                    ValorBaseCalculoIpi = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeUnidadeTributavel = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnidadeTributavel = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIpi = table.Column<double>(type: "REAL", nullable: true),
                    ValorIpi = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_IPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_ISSQN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    BaseCalculoIssqn = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorIssqn = table.Column<double>(type: "REAL", nullable: true),
                    MunicipioIssqn = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemListaServicos = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorDeducao = table.Column<double>(type: "REAL", nullable: true),
                    ValorOutrasRetencoes = table.Column<double>(type: "REAL", nullable: true),
                    ValorDescontoIncondicionado = table.Column<double>(type: "REAL", nullable: true),
                    ValorDescontoCondicionado = table.Column<double>(type: "REAL", nullable: true),
                    ValorRetencaoIss = table.Column<double>(type: "REAL", nullable: true),
                    IndicadorExigibilidadeIss = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoServico = table.Column<string>(type: "TEXT", nullable: true),
                    MunicipioIncidencia = table.Column<int>(type: "INTEGER", nullable: true),
                    PaisSevicoPrestado = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroProcesso = table.Column<string>(type: "TEXT", nullable: true),
                    IndicadorIncentivoFiscal = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_ISSQN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_PIS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    CstPis = table.Column<string>(type: "TEXT", nullable: true),
                    ValorBaseCalculoPis = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPisPercentual = table.Column<double>(type: "REAL", nullable: true),
                    ValorPis = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeVendida = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPisReais = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_PIS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DETALHE_IMPOSTO_PIS_ST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    ValorBaseCalculoPisSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPisStPercentual = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeVendidaPisSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPisStReais = table.Column<double>(type: "REAL", nullable: true),
                    ValorPisSt = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DETALHE_IMPOSTO_PIS_ST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_DUPLICATA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeFatura = table.Column<int>(type: "INTEGER", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_DUPLICATA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_EMITENTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Fantasia = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMunicipio = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeMunicipio = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPais = table.Column<int>(type: "INTEGER", nullable: true),
                    NomePais = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadualSt = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "TEXT", nullable: true),
                    Cnae = table.Column<string>(type: "TEXT", nullable: true),
                    Crt = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_EMITENTE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_EXPORTACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    Drawback = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroRegistro = table.Column<int>(type: "INTEGER", nullable: true),
                    ChaveAcesso = table.Column<string>(type: "TEXT", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_EXPORTACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_FATURA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    ValorOriginal = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorLiquido = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_FATURA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_IMPORTACAO_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDeclaracaoImportacao = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroAdicao = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroSequencial = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoFabricanteEstrangeiro = table.Column<string>(type: "TEXT", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    Drawback = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_IMPORTACAO_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_INFORMACAO_PAGAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    IndicadorPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    MeioPagamento = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    TipoIntegracao = table.Column<string>(type: "TEXT", nullable: true),
                    CnpjOperadoraCartao = table.Column<string>(type: "TEXT", nullable: true),
                    Bandeira = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroAutorizacao = table.Column<string>(type: "TEXT", nullable: true),
                    Troco = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_INFORMACAO_PAGAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_ITEM_RASTREADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeDetalhe = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroLote = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadeItens = table.Column<double>(type: "REAL", nullable: true),
                    DataFabricacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataValidade = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CodigoAgregacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_ITEM_RASTREADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_LOCAL_ENTREGA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    NomeRecebedor = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMunicipio = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeMunicipio = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPais = table.Column<int>(type: "INTEGER", nullable: true),
                    NomePais = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_LOCAL_ENTREGA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_LOCAL_RETIRADA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    NomeExpedidor = table.Column<string>(type: "TEXT", nullable: true),
                    Logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoMunicipio = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeMunicipio = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPais = table.Column<int>(type: "INTEGER", nullable: true),
                    NomePais = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_LOCAL_RETIRADA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_NF_REFERENCIADA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoUf = table.Column<int>(type: "INTEGER", nullable: true),
                    AnoMes = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Modelo = table.Column<string>(type: "TEXT", nullable: true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroNf = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_NF_REFERENCIADA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_NUMERO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Modelo = table.Column<string>(type: "TEXT", nullable: true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_NUMERO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_NUMERO_INUTILIZADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<int>(type: "INTEGER", nullable: true),
                    DataInutilizacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_NUMERO_INUTILIZADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_PROCESSO_REFERENCIADO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Identificador = table.Column<string>(type: "TEXT", nullable: true),
                    Origem = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_PROCESSO_REFERENCIADO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_PROD_RURAL_REFERENCIADA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoUf = table.Column<int>(type: "INTEGER", nullable: true),
                    AnoMes = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    Modelo = table.Column<string>(type: "TEXT", nullable: true),
                    Serie = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroNf = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_PROD_RURAL_REFERENCIADA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_REFERENCIADA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    ChaveAcesso = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_REFERENCIADA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_RESPONSAVEL_TECNICO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Contato = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telefone = table.Column<string>(type: "TEXT", nullable: true),
                    IdentificadorCsrt = table.Column<string>(type: "TEXT", nullable: true),
                    HashCsrt = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_RESPONSAVEL_TECNICO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_TRANSPORTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    ModalidadeFrete = table.Column<string>(type: "TEXT", nullable: true),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: true),
                    Cpf = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "TEXT", nullable: true),
                    Endereco = table.Column<string>(type: "TEXT", nullable: true),
                    NomeMunicipio = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    ValorServico = table.Column<double>(type: "REAL", nullable: true),
                    ValorBcRetencaoIcms = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaRetencaoIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsRetido = table.Column<double>(type: "REAL", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Municipio = table.Column<int>(type: "INTEGER", nullable: true),
                    PlacaVeiculo = table.Column<string>(type: "TEXT", nullable: true),
                    UfVeiculo = table.Column<string>(type: "TEXT", nullable: true),
                    RntcVeiculo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_TRANSPORTE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_TRANSPORTE_REBOQUE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeTransporte = table.Column<int>(type: "INTEGER", nullable: true),
                    Placa = table.Column<string>(type: "TEXT", nullable: true),
                    Uf = table.Column<string>(type: "TEXT", nullable: true),
                    Rntc = table.Column<string>(type: "TEXT", nullable: true),
                    Vagao = table.Column<string>(type: "TEXT", nullable: true),
                    Balsa = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_TRANSPORTE_REBOQUE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_TRANSPORTE_VOLUME",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeTransporte = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: true),
                    Especie = table.Column<string>(type: "TEXT", nullable: true),
                    Marca = table.Column<string>(type: "TEXT", nullable: true),
                    Numeracao = table.Column<string>(type: "TEXT", nullable: true),
                    PesoLiquido = table.Column<double>(type: "REAL", nullable: true),
                    PesoBruto = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_TRANSPORTE_VOLUME", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NFE_TRANSPORTE_VOLUME_LACRE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdNfeTransporteVolume = table.Column<int>(type: "INTEGER", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFE_TRANSPORTE_VOLUME_LACRE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_CAIXA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_CAIXA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_CONFIGURACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEcfImpressora = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvCaixa = table.Column<int>(type: "INTEGER", nullable: true),
                    IdTributOperacaoFiscalPadrao = table.Column<int>(type: "INTEGER", nullable: true),
                    MensagemCupom = table.Column<string>(type: "TEXT", nullable: true),
                    PortaEcf = table.Column<string>(type: "TEXT", nullable: true),
                    IpServidor = table.Column<string>(type: "TEXT", nullable: true),
                    IpSitef = table.Column<string>(type: "TEXT", nullable: true),
                    TipoTef = table.Column<string>(type: "TEXT", nullable: true),
                    TituloTelaCaixa = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoImagensProdutos = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoImagensMarketing = table.Column<string>(type: "TEXT", nullable: true),
                    CorJanelasInternas = table.Column<string>(type: "TEXT", nullable: true),
                    MarketingAtivo = table.Column<string>(type: "TEXT", nullable: true),
                    CfopEcf = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeoutEcf = table.Column<int>(type: "INTEGER", nullable: true),
                    IntervaloEcf = table.Column<int>(type: "INTEGER", nullable: true),
                    DescricaoSuprimento = table.Column<string>(type: "TEXT", nullable: true),
                    DescricaoSangria = table.Column<string>(type: "TEXT", nullable: true),
                    TefTipoGp = table.Column<int>(type: "INTEGER", nullable: true),
                    TefTempoEspera = table.Column<int>(type: "INTEGER", nullable: true),
                    TefEsperaSts = table.Column<int>(type: "INTEGER", nullable: true),
                    TefNumeroVias = table.Column<int>(type: "INTEGER", nullable: true),
                    DecimaisQuantidade = table.Column<int>(type: "INTEGER", nullable: true),
                    DecimaisValor = table.Column<int>(type: "INTEGER", nullable: true),
                    BitsPorSegundo = table.Column<int>(type: "INTEGER", nullable: true),
                    QuantidadeMaximaCartoes = table.Column<int>(type: "INTEGER", nullable: true),
                    PesquisaParte = table.Column<string>(type: "TEXT", nullable: true),
                    Laudo = table.Column<string>(type: "TEXT", nullable: true),
                    DataAtualizacaoEstoque = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PedeCpfCupom = table.Column<string>(type: "TEXT", nullable: true),
                    TipoIntegracao = table.Column<int>(type: "INTEGER", nullable: true),
                    TimerIntegracao = table.Column<int>(type: "INTEGER", nullable: true),
                    GavetaSinalInvertido = table.Column<string>(type: "TEXT", nullable: true),
                    GavetaUtilizacao = table.Column<int>(type: "INTEGER", nullable: true),
                    UsaTecladoReduzido = table.Column<string>(type: "TEXT", nullable: true),
                    Modulo = table.Column<string>(type: "TEXT", nullable: true),
                    Plano = table.Column<string>(type: "TEXT", nullable: true),
                    PlanoValor = table.Column<double>(type: "REAL", nullable: true),
                    PlanoSituacao = table.Column<string>(type: "TEXT", nullable: true),
                    ReciboFormatoPagina = table.Column<string>(type: "TEXT", nullable: true),
                    ReciboLarguraPagina = table.Column<double>(type: "REAL", nullable: true),
                    ReciboMargemPagina = table.Column<double>(type: "REAL", nullable: true),
                    EncerraMovimentoAuto = table.Column<string>(type: "TEXT", nullable: true),
                    PermiteEstoqueNegativo = table.Column<string>(type: "TEXT", nullable: true),
                    ModuloFiscalPrincipal = table.Column<string>(type: "TEXT", nullable: true),
                    ModuloFiscalContingencia = table.Column<string>(type: "TEXT", nullable: true),
                    AcbrMonitorEndereco = table.Column<string>(type: "TEXT", nullable: true),
                    AcbrMonitorPorta = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_CONFIGURACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_CONFIGURACAO_BALANCA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvConfiguracao = table.Column<int>(type: "INTEGER", nullable: true),
                    Modelo = table.Column<int>(type: "INTEGER", nullable: true),
                    Identificador = table.Column<string>(type: "TEXT", nullable: true),
                    HandShake = table.Column<int>(type: "INTEGER", nullable: true),
                    Parity = table.Column<int>(type: "INTEGER", nullable: true),
                    StopBits = table.Column<int>(type: "INTEGER", nullable: true),
                    DataBits = table.Column<int>(type: "INTEGER", nullable: true),
                    BaudRate = table.Column<int>(type: "INTEGER", nullable: true),
                    Porta = table.Column<string>(type: "TEXT", nullable: true),
                    Timeout = table.Column<int>(type: "INTEGER", nullable: true),
                    TipoConfiguracao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_CONFIGURACAO_BALANCA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_CONFIGURACAO_LEITOR_SERIAL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvConfiguracao = table.Column<int>(type: "INTEGER", nullable: true),
                    Usa = table.Column<string>(type: "TEXT", nullable: true),
                    Porta = table.Column<string>(type: "TEXT", nullable: true),
                    Baud = table.Column<int>(type: "INTEGER", nullable: true),
                    HandShake = table.Column<int>(type: "INTEGER", nullable: true),
                    Parity = table.Column<int>(type: "INTEGER", nullable: true),
                    StopBits = table.Column<int>(type: "INTEGER", nullable: true),
                    DataBits = table.Column<int>(type: "INTEGER", nullable: true),
                    Intervalo = table.Column<int>(type: "INTEGER", nullable: true),
                    UsarFila = table.Column<string>(type: "TEXT", nullable: true),
                    HardFlow = table.Column<string>(type: "TEXT", nullable: true),
                    SoftFlow = table.Column<string>(type: "TEXT", nullable: true),
                    Sufixo = table.Column<string>(type: "TEXT", nullable: true),
                    ExcluirSufixo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_CONFIGURACAO_LEITOR_SERIAL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_FECHAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvTipoPagamento = table.Column<int>(type: "INTEGER", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_FECHAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_MOVIMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEcfImpressora = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvOperador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvCaixa = table.Column<int>(type: "INTEGER", nullable: true),
                    IdGerenteSupervisor = table.Column<int>(type: "INTEGER", nullable: true),
                    DataAbertura = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraAbertura = table.Column<string>(type: "TEXT", nullable: true),
                    DataFechamento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraFechamento = table.Column<string>(type: "TEXT", nullable: true),
                    TotalSuprimento = table.Column<double>(type: "REAL", nullable: true),
                    TotalSangria = table.Column<double>(type: "REAL", nullable: true),
                    TotalNaoFiscal = table.Column<double>(type: "REAL", nullable: true),
                    TotalVenda = table.Column<double>(type: "REAL", nullable: true),
                    TotalDesconto = table.Column<double>(type: "REAL", nullable: true),
                    TotalAcrescimo = table.Column<double>(type: "REAL", nullable: true),
                    TotalFinal = table.Column<double>(type: "REAL", nullable: true),
                    TotalRecebido = table.Column<double>(type: "REAL", nullable: true),
                    TotalTroco = table.Column<double>(type: "REAL", nullable: true),
                    TotalCancelado = table.Column<double>(type: "REAL", nullable: true),
                    StatusMovimento = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_MOVIMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_OPERADOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Senha = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_OPERADOR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_SANGRIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    DataSangria = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraSangria = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_SANGRIA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_SUPRIMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    DataSuprimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraSuprimento = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_SUPRIMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_TIPO_PAGAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Tef = table.Column<string>(type: "TEXT", nullable: true),
                    ImprimeVinculado = table.Column<string>(type: "TEXT", nullable: true),
                    PermiteTroco = table.Column<string>(type: "TEXT", nullable: true),
                    TefTipoGp = table.Column<string>(type: "TEXT", nullable: true),
                    GeraParcelas = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPagamentoNfce = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_TIPO_PAGAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_TOTAL_TIPO_PAGAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdPdvVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvTipoPagamento = table.Column<int>(type: "INTEGER", nullable: true),
                    DataVenda = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraVenda = table.Column<string>(type: "TEXT", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    Ccf = table.Column<int>(type: "INTEGER", nullable: true),
                    Gnf = table.Column<int>(type: "INTEGER", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    Nsu = table.Column<string>(type: "TEXT", nullable: true),
                    Estorno = table.Column<string>(type: "TEXT", nullable: true),
                    Rede = table.Column<string>(type: "TEXT", nullable: true),
                    CartaoDc = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_TOTAL_TIPO_PAGAMENTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_VENDA_CABECALHO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    IdColaborador = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvMovimento = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfDav = table.Column<int>(type: "INTEGER", nullable: true),
                    IdEcfPreVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    Ccf = table.Column<int>(type: "INTEGER", nullable: true),
                    DataVenda = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraVenda = table.Column<string>(type: "TEXT", nullable: true),
                    ValorVenda = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    TaxaAcrescimo = table.Column<double>(type: "REAL", nullable: true),
                    ValorAcrescimo = table.Column<double>(type: "REAL", nullable: true),
                    ValorFinal = table.Column<double>(type: "REAL", nullable: true),
                    ValorRecebido = table.Column<double>(type: "REAL", nullable: true),
                    ValorTroco = table.Column<double>(type: "REAL", nullable: true),
                    ValorCancelado = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalProdutos = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalDocumento = table.Column<double>(type: "REAL", nullable: true),
                    ValorBaseIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcmsOutras = table.Column<double>(type: "REAL", nullable: true),
                    ValorIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorPis = table.Column<double>(type: "REAL", nullable: true),
                    ValorCofins = table.Column<double>(type: "REAL", nullable: true),
                    ValorAcrescimoItens = table.Column<double>(type: "REAL", nullable: true),
                    ValorDescontoItens = table.Column<double>(type: "REAL", nullable: true),
                    StatusVenda = table.Column<string>(type: "TEXT", nullable: true),
                    NomeCliente = table.Column<string>(type: "TEXT", nullable: true),
                    CpfCnpjCliente = table.Column<string>(type: "TEXT", nullable: true),
                    CupomCancelado = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true),
                    TipoOperacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_VENDA_CABECALHO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDV_VENDA_DETALHE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    IdPdvVendaCabecalho = table.Column<int>(type: "INTEGER", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Gtin = table.Column<string>(type: "TEXT", nullable: true),
                    Ccf = table.Column<int>(type: "INTEGER", nullable: true),
                    Coo = table.Column<int>(type: "INTEGER", nullable: true),
                    SerieEcf = table.Column<string>(type: "TEXT", nullable: true),
                    Item = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotal = table.Column<double>(type: "REAL", nullable: true),
                    ValorTotalItem = table.Column<double>(type: "REAL", nullable: true),
                    ValorBaseIcms = table.Column<double>(type: "REAL", nullable: true),
                    TaxaIcms = table.Column<double>(type: "REAL", nullable: true),
                    ValorIcms = table.Column<double>(type: "REAL", nullable: true),
                    TaxaDesconto = table.Column<double>(type: "REAL", nullable: true),
                    ValorDesconto = table.Column<double>(type: "REAL", nullable: true),
                    TaxaIssqn = table.Column<double>(type: "REAL", nullable: true),
                    ValorIssqn = table.Column<double>(type: "REAL", nullable: true),
                    TaxaPis = table.Column<double>(type: "REAL", nullable: true),
                    ValorPis = table.Column<double>(type: "REAL", nullable: true),
                    TaxaCofins = table.Column<double>(type: "REAL", nullable: true),
                    ValorCofins = table.Column<double>(type: "REAL", nullable: true),
                    TaxaAcrescimo = table.Column<double>(type: "REAL", nullable: true),
                    ValorAcrescimo = table.Column<double>(type: "REAL", nullable: true),
                    TotalizadorParcial = table.Column<string>(type: "TEXT", nullable: true),
                    Cst = table.Column<string>(type: "TEXT", nullable: true),
                    Cancelado = table.Column<string>(type: "TEXT", nullable: true),
                    MovimentaEstoque = table.Column<string>(type: "TEXT", nullable: true),
                    EcfIcmsSt = table.Column<string>(type: "TEXT", nullable: true),
                    ValorImpostoFederal = table.Column<double>(type: "REAL", nullable: true),
                    ValorImpostoEstadual = table.Column<double>(type: "REAL", nullable: true),
                    ValorImpostoMunicipal = table.Column<double>(type: "REAL", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDV_VENDA_DETALHE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProdutoUnidade = table.Column<int>(type: "INTEGER", nullable: true),
                    IdTributGrupoTributario = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProdutoTipo = table.Column<int>(type: "INTEGER", nullable: true),
                    IdProdutoSubgrupo = table.Column<int>(type: "INTEGER", nullable: true),
                    Gtin = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoInterno = table.Column<string>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    DescricaoPdv = table.Column<string>(type: "TEXT", nullable: true),
                    ValorCompra = table.Column<double>(type: "REAL", nullable: true),
                    ValorVenda = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeEstoque = table.Column<double>(type: "REAL", nullable: true),
                    EstoqueMinimo = table.Column<double>(type: "REAL", nullable: true),
                    EstoqueMaximo = table.Column<double>(type: "REAL", nullable: true),
                    CodigoNcm = table.Column<string>(type: "TEXT", nullable: true),
                    Iat = table.Column<string>(type: "TEXT", nullable: true),
                    Ippt = table.Column<string>(type: "TEXT", nullable: true),
                    TipoItemSped = table.Column<string>(type: "TEXT", nullable: true),
                    TaxaIpi = table.Column<double>(type: "REAL", nullable: true),
                    TaxaIssqn = table.Column<double>(type: "REAL", nullable: true),
                    TaxaPis = table.Column<double>(type: "REAL", nullable: true),
                    TaxaCofins = table.Column<double>(type: "REAL", nullable: true),
                    TaxaIcms = table.Column<double>(type: "REAL", nullable: true),
                    Cst = table.Column<string>(type: "TEXT", nullable: true),
                    Csosn = table.Column<string>(type: "TEXT", nullable: true),
                    TotalizadorParcial = table.Column<string>(type: "TEXT", nullable: true),
                    EcfIcmsSt = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoBalanca = table.Column<int>(type: "INTEGER", nullable: true),
                    PafPSt = table.Column<string>(type: "TEXT", nullable: true),
                    HashRegistro = table.Column<string>(type: "TEXT", nullable: true),
                    ValorCusto = table.Column<double>(type: "REAL", nullable: true),
                    Situacao = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoCest = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_FICHA_TECNICA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    IdProdutoFilho = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorCusto = table.Column<double>(type: "REAL", nullable: true),
                    PercentualCusto = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_FICHA_TECNICA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_GRUPO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_GRUPO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_IMAGEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    Imagem = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_IMAGEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_PROMOCAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: true),
                    QuantidadeEmPromocao = table.Column<double>(type: "REAL", nullable: true),
                    QuantidadeMaximaCliente = table.Column<double>(type: "REAL", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_PROMOCAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_SUBGRUPO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProdutoGrupo = table.Column<int>(type: "INTEGER", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_SUBGRUPO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_TIPO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_TIPO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO_UNIDADE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sigla = table.Column<string>(type: "TEXT", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    PodeFracionar = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO_UNIDADE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RESERVA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: true),
                    NomeContato = table.Column<string>(type: "TEXT", nullable: true),
                    TelefoneContato = table.Column<string>(type: "TEXT", nullable: true),
                    DataReserva = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoraReserva = table.Column<string>(type: "TEXT", nullable: true),
                    QuantidadePessoas = table.Column<int>(type: "INTEGER", nullable: true),
                    Situacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RESERVA_MESA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdMesa = table.Column<int>(type: "INTEGER", nullable: true),
                    IdReserva = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESERVA_MESA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TAXA_ENTREGA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Valor = table.Column<double>(type: "REAL", nullable: true),
                    EstimativaMinutos = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAXA_ENTREGA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_COFINS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributConfiguraOfGt = table.Column<int>(type: "INTEGER", nullable: true),
                    CstCofins = table.Column<string>(type: "TEXT", nullable: true),
                    EfdTabela435 = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBaseCalculo = table.Column<string>(type: "TEXT", nullable: true),
                    PorcentoBaseCalculo = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPorcento = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaUnidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaFiscal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_COFINS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_CONFIGURA_OF_GT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributGrupoTributario = table.Column<int>(type: "INTEGER", nullable: true),
                    IdTributOperacaoFiscal = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_CONFIGURA_OF_GT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_GRUPO_TRIBUTARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    OrigemMercadoria = table.Column<string>(type: "TEXT", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_GRUPO_TRIBUTARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_ICMS_CUSTOM_CAB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    OrigemMercadoria = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_ICMS_CUSTOM_CAB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_ICMS_CUSTOM_DET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributIcmsCustomCab = table.Column<int>(type: "INTEGER", nullable: true),
                    UfDestino = table.Column<string>(type: "TEXT", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Csosn = table.Column<string>(type: "TEXT", nullable: true),
                    Cst = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBc = table.Column<string>(type: "TEXT", nullable: true),
                    Aliquota = table.Column<double>(type: "REAL", nullable: true),
                    ValorPauta = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    Mva = table.Column<double>(type: "REAL", nullable: true),
                    PorcentoBc = table.Column<double>(type: "REAL", nullable: true),
                    ModalidadeBcSt = table.Column<string>(type: "TEXT", nullable: true),
                    AliquotaInternaSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaInterestadualSt = table.Column<double>(type: "REAL", nullable: true),
                    PorcentoBcSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximoSt = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_ICMS_CUSTOM_DET", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_ICMS_UF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributConfiguraOfGt = table.Column<int>(type: "INTEGER", nullable: true),
                    UfDestino = table.Column<string>(type: "TEXT", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Csosn = table.Column<string>(type: "TEXT", nullable: true),
                    Cst = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBc = table.Column<string>(type: "TEXT", nullable: true),
                    Aliquota = table.Column<double>(type: "REAL", nullable: true),
                    ValorPauta = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    Mva = table.Column<double>(type: "REAL", nullable: true),
                    PorcentoBc = table.Column<double>(type: "REAL", nullable: true),
                    ModalidadeBcSt = table.Column<string>(type: "TEXT", nullable: true),
                    AliquotaInternaSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaInterestadualSt = table.Column<double>(type: "REAL", nullable: true),
                    PorcentoBcSt = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaIcmsSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaSt = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximoSt = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_ICMS_UF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_IPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributConfiguraOfGt = table.Column<int>(type: "INTEGER", nullable: true),
                    CstIpi = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBaseCalculo = table.Column<string>(type: "TEXT", nullable: true),
                    PorcentoBaseCalculo = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPorcento = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaUnidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaFiscal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_IPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_ISS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributOperacaoFiscal = table.Column<int>(type: "INTEGER", nullable: true),
                    ModalidadeBaseCalculo = table.Column<string>(type: "TEXT", nullable: true),
                    PorcentoBaseCalculo = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPorcento = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaUnidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaFiscal = table.Column<double>(type: "REAL", nullable: true),
                    ItemListaServico = table.Column<int>(type: "INTEGER", nullable: true),
                    CodigoTributacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_ISS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_OPERACAO_FISCAL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    DescricaoNaNf = table.Column<string>(type: "TEXT", nullable: true),
                    Cfop = table.Column<int>(type: "INTEGER", nullable: true),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_OPERACAO_FISCAL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRIBUT_PIS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTributConfiguraOfGt = table.Column<int>(type: "INTEGER", nullable: true),
                    CstPis = table.Column<string>(type: "TEXT", nullable: true),
                    EfdTabela435 = table.Column<string>(type: "TEXT", nullable: true),
                    ModalidadeBaseCalculo = table.Column<string>(type: "TEXT", nullable: true),
                    PorcentoBaseCalculo = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaPorcento = table.Column<double>(type: "REAL", nullable: true),
                    AliquotaUnidade = table.Column<double>(type: "REAL", nullable: true),
                    ValorPrecoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    ValorPautaFiscal = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRIBUT_PIS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID_COLABORADOR = table.Column<int>(type: "INTEGER", nullable: false),
                    ID_PAPEL = table.Column<int>(type: "INTEGER", nullable: false),
                    LOGIN = table.Column<string>(type: "TEXT", nullable: false),
                    SENHA = table.Column<string>(type: "TEXT", nullable: false),
                    ADMINISTRADOR = table.Column<string>(type: "TEXT", nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USUARIO_COLABORADOR_ID_COLABORADOR",
                        column: x => x.ID_COLABORADOR,
                        principalTable: "COLABORADOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ID_COLABORADOR",
                table: "USUARIO",
                column: "ID_COLABORADOR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CARDAPIO");

            migrationBuilder.DropTable(
                name: "CARDAPIO_PERGUNTA_PADRAO");

            migrationBuilder.DropTable(
                name: "CARDAPIO_RESPOSTA_PADRAO");

            migrationBuilder.DropTable(
                name: "CFOP");

            migrationBuilder.DropTable(
                name: "CLIENTE");

            migrationBuilder.DropTable(
                name: "CLIENTE_FIADO");

            migrationBuilder.DropTable(
                name: "COMANDA");

            migrationBuilder.DropTable(
                name: "COMANDA_DETALHE");

            migrationBuilder.DropTable(
                name: "COMANDA_DETALHE_COMPLEMENTO");

            migrationBuilder.DropTable(
                name: "COMANDA_OBSERVACAO_PADRAO");

            migrationBuilder.DropTable(
                name: "COMANDA_PEDIDO");

            migrationBuilder.DropTable(
                name: "COMPRA_PEDIDO_CABECALHO");

            migrationBuilder.DropTable(
                name: "COMPRA_PEDIDO_DETALHE");

            migrationBuilder.DropTable(
                name: "CONTADOR");

            migrationBuilder.DropTable(
                name: "CONTAS_PAGAR");

            migrationBuilder.DropTable(
                name: "CONTAS_RECEBER");

            migrationBuilder.DropTable(
                name: "COZINHA");

            migrationBuilder.DropTable(
                name: "DELIVERY");

            migrationBuilder.DropTable(
                name: "DELIVERY_ACERTO");

            migrationBuilder.DropTable(
                name: "DELIVERY_ACERTO_COMANDA");

            migrationBuilder.DropTable(
                name: "ECF_ALIQUOTAS");

            migrationBuilder.DropTable(
                name: "ECF_DOCUMENTOS_EMITIDOS");

            migrationBuilder.DropTable(
                name: "ECF_E3");

            migrationBuilder.DropTable(
                name: "ECF_IMPRESSORA");

            migrationBuilder.DropTable(
                name: "ECF_LOG_TOTAIS");

            migrationBuilder.DropTable(
                name: "ECF_R01");

            migrationBuilder.DropTable(
                name: "ECF_R02");

            migrationBuilder.DropTable(
                name: "ECF_R03");

            migrationBuilder.DropTable(
                name: "ECF_R06");

            migrationBuilder.DropTable(
                name: "ECF_R07");

            migrationBuilder.DropTable(
                name: "ECF_RECEBIMENTO_NAO_FISCAL");

            migrationBuilder.DropTable(
                name: "ECF_RELATORIO_GERENCIAL");

            migrationBuilder.DropTable(
                name: "ECF_SINTEGRA_60A");

            migrationBuilder.DropTable(
                name: "ECF_SINTEGRA_60M");

            migrationBuilder.DropTable(
                name: "EMPRESA");

            migrationBuilder.DropTable(
                name: "EMPRESA_CNAE");

            migrationBuilder.DropTable(
                name: "EMPRESA_DELIVERY_PEDIDO");

            migrationBuilder.DropTable(
                name: "EMPRESA_SEGMENTO");

            migrationBuilder.DropTable(
                name: "ENTREGADOR_ROTA");

            migrationBuilder.DropTable(
                name: "ENTREGADOR_ROTA_DETALHE");

            migrationBuilder.DropTable(
                name: "FIDELIDADE_HISTORICO");

            migrationBuilder.DropTable(
                name: "FIDELIDADE_UTILIZADO");

            migrationBuilder.DropTable(
                name: "FORNECEDOR");

            migrationBuilder.DropTable(
                name: "IBPT");

            migrationBuilder.DropTable(
                name: "LOG_IMPORTACAO");

            migrationBuilder.DropTable(
                name: "MESA");

            migrationBuilder.DropTable(
                name: "NFCE_PLANO_PAGAMENTO");

            migrationBuilder.DropTable(
                name: "NFE_ACESSO_XML");

            migrationBuilder.DropTable(
                name: "NFE_CABECALHO");

            migrationBuilder.DropTable(
                name: "NFE_CANA");

            migrationBuilder.DropTable(
                name: "NFE_CANA_DEDUCOES_SAFRA");

            migrationBuilder.DropTable(
                name: "NFE_CANA_FORNECIMENTO_DIARIO");

            migrationBuilder.DropTable(
                name: "NFE_CONFIGURACAO");

            migrationBuilder.DropTable(
                name: "NFE_CTE_REFERENCIADO");

            migrationBuilder.DropTable(
                name: "NFE_CUPOM_FISCAL_REFERENCIADO");

            migrationBuilder.DropTable(
                name: "NFE_DECLARACAO_IMPORTACAO");

            migrationBuilder.DropTable(
                name: "NFE_DESTINATARIO");

            migrationBuilder.DropTable(
                name: "NFE_DET_ESPECIFICO_ARMAMENTO");

            migrationBuilder.DropTable(
                name: "NFE_DET_ESPECIFICO_COMBUSTIVEL");

            migrationBuilder.DropTable(
                name: "NFE_DET_ESPECIFICO_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "NFE_DET_ESPECIFICO_VEICULO");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_COFINS");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_COFINS_ST");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_ICMS");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_ICMS_UFDEST");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_II");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_IPI");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_ISSQN");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_PIS");

            migrationBuilder.DropTable(
                name: "NFE_DETALHE_IMPOSTO_PIS_ST");

            migrationBuilder.DropTable(
                name: "NFE_DUPLICATA");

            migrationBuilder.DropTable(
                name: "NFE_EMITENTE");

            migrationBuilder.DropTable(
                name: "NFE_EXPORTACAO");

            migrationBuilder.DropTable(
                name: "NFE_FATURA");

            migrationBuilder.DropTable(
                name: "NFE_IMPORTACAO_DETALHE");

            migrationBuilder.DropTable(
                name: "NFE_INFORMACAO_PAGAMENTO");

            migrationBuilder.DropTable(
                name: "NFE_ITEM_RASTREADO");

            migrationBuilder.DropTable(
                name: "NFE_LOCAL_ENTREGA");

            migrationBuilder.DropTable(
                name: "NFE_LOCAL_RETIRADA");

            migrationBuilder.DropTable(
                name: "NFE_NF_REFERENCIADA");

            migrationBuilder.DropTable(
                name: "NFE_NUMERO");

            migrationBuilder.DropTable(
                name: "NFE_NUMERO_INUTILIZADO");

            migrationBuilder.DropTable(
                name: "NFE_PROCESSO_REFERENCIADO");

            migrationBuilder.DropTable(
                name: "NFE_PROD_RURAL_REFERENCIADA");

            migrationBuilder.DropTable(
                name: "NFE_REFERENCIADA");

            migrationBuilder.DropTable(
                name: "NFE_RESPONSAVEL_TECNICO");

            migrationBuilder.DropTable(
                name: "NFE_TRANSPORTE");

            migrationBuilder.DropTable(
                name: "NFE_TRANSPORTE_REBOQUE");

            migrationBuilder.DropTable(
                name: "NFE_TRANSPORTE_VOLUME");

            migrationBuilder.DropTable(
                name: "NFE_TRANSPORTE_VOLUME_LACRE");

            migrationBuilder.DropTable(
                name: "PDV_CAIXA");

            migrationBuilder.DropTable(
                name: "PDV_CONFIGURACAO");

            migrationBuilder.DropTable(
                name: "PDV_CONFIGURACAO_BALANCA");

            migrationBuilder.DropTable(
                name: "PDV_CONFIGURACAO_LEITOR_SERIAL");

            migrationBuilder.DropTable(
                name: "PDV_FECHAMENTO");

            migrationBuilder.DropTable(
                name: "PDV_MOVIMENTO");

            migrationBuilder.DropTable(
                name: "PDV_OPERADOR");

            migrationBuilder.DropTable(
                name: "PDV_SANGRIA");

            migrationBuilder.DropTable(
                name: "PDV_SUPRIMENTO");

            migrationBuilder.DropTable(
                name: "PDV_TIPO_PAGAMENTO");

            migrationBuilder.DropTable(
                name: "PDV_TOTAL_TIPO_PAGAMENTO");

            migrationBuilder.DropTable(
                name: "PDV_VENDA_CABECALHO");

            migrationBuilder.DropTable(
                name: "PDV_VENDA_DETALHE");

            migrationBuilder.DropTable(
                name: "PRODUTO");

            migrationBuilder.DropTable(
                name: "PRODUTO_FICHA_TECNICA");

            migrationBuilder.DropTable(
                name: "PRODUTO_GRUPO");

            migrationBuilder.DropTable(
                name: "PRODUTO_IMAGEM");

            migrationBuilder.DropTable(
                name: "PRODUTO_PROMOCAO");

            migrationBuilder.DropTable(
                name: "PRODUTO_SUBGRUPO");

            migrationBuilder.DropTable(
                name: "PRODUTO_TIPO");

            migrationBuilder.DropTable(
                name: "PRODUTO_UNIDADE");

            migrationBuilder.DropTable(
                name: "RESERVA");

            migrationBuilder.DropTable(
                name: "RESERVA_MESA");

            migrationBuilder.DropTable(
                name: "TAXA_ENTREGA");

            migrationBuilder.DropTable(
                name: "TRIBUT_COFINS");

            migrationBuilder.DropTable(
                name: "TRIBUT_CONFIGURA_OF_GT");

            migrationBuilder.DropTable(
                name: "TRIBUT_GRUPO_TRIBUTARIO");

            migrationBuilder.DropTable(
                name: "TRIBUT_ICMS_CUSTOM_CAB");

            migrationBuilder.DropTable(
                name: "TRIBUT_ICMS_CUSTOM_DET");

            migrationBuilder.DropTable(
                name: "TRIBUT_ICMS_UF");

            migrationBuilder.DropTable(
                name: "TRIBUT_IPI");

            migrationBuilder.DropTable(
                name: "TRIBUT_ISS");

            migrationBuilder.DropTable(
                name: "TRIBUT_OPERACAO_FISCAL");

            migrationBuilder.DropTable(
                name: "TRIBUT_PIS");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "COLABORADOR");
        }
    }
}
