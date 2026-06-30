using PDV.Services.Interfaces;
﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PDV.Services;

#nullable disable

namespace PDV.Migrations
{
    [DbContext(typeof(PdvContext))]
    partial class PdvContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("PDV.Models.Pdv.Cardapio", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InfoAlergico")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ingredientes")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModoPreparo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CARDAPIO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.CardapioPerguntaPadrao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdCardapio")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Pergunta")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CARDAPIO_PERGUNTA_PADRAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.CardapioRespostaPadrao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdCardapioPerguntaPadrao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Resposta")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CARDAPIO_RESPOSTA_PADRAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Cfop", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aplicacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Codigo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CFOP");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Cliente", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoIbgeCidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoIbgeUf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contato")
                        .HasColumnType("TEXT");

                    b.Property<string>("CpfCnpj")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataCadastro")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataEmissaoRg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fantasia")
                        .HasColumnType("TEXT");

                    b.Property<double?>("FiadoValorTeto")
                        .HasColumnType("REAL");

                    b.Property<string>("FidelidadeAviso")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FidelidadeQuantidade")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("FidelidadeValor")
                        .HasColumnType("REAL");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgaoRg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sexo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoPessoa")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CLIENTE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ClienteFiado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataLancamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPagamento")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorPendente")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("CLIENTE_FIADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Colaborador", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ComissaoPrazo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ComissaoVista")
                        .HasColumnType("REAL");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EntregadorVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<string>("NivelAutorizacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("COLABORADOR");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Comanda", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoCompartilhado")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataChegada")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataSaida")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraChegada")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraSaida")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEmpresaDeliveryPedido")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdMesa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuantidadePessoas")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Situacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPorPessoa")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSubtotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("COMANDA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ComandaDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GerouPedidoCozinha")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdComanda")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalComplemento")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitario")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("COMANDA_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ComandaDetalheComplemento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdComandaDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NomeProduto")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitario")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("COMANDA_DETALHE_COMPLEMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ComandaObservacaoPadrao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("COMANDA_OBSERVACAO_PADRAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ComandaPedido", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EntrouNaFila")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EstimativaMinutos")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("FimPreparo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdComanda")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdCozinha")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("InicioPreparo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Posicao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Prioridade")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SaiuDaFila")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("COMANDA_PEDIDO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.CompraPedidoCabecalho", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AtualizouEstoque")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contato")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPedido")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPrevisaoEntrega")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPrevisaoPagamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataRecebimentoItens")
                        .HasColumnType("TEXT");

                    b.Property<string>("DiaFixoParcela")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DiaPrimeiroVencimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("FormaPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("GeraFinanceiro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraRecebimentoItens")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdFornecedor")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IntervaloEntreParcelas")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocalCobranca")
                        .HasColumnType("TEXT");

                    b.Property<string>("LocalEntrega")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroDocumentoEntrada")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuantidadeParcelas")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSubtotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("COMPRA_PEDIDO_CABECALHO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.CompraPedidoDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Csosn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cst")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCompraPedidoCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSubtotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitario")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("COMPRA_PEDIDO_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Contador", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoIbgeCidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoIbgeUf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoCrc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CONTADOR");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ContasPagar", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataLancamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPagamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataVencimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Historico")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCompraPedidoCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdFornecedor")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroDocumento")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusPagamento")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaJuro")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaMulta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorAPagar")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorJuro")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorMulta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPago")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("CONTAS_PAGAR");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ContasReceber", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataLancamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataRecebimento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataVencimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Historico")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroDocumento")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusRecebimento")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaJuro")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaMulta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorAReceber")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorJuro")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorMulta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRecebido")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("CONTAS_RECEBER");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Cozinha", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImpressoraEndereco")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImpressoraNome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("COZINHA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Delivery", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Entregue")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdComanda")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTaxaEntrega")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("InicioPreparo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeCliente")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PrevisaoEntrega")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PrevisaoPreparo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PrevisaoRetirada")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProntoParaRetirada")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Retirou")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SaiuParaEntrega")
                        .HasColumnType("TEXT");

                    b.Property<string>("TelefonePrincipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("TelefoneRecado")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorAReceber")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFrete")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRecebido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSolicitadoTroco")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("DELIVERY");
                });

            modelBuilder.Entity("PDV.Models.Pdv.DeliveryAcerto", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataAcerto")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraAcerto")
                        .HasColumnType("TEXT");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorPagoEntregador")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRecebido")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("DELIVERY_ACERTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.DeliveryAcertoComanda", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdDelivery")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdDeliveryAcerto")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("DELIVERY_ACERTO_COMANDA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfAliquotas", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EcfIcmsSt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PafPSt")
                        .HasColumnType("TEXT");

                    b.Property<string>("TotalizadorParcial")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_ALIQUOTAS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfDocumentosEmitidos", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraEmissao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_DOCUMENTOS_EMITIDOS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfE3", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEstoque")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraEstoque")
                        .HasColumnType("TEXT");

                    b.Property<string>("MarcaEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("MfAdicional")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModeloEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoEcf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_E3");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfImpressora", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataInstalacaoSb")
                        .HasColumnType("TEXT");

                    b.Property<string>("Docto")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraInstalacaoSb")
                        .HasColumnType("TEXT");

                    b.Property<string>("Identificacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("LacreNaMfd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Le")
                        .HasColumnType("TEXT");

                    b.Property<string>("Lef")
                        .HasColumnType("TEXT");

                    b.Property<string>("Marca")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Md")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mfd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Modelo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModeloAcbr")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModeloDocumentoFiscal")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Versao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vr")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_IMPRESSORA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfLogTotais", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Produto")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R01")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R02")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R03")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R04")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R05")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R06")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("R07")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TipoPagamento")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ECF_LOG_TOTAIS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfR01", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BairroSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("CepSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("CidadeSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("CnpjEmpresa")
                        .HasColumnType("TEXT");

                    b.Property<string>("CnpjSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("ComplementoSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContatoSh")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataFinal")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataInicial")
                        .HasColumnType("TEXT");

                    b.Property<string>("DenominacaoSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnderecoSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoEstadualSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipalSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("Md5PafEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomePafEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroLaudoPaf")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("PrincipalExecutavel")
                        .HasColumnType("TEXT");

                    b.Property<string>("RazaoSocialSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("TelefoneSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("UfSh")
                        .HasColumnType("TEXT");

                    b.Property<string>("VersaoEr")
                        .HasColumnType("TEXT");

                    b.Property<string>("VersaoPafEcf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_R01");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfR02", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cro")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Crz")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEmissao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataMovimento")
                        .HasColumnType("TEXT");

                    b.Property<double?>("GrandeTotal")
                        .HasColumnType("REAL");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraEmissao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdEcfCaixa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfImpressora")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvOperador")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<double?>("VendaBruta")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_R02");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfR03", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Crz")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdEcfR02")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("TotalizadorParcial")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorAcumulado")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_R03");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfR06", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cdc")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Denominacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Gnf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Grg")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraEmissao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdEcfCaixa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfImpressora")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvOperador")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ECF_R06");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfR07", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ccf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Estorno")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdEcfR06")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MeioPagamento")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorEstorno")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPagamento")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_R07");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfRecebimentoNaoFiscal", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataRecebimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_RECEBIMENTO_NAO_FISCAL");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfRelatorioGerencial", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DavEmitidos")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvConfiguracao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdentificacaoPaf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MeiosPagamento")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Outros")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ParametrosConfiguracao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("X")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ECF_RELATORIO_GERENCIAL");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfSintegra60A", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfSintegra60M")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SituacaoTributaria")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_SINTEGRA_60A");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EcfSintegra60M", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CooFinal")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CooInicial")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cro")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Crz")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModeloDocumentoFiscal")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumeroEquipamento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroSerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorGrandeTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorVendaBruta")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ECF_SINTEGRA_60M");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Empresa", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaPis")
                        .HasColumnType("REAL");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoIbgeCidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoIbgeUf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contato")
                        .HasColumnType("TEXT");

                    b.Property<string>("Crt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataConstituicao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fone")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logotipo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("NaturezaJuridica")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeFantasia")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("RazaoSocial")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Registrado")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Simei")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoRegime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EMPRESA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EmpresaCnae", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Principal")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EMPRESA_CNAE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EmpresaDeliveryPedido", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoPedidoEmpresa")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConteudoJson")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataSolicitacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraSolicitacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EMPRESA_DELIVERY_PEDIDO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EmpresaSegmento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Denominacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Divisoes")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EMPRESA_SEGMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EntregadorRota", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataRota")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EstimativaMinutos")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HoraPrevistoRetorno")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraSaida")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ENTREGADOR_ROTA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.EntregadorRotaDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdDelivery")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEntregadorRota")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Latitude")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Longitude")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PosicaoNaFila")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ENTREGADOR_ROTA_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.FidelidadeHistorico", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataConsumo")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraConsumo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdFidelidadeUtilizado")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorConsumo")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("FIDELIDADE_HISTORICO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.FidelidadeUtilizado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataUtilizacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraUtilizacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorUtilizado")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("FIDELIDADE_UTILIZADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Fornecedor", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Celular")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoIbgeCidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoIbgeUf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contato")
                        .HasColumnType("TEXT");

                    b.Property<string>("CpfCnpj")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataCadastro")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataEmissaoRg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fantasia")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgaoRg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rg")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sexo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoPessoa")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FORNECEDOR");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Ibpt", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Chave")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Estadual")
                        .HasColumnType("REAL");

                    b.Property<string>("Ex")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fonte")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ImportadosFederal")
                        .HasColumnType("REAL");

                    b.Property<double?>("Municipal")
                        .HasColumnType("REAL");

                    b.Property<double?>("NacionalFederal")
                        .HasColumnType("REAL");

                    b.Property<string>("Ncm")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Versao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VigenciaFim")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VigenciaInicio")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("IBPT");
                });

            modelBuilder.Entity("PDV.Models.Pdv.LogImportacao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataImportacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Erro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraImportacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Registro")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LOG_IMPORTACAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Mesa", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Disponivel")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuantidadeCadeiras")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuantidadeCadeirasCrianca")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MESA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfcePlanoPagamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoTipoPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoTransacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPagamento")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPlanoExpira")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataSolicitacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("MetodoPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoPlano")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFCE_PLANO_PAGAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeAcessoXml", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NFE_ACESSO_XML");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCabecalho", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ambiente")
                        .HasColumnType("TEXT");

                    b.Property<double?>("BaseCalculoIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoIrrf")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoPrevidencia")
                        .HasColumnType("REAL");

                    b.Property<string>("ChaveAcesso")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoModelo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoMunicipio")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoNumerico")
                        .HasColumnType("TEXT");

                    b.Property<string>("ComexLocalDespacho")
                        .HasColumnType("TEXT");

                    b.Property<string>("ComexLocalEmbarque")
                        .HasColumnType("TEXT");

                    b.Property<string>("ComexUfEmbarque")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompraContrato")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompraNotaEmpenho")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompraPedido")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsumidorOperacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsumidorPresenca")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataEntradaContingencia")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataHoraEmissao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataHoraEntradaSaida")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataPrestacaoServico")
                        .HasColumnType("TEXT");

                    b.Property<double?>("DescontoCondicionadoIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("DescontoIncondicionadoIssqn")
                        .HasColumnType("REAL");

                    b.Property<string>("DigitoChaveAcesso")
                        .HasColumnType("TEXT");

                    b.Property<string>("FinalidadeEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("FormatoImpressaoDanfe")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTributOperacaoFiscal")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InformacoesAddContribuinte")
                        .HasColumnType("TEXT");

                    b.Property<string>("InformacoesAddFisco")
                        .HasColumnType("TEXT");

                    b.Property<string>("JustificativaContingencia")
                        .HasColumnType("TEXT");

                    b.Property<string>("LocalDestino")
                        .HasColumnType("TEXT");

                    b.Property<string>("NaturezaOperacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<double?>("OutrasRetencoesIssqn")
                        .HasColumnType("REAL");

                    b.Property<string>("ProcessoEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Qrcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("RegimeEspecialTributacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusNota")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoEmissao")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoOperacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TotalIcmsFcpUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalIcmsInterestadualUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalIcmsInterestadualUfRemetente")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalRetencaoIssqn")
                        .HasColumnType("REAL");

                    b.Property<int?>("UfEmitente")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UrlChave")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCofinsIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDeducaoIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDespesasAcessorias")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFrete")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsDesonerado")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorImpostoImportacao")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIpi")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIpiDevolvido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPis")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPisIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetidoCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetidoCsll")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetidoIrrf")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetidoPis")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetidoPrevidencia")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSeguro")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorServicos")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalFcp")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalFcpSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalFcpStRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalProdutos")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalTributos")
                        .HasColumnType("REAL");

                    b.Property<string>("VersaoProcessoEmissao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_CABECALHO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCana", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MesAnoReferencia")
                        .HasColumnType("TEXT");

                    b.Property<string>("Safra")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_CANA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCanaDeducoesSafra", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Decricao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCana")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorDeducao")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFornecimento")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorLiquidoFornecimento")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalDeducao")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_CANA_DEDUCOES_SAFRA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCanaFornecimentoDiario", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Dia")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCana")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeTotalAnterior")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeTotalGeral")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeTotalMes")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_CANA_FORNECIMENTO_DIARIO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeConfiguracao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CaminhoArquivoDanfe")
                        .HasColumnType("TEXT");

                    b.Property<string>("CaminhoLogomarca")
                        .HasColumnType("TEXT");

                    b.Property<string>("CaminhoSalvarPdf")
                        .HasColumnType("TEXT");

                    b.Property<string>("CaminhoSalvarXml")
                        .HasColumnType("TEXT");

                    b.Property<string>("CaminhoSchemas")
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificadoDigitalCaminho")
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificadoDigitalSenha")
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificadoDigitalSerie")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailAssunto")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailAutenticaSsl")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EmailPorta")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailSenha")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailServidorSmtp")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailTexto")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailUsuario")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FormatoImpressaoDanfe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NfceCsc")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceIdCsc")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImpressaoTributos")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImprimirDescontoPorItem")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImprimirGtin")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImprimirItensUmaLinha")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImprimirNomeFantasia")
                        .HasColumnType("TEXT");

                    b.Property<string>("NfceImprimirQrcodeLateral")
                        .HasColumnType("TEXT");

                    b.Property<double?>("NfceMargemDireita")
                        .HasColumnType("REAL");

                    b.Property<double?>("NfceMargemEsquerda")
                        .HasColumnType("REAL");

                    b.Property<double?>("NfceMargemInferior")
                        .HasColumnType("REAL");

                    b.Property<double?>("NfceMargemSuperior")
                        .HasColumnType("REAL");

                    b.Property<string>("NfceModeloImpressao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NfceResolucaoImpressao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NfceTamanhoFonteItem")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProcessoEmissao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RespTecCnpj")
                        .HasColumnType("TEXT");

                    b.Property<string>("RespTecContato")
                        .HasColumnType("TEXT");

                    b.Property<string>("RespTecEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("RespTecFone")
                        .HasColumnType("TEXT");

                    b.Property<string>("RespTecHashCsrt")
                        .HasColumnType("TEXT");

                    b.Property<string>("RespTecIdCsrt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SalvarXml")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TipoEmissao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VersaoProcessoEmissao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("WebserviceAmbiente")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebserviceProxyHost")
                        .HasColumnType("TEXT");

                    b.Property<int?>("WebserviceProxyPorta")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebserviceProxySenha")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebserviceProxyUsuario")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebserviceUf")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebserviceVisualizar")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_CONFIGURACAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCteReferenciado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChaveAcesso")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NFE_CTE_REFERENCIADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeCupomFiscalReferenciado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataEmissaoCupom")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModeloDocumentoFiscal")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumeroCaixa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NumeroOrdemEcf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroSerieEcf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_CUPOM_FISCAL_REFERENCIADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDeclaracaoImportacao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoExportador")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataDesembaraco")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("FormaIntermediacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocalDesembaraco")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroDocumento")
                        .HasColumnType("TEXT");

                    b.Property<string>("UfDesembaraco")
                        .HasColumnType("TEXT");

                    b.Property<string>("UfTerceiro")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorAfrmm")
                        .HasColumnType("REAL");

                    b.Property<string>("ViaTransporte")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_DECLARACAO_IMPORTACAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDestinatario", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoMunicipio")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoPais")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EstrangeiroIdentificacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndicadorIe")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeMunicipio")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomePais")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Suframa")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_DESTINATARIO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetEspecificoArmamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroSerieArma")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroSerieCano")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoArma")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_DET_ESPECIFICO_ARMAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetEspecificoCombustivel", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("CideAliquota")
                        .HasColumnType("REAL");

                    b.Property<double?>("CideBaseCalculo")
                        .HasColumnType("REAL");

                    b.Property<double?>("CideValor")
                        .HasColumnType("REAL");

                    b.Property<string>("Codif")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoAnp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DescricaoAnp")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EncerranteBico")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EncerranteBomba")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EncerranteTanque")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("EncerranteValorFim")
                        .HasColumnType("REAL");

                    b.Property<double?>("EncerranteValorInicio")
                        .HasColumnType("REAL");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("PercentualGasImportado")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualGasNacional")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualGlp")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeTempAmbiente")
                        .HasColumnType("REAL");

                    b.Property<string>("UfConsumo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorPartida")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DET_ESPECIFICO_COMBUSTIVEL");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetEspecificoMedicamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoAnvisa")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MotivoIsencao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PrecoMaximoConsumidor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DET_ESPECIFICO_MEDICAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetEspecificoVeiculo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnoFabricacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("AnoModelo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CapacidadeMaximaTracao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Chassi")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cilindradas")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoCorDenatran")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoMarcaModelo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CondicaoVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CondicaoVin")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cor")
                        .HasColumnType("TEXT");

                    b.Property<string>("DescricaoCor")
                        .HasColumnType("TEXT");

                    b.Property<string>("DistanciaEixos")
                        .HasColumnType("TEXT");

                    b.Property<string>("EspecieVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LotacaoMaxima")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroMotor")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroSerie")
                        .HasColumnType("TEXT");

                    b.Property<string>("PesoBruto")
                        .HasColumnType("TEXT");

                    b.Property<string>("PesoLiquido")
                        .HasColumnType("TEXT");

                    b.Property<string>("PotenciaMotor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Restricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoCombustivel")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoOperacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoPintura")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoVeiculo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_DET_ESPECIFICO_VEICULO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cest")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CnpjFabricante")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoBeneficioFiscal")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoProduto")
                        .HasColumnType("TEXT");

                    b.Property<string>("EntraTotal")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ExTipi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gtin")
                        .HasColumnType("TEXT");

                    b.Property<string>("GtinUnidadeTributavel")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndicadorEscalaRelevante")
                        .HasColumnType("TEXT");

                    b.Property<string>("InformacoesAdicionais")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ItemPedidoCompra")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ncm")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeProduto")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroFci")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumeroItem")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroPedidoCompra")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroRecopi")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nve")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PercentualDevolvido")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeComercial")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeTributavel")
                        .HasColumnType("REAL");

                    b.Property<string>("UnidadeComercial")
                        .HasColumnType("TEXT");

                    b.Property<string>("UnidadeTributavel")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorBrutoProduto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFrete")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIpiDevolvido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorOutrasDespesas")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSeguro")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorSubtotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalTributos")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitarioComercial")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitarioTributavel")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoCofins", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaCofinsPercentual")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaCofinsReais")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoCofins")
                        .HasColumnType("REAL");

                    b.Property<string>("CstCofins")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("QuantidadeVendida")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCofins")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_COFINS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoCofinstSt", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("PcofinsAliqSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("VBcCofinsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("VCofinsSt")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_COFINS_ST");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoIcms", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaCreditoIcmsSn")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaIcmsEfetivo")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaSuportadaConsumidor")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoFcp")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoFcpSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoFcpStRetido")
                        .HasColumnType("REAL");

                    b.Property<string>("Csosn")
                        .HasColumnType("TEXT");

                    b.Property<string>("CstIcms")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBcIcms")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModalidadeBcIcmsSt")
                        .HasColumnType("TEXT");

                    b.Property<string>("MotivoDesoneracaoIcms")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrigemMercadoria")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PercentualBcOperacaoPropria")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualDiferimento")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualFcp")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualFcpSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualFcpStRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualMvaIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualReducaoBcEfetivo")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualReducaoBcIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualReducaoBcIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<string>("UfSt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorBaseCalculoIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcEfetivo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcIcmsStDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcIcmsStRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCreditoIcmsSn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFcp")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFcpSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFcpStRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsDesonerado")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsDiferido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsEfetivo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsOperacao")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsStDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsStRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsSubstituto")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_ICMS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoIcmsUfdest", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaInteresdatualUfEnvolvidas")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaInternaUfDestino")
                        .HasColumnType("REAL");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("PercentualFcpUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("PercentualProvisorioPartilhaIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcFcpUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBcIcmsUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsFcpUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorInterestadualUfDestino")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorInterestadualUfRemetente")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_ICMS_UFDEST");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoIi", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorBcIi")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDespesasAduaneiras")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorImpostoImportacao")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIof")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_II");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoIpi", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaIpi")
                        .HasColumnType("REAL");

                    b.Property<string>("CnpjProdutor")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoSeloIpi")
                        .HasColumnType("TEXT");

                    b.Property<string>("CstIpi")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnquadramentoLegalIpi")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QuantidadeSeloIpi")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("QuantidadeUnidadeTributavel")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBaseCalculoIpi")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIpi")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnidadeTributavel")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_IPI");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoIssqn", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("BaseCalculoIssqn")
                        .HasColumnType("REAL");

                    b.Property<string>("CodigoServico")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndicadorExigibilidadeIss")
                        .HasColumnType("TEXT");

                    b.Property<string>("IndicadorIncentivoFiscal")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ItemListaServicos")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MunicipioIncidencia")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MunicipioIssqn")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroProcesso")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaisSevicoPrestado")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorDeducao")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDescontoCondicionado")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDescontoIncondicionado")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorOutrasRetencoes")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRetencaoIss")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_ISSQN");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoPis", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPisPercentual")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaPisReais")
                        .HasColumnType("REAL");

                    b.Property<string>("CstPis")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("QuantidadeVendida")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBaseCalculoPis")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPis")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_PIS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDetalheImpostoPisSt", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPisStPercentual")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaPisStReais")
                        .HasColumnType("REAL");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("QuantidadeVendidaPisSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBaseCalculoPisSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPisSt")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DETALHE_IMPOSTO_PIS_ST");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeDuplicata", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataVencimento")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeFatura")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_DUPLICATA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeEmitente", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnae")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoMunicipio")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoPais")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Crt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fantasia")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoEstadualSt")
                        .HasColumnType("TEXT");

                    b.Property<string>("InscricaoMunicipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeMunicipio")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomePais")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_EMITENTE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeExportacao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChaveAcesso")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Drawback")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NumeroRegistro")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_EXPORTACAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeFatura", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorLiquido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorOriginal")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_FATURA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeImportacaoDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoFabricanteEstrangeiro")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Drawback")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeDeclaracaoImportacao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NumeroAdicao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NumeroSequencial")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_IMPORTACAO_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeInformacaoPagamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bandeira")
                        .HasColumnType("TEXT");

                    b.Property<string>("CnpjOperadoraCartao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndicadorPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("MeioPagamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("NumeroAutorizacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("TipoIntegracao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Troco")
                        .HasColumnType("REAL");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_INFORMACAO_PAGAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeItemRastreado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoAgregacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataFabricacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataValidade")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeDetalhe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumeroLote")
                        .HasColumnType("TEXT");

                    b.Property<double?>("QuantidadeItens")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_ITEM_RASTREADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeLocalEntrega", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoMunicipio")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoPais")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeMunicipio")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomePais")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeRecebedor")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_LOCAL_ENTREGA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeLocalRetirada", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoMunicipio")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoPais")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeExpedidor")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeMunicipio")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomePais")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_LOCAL_RETIRADA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeNfReferenciada", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnoMes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoUf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Modelo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumeroNf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_NF_REFERENCIADA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeNumero", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Modelo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_NUMERO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeNumeroInutilizado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataInutilizacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Numero")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_NUMERO_INUTILIZADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeProcessoReferenciado", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Identificador")
                        .HasColumnType("TEXT");

                    b.Property<string>("Origem")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_PROCESSO_REFERENCIADO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeProdRuralReferenciada", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnoMes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CodigoUf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("Modelo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumeroNf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Serie")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_PROD_RURAL_REFERENCIADA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeReferenciada", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChaveAcesso")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NFE_REFERENCIADA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeResponsavelTecnico", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<string>("Contato")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashCsrt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdentificadorCsrt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_RESPONSAVEL_TECNICO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeTransporte", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaRetencaoIcms")
                        .HasColumnType("REAL");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Endereco")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InscricaoEstadual")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModalidadeFrete")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Municipio")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeMunicipio")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlacaVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<string>("RntcVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.Property<string>("UfVeiculo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorBcRetencaoIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsRetido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorServico")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("NFE_TRANSPORTE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeTransporteReboque", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Balsa")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeTransporte")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Placa")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rntc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Vagao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_TRANSPORTE_REBOQUE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeTransporteVolume", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Especie")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdNfeTransporte")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Marca")
                        .HasColumnType("TEXT");

                    b.Property<string>("Numeracao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PesoBruto")
                        .HasColumnType("REAL");

                    b.Property<double?>("PesoLiquido")
                        .HasColumnType("REAL");

                    b.Property<int?>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("NFE_TRANSPORTE_VOLUME");
                });

            modelBuilder.Entity("PDV.Models.Pdv.NfeTransporteVolumeLacre", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdNfeTransporteVolume")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Numero")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NFE_TRANSPORTE_VOLUME_LACRE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvCaixa", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataCadastro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_CAIXA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvConfiguracao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AcbrMonitorEndereco")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AcbrMonitorPorta")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BitsPorSegundo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CaminhoImagensMarketing")
                        .HasColumnType("TEXT");

                    b.Property<string>("CaminhoImagensProdutos")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CfopEcf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CorJanelasInternas")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataAtualizacaoEstoque")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DecimaisQuantidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DecimaisValor")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DescricaoSangria")
                        .HasColumnType("TEXT");

                    b.Property<string>("DescricaoSuprimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("EncerraMovimentoAuto")
                        .HasColumnType("TEXT");

                    b.Property<string>("GavetaSinalInvertido")
                        .HasColumnType("TEXT");

                    b.Property<int?>("GavetaUtilizacao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfImpressora")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvCaixa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTributOperacaoFiscalPadrao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IntervaloEcf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IpServidor")
                        .HasColumnType("TEXT");

                    b.Property<string>("IpSitef")
                        .HasColumnType("TEXT");

                    b.Property<string>("Laudo")
                        .HasColumnType("TEXT");

                    b.Property<string>("MarketingAtivo")
                        .HasColumnType("TEXT");

                    b.Property<string>("MensagemCupom")
                        .HasColumnType("TEXT");

                    b.Property<string>("Modulo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuloFiscalContingencia")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuloFiscalPrincipal")
                        .HasColumnType("TEXT");

                    b.Property<string>("PedeCpfCupom")
                        .HasColumnType("TEXT");

                    b.Property<string>("PermiteEstoqueNegativo")
                        .HasColumnType("TEXT");

                    b.Property<string>("PesquisaParte")
                        .HasColumnType("TEXT");

                    b.Property<string>("Plano")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlanoSituacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PlanoValor")
                        .HasColumnType("REAL");

                    b.Property<string>("PortaEcf")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuantidadeMaximaCartoes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReciboFormatoPagina")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ReciboLarguraPagina")
                        .HasColumnType("REAL");

                    b.Property<double?>("ReciboMargemPagina")
                        .HasColumnType("REAL");

                    b.Property<int?>("TefEsperaSts")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TefNumeroVias")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TefTempoEspera")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TefTipoGp")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TimeoutEcf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TimerIntegracao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TipoIntegracao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TipoTef")
                        .HasColumnType("TEXT");

                    b.Property<string>("TituloTelaCaixa")
                        .HasColumnType("TEXT");

                    b.Property<string>("UsaTecladoReduzido")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_CONFIGURACAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvConfiguracaoBalanca", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BaudRate")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DataBits")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HandShake")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvConfiguracao")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Identificador")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Modelo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Parity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Porta")
                        .HasColumnType("TEXT");

                    b.Property<int?>("StopBits")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Timeout")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TipoConfiguracao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_CONFIGURACAO_BALANCA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvConfiguracaoLeitorSerial", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Baud")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DataBits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExcluirSufixo")
                        .HasColumnType("TEXT");

                    b.Property<int?>("HandShake")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HardFlow")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvConfiguracao")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Intervalo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Parity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Porta")
                        .HasColumnType("TEXT");

                    b.Property<string>("SoftFlow")
                        .HasColumnType("TEXT");

                    b.Property<int?>("StopBits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Sufixo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Usa")
                        .HasColumnType("TEXT");

                    b.Property<string>("UsarFila")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_CONFIGURACAO_LEITOR_SERIAL");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvFechamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvTipoPagamento")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_FECHAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvMovimento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataAbertura")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataFechamento")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraAbertura")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraFechamento")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdEcfImpressora")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdGerenteSupervisor")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvCaixa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvOperador")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StatusMovimento")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TotalAcrescimo")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalCancelado")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalFinal")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalNaoFiscal")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalRecebido")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalSangria")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalSuprimento")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalTroco")
                        .HasColumnType("REAL");

                    b.Property<double?>("TotalVenda")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_MOVIMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvOperador", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("Senha")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_OPERADOR");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvSangria", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataSangria")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraSangria")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_SANGRIA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvSuprimento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataSuprimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraSuprimento")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_SUPRIMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvTipoPagamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoPagamentoNfce")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("GeraParcelas")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImprimeVinculado")
                        .HasColumnType("TEXT");

                    b.Property<string>("PermiteTroco")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tef")
                        .HasColumnType("TEXT");

                    b.Property<string>("TefTipoGp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PDV_TIPO_PAGAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvTotalTipoPagamento", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CartaoDc")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Ccf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataVenda")
                        .HasColumnType("TEXT");

                    b.Property<string>("Estorno")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Gnf")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraVenda")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvTipoPagamento")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nsu")
                        .HasColumnType("TEXT");

                    b.Property<string>("Rede")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_TOTAL_TIPO_PAGAMENTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvVendaCabecalho", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ccf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CpfCnpjCliente")
                        .HasColumnType("TEXT");

                    b.Property<string>("CupomCancelado")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataVenda")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraVenda")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdColaborador")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfDav")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdEcfPreVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdPdvMovimento")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NomeCliente")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<string>("StatusVenda")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TaxaAcrescimo")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<string>("TipoOperacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorAcrescimo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorAcrescimoItens")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBaseIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCancelado")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDescontoItens")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorFinal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcmsOutras")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPis")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorRecebido")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalDocumento")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalProdutos")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTroco")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorVenda")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_VENDA_CABECALHO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.PdvVendaDetalhe", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cancelado")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Ccf")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Coo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cst")
                        .HasColumnType("TEXT");

                    b.Property<string>("EcfIcmsSt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gtin")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdPdvVendaCabecalho")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Item")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MovimentaEstoque")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<string>("SerieEcf")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TaxaAcrescimo")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaPis")
                        .HasColumnType("REAL");

                    b.Property<string>("TotalizadorParcial")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorAcrescimo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorBaseIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorDesconto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorImpostoEstadual")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorImpostoFederal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorImpostoMunicipal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPis")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorTotalItem")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorUnitario")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PDV_VENDA_DETALHE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Produto", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CodigoBalanca")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodigoCest")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoInterno")
                        .HasColumnType("TEXT");

                    b.Property<string>("CodigoNcm")
                        .HasColumnType("TEXT");

                    b.Property<string>("Csosn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cst")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("DescricaoPdv")
                        .HasColumnType("TEXT");

                    b.Property<string>("EcfIcmsSt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("EstoqueMaximo")
                        .HasColumnType("REAL");

                    b.Property<double?>("EstoqueMinimo")
                        .HasColumnType("REAL");

                    b.Property<string>("Gtin")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashRegistro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Iat")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdProdutoSubgrupo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProdutoTipo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProdutoUnidade")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTributGrupoTributario")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ippt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Localizacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("PafPSt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("QuantidadeEstoque")
                        .HasColumnType("REAL");

                    b.Property<string>("Situacao")
                        .HasColumnType("TEXT");

                    b.Property<double?>("TaxaCofins")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaIcms")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaIpi")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaIssqn")
                        .HasColumnType("REAL");

                    b.Property<double?>("TaxaPis")
                        .HasColumnType("REAL");

                    b.Property<string>("TipoItemSped")
                        .HasColumnType("TEXT");

                    b.Property<string>("TotalizadorParcial")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorCompra")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCusto")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorVenda")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoFichaTecnica", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProdutoFilho")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("PercentualCusto")
                        .HasColumnType("REAL");

                    b.Property<double?>("Quantidade")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorCusto")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_FICHA_TECNICA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoGrupo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_GRUPO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoImagem", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Imagem")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentType")
                        .HasColumnType("TEXT");

                    b.Property<string>("HashSha256")
                        .HasColumnType("TEXT");

                    b.Property<string>("PendenteExclusao")
                        .HasColumnType("TEXT");

                    b.Property<string>("PendenteUpload")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SincronizadoEm")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TamanhoBytes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UrlRemota")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_IMAGEM");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoPromocao", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataFim")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataInicio")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdProduto")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("QuantidadeEmPromocao")
                        .HasColumnType("REAL");

                    b.Property<double?>("QuantidadeMaximaCliente")
                        .HasColumnType("REAL");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_PROMOCAO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoSubgrupo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdProdutoGrupo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_SUBGRUPO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoTipo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_TIPO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ProdutoUnidade", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("PodeFracionar")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sigla")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PRODUTO_UNIDADE");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Reserva", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataReserva")
                        .HasColumnType("TEXT");

                    b.Property<string>("HoraReserva")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NomeContato")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuantidadePessoas")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Situacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("TelefoneContato")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RESERVA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.ReservaMesa", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdMesa")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdReserva")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("RESERVA_MESA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TaxaEntrega", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EstimativaMinutos")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TAXA_ENTREGA");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributCofins", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPorcento")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaUnidade")
                        .HasColumnType("REAL");

                    b.Property<string>("CstCofins")
                        .HasColumnType("TEXT");

                    b.Property<string>("EfdTabela435")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributConfiguraOfGt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBaseCalculo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PorcentoBaseCalculo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaFiscal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_COFINS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributConfiguraOfGt", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTributGrupoTributario")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdTributOperacaoFiscal")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_CONFIGURA_OF_GT");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributGrupoTributario", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrigemMercadoria")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_GRUPO_TRIBUTARIO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributIcmsCustomCab", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrigemMercadoria")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_ICMS_CUSTOM_CAB");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributIcmsCustomDet", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Aliquota")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaInterestadualSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaInternaSt")
                        .HasColumnType("REAL");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Csosn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cst")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributIcmsCustomCab")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBc")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModalidadeBcSt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Mva")
                        .HasColumnType("REAL");

                    b.Property<double?>("PorcentoBc")
                        .HasColumnType("REAL");

                    b.Property<double?>("PorcentoBcSt")
                        .HasColumnType("REAL");

                    b.Property<string>("UfDestino")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorPauta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximoSt")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_ICMS_CUSTOM_DET");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributIcmsUf", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Aliquota")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaIcmsSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaInterestadualSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaInternaSt")
                        .HasColumnType("REAL");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Csosn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cst")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributConfiguraOfGt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBc")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModalidadeBcSt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Mva")
                        .HasColumnType("REAL");

                    b.Property<double?>("PorcentoBc")
                        .HasColumnType("REAL");

                    b.Property<double?>("PorcentoBcSt")
                        .HasColumnType("REAL");

                    b.Property<string>("UfDestino")
                        .HasColumnType("TEXT");

                    b.Property<double?>("ValorPauta")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaSt")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximoSt")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_ICMS_UF");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributIpi", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPorcento")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaUnidade")
                        .HasColumnType("REAL");

                    b.Property<string>("CstIpi")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributConfiguraOfGt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBaseCalculo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PorcentoBaseCalculo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaFiscal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_IPI");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributIss", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPorcento")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaUnidade")
                        .HasColumnType("REAL");

                    b.Property<string>("CodigoTributacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributOperacaoFiscal")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ItemListaServico")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBaseCalculo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PorcentoBaseCalculo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaFiscal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_ISS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributOperacaoFiscal", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cfop")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<string>("DescricaoNaNf")
                        .HasColumnType("TEXT");

                    b.Property<string>("Observacao")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_OPERACAO_FISCAL");
                });

            modelBuilder.Entity("PDV.Models.Pdv.TributPis", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AliquotaPorcento")
                        .HasColumnType("REAL");

                    b.Property<double?>("AliquotaUnidade")
                        .HasColumnType("REAL");

                    b.Property<string>("CstPis")
                        .HasColumnType("TEXT");

                    b.Property<string>("EfdTabela435")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IdTributConfiguraOfGt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModalidadeBaseCalculo")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PorcentoBaseCalculo")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPautaFiscal")
                        .HasColumnType("REAL");

                    b.Property<double?>("ValorPrecoMaximo")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("TRIBUT_PIS");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID");

                    b.Property<string>("Administrador")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("ADMINISTRADOR");

                    b.Property<DateTime?>("DataCadastro")
                        .HasColumnType("TEXT")
                        .HasColumnName("DATA_CADASTRO");

                    b.Property<int>("IdColaborador")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID_COLABORADOR");

                    b.Property<int>("IdPapel")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ID_PAPEL");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("LOGIN");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SENHA");

                    b.HasKey("Id");

                    b.HasIndex("IdColaborador");

                    b.ToTable("USUARIO");
                });

            modelBuilder.Entity("PDV.Models.Pdv.Usuario", b =>
                {
                    b.HasOne("PDV.Models.Pdv.Colaborador", "Colaborador")
                        .WithMany()
                        .HasForeignKey("IdColaborador")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colaborador");
                });
#pragma warning restore 612, 618
        }
    }
}
