using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PDV.Models.Pdv;
using PDV.Models.Pdv.Cadastros;
using PDV.Models.Pdv.Financeiro;
using PDV.Models.Pdv.Fiscal;
using PDV.Services.Interfaces;

namespace PDV.Services
{
    /// <summary>
    /// Contexto de Banco de Dados para o sistema PDV
    /// Gerencia todas as operações com o banco de dados pdv.sqlite
    /// </summary>
    public class PdvContext : DbContext
    {
        private readonly IRetaguardaSyncCoordinator? _syncCoordinator;

        public PdvContext(DbContextOptions<PdvContext> options, IRetaguardaSyncCoordinator? syncCoordinator = null) : base(options)
        {
            _syncCoordinator = syncCoordinator;
        }

        // ===== CADASTROS =====
        public DbSet<Cardapio> Cardapios { get; set; }
        public DbSet<CardapioPerguntaPadrao> CardapioPerguntas { get; set; }
        public DbSet<CardapioRespostaPadrao> CardapioRespostas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteFiado> ClientesFiados { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Contador> Contadores { get; set; }
        public DbSet<Cozinha> Cozinhas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoFichaTecnica> ProdutosFichaTecnica { get; set; }
        public DbSet<ProdutoGrupo> ProdutosGrupos { get; set; }
        public DbSet<ProdutoImagem> ProdutosImagens { get; set; }
        public DbSet<ProdutoPromocao> ProdutosPromocoes { get; set; }
        public DbSet<ProdutoSubgrupo> ProdutosSubgrupos { get; set; }
        public DbSet<ProdutoTipo> ProdutosTipos { get; set; }
        public DbSet<ProdutoUnidade> ProdutosUnidades { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaMesa> ReservasMesas { get; set; }

        // ===== FINANCEIRO =====
        public DbSet<ContasPagar> ContasPagar { get; set; }
        public DbSet<ContasReceber> ContasReceber { get; set; }
        public DbSet<FidelidadeHistorico> FidelidadeHistoricos { get; set; }
        public DbSet<FidelidadeUtilizado> FidelidadeUtilizados { get; set; }
        public DbSet<Ibpt> Ibpts { get; set; }
        public DbSet<NfcePlanoPagamento> NfcePlanoPagamentos { get; set; }
        public DbSet<TaxaEntrega> TaxasEntrega { get; set; }

        // ===== FISCAL =====
        public DbSet<Cfop> Cfops { get; set; }
        public DbSet<EcfAliquotas> EcfAliquotas { get; set; }
        public DbSet<EcfDocumentosEmitidos> EcfDocumentosEmitidos { get; set; }
        public DbSet<EcfE3> EcfE3s { get; set; }
        public DbSet<EcfImpressora> EcfImpressoras { get; set; }
        public DbSet<EcfLogTotais> EcfLogTotais { get; set; }
        public DbSet<EcfR01> EcfR01s { get; set; }
        public DbSet<EcfR02> EcfR02s { get; set; }
        public DbSet<EcfR03> EcfR03s { get; set; }
        public DbSet<EcfR06> EcfR06s { get; set; }
        public DbSet<EcfR07> EcfR07s { get; set; }
        public DbSet<EcfRecebimentoNaoFiscal> EcfRecebimentoNaoFiscal { get; set; }
        public DbSet<EcfRelatorioGerencial> EcfRelatoriosGerenciais { get; set; }
        public DbSet<EcfSintegra60A> EcfSintegra60A { get; set; }
        public DbSet<EcfSintegra60M> EcfSintegra60M { get; set; }
        public DbSet<NfeAcessoXml> NfeAcessoXml { get; set; }
        public DbSet<NfeCabecalho> NfeCabecalhos { get; set; }
        public DbSet<NfeCana> NfeCanas { get; set; }
        public DbSet<NfeCanaDeducoesSafra> NfeCanaDeducoesSafra { get; set; }
        public DbSet<NfeCanaFornecimentoDiario> NfeCanaFornecimentoDiario { get; set; }
        public DbSet<NfeConfiguracao> NfeConfiguracoes { get; set; }
        public DbSet<NfeCteReferenciado> NfeCteReferenciado { get; set; }
        public DbSet<NfeCupomFiscalReferenciado> NfeCupomFiscalReferenciado { get; set; }
        public DbSet<NfeDeclaracaoImportacao> NfeDeclaracaoImportacao { get; set; }
        public DbSet<NfeDestinatario> NfeDestinatarios { get; set; }
        public DbSet<NfeDetalhe> NfeDetalhes { get; set; }
        public DbSet<NfeDetalheImpostoCofins> NfeDetalheImpostoCofins { get; set; }
        public DbSet<NfeDetalheImpostoCofinsSt> NfeDetalheImpostoCofinsSt { get; set; }
        public DbSet<NfeDetalheImpostoIcms> NfeDetalheImpostoIcms { get; set; }
        public DbSet<NfeDetalheImpostoIcmsUfdest> NfeDetalheImpostoIcmsUfdest { get; set; }
        public DbSet<NfeDetalheImpostoIi> NfeDetalheImpostoIi { get; set; }
        public DbSet<NfeDetalheImpostoIpi> NfeDetalheImpostoIpi { get; set; }
        public DbSet<NfeDetalheImpostoIssqn> NfeDetalheImpostoIssqn { get; set; }
        public DbSet<NfeDetalheImpostoPis> NfeDetalheImpostoPis { get; set; }
        public DbSet<NfeDetalheImpostoPisSt> NfeDetalheImpostoPisSt { get; set; }
        public DbSet<NfeDetEspecificoArmamento> NfeDetEspecificoArmamento { get; set; }
        public DbSet<NfeDetEspecificoCombustivel> NfeDetEspecificoCombustivel { get; set; }
        public DbSet<NfeDetEspecificoMedicamento> NfeDetEspecificoMedicamento { get; set; }
        public DbSet<NfeDetEspecificoVeiculo> NfeDetEspecificoVeiculo { get; set; }
        public DbSet<NfeDuplicata> NfeDuplicatas { get; set; }
        public DbSet<NfeEmitente> NfeEmitentes { get; set; }
        public DbSet<NfeExportacao> NfeExportacoes { get; set; }
        public DbSet<NfeFatura> NfeFaturas { get; set; }
        public DbSet<NfeImportacaoDetalhe> NfeImportacaoDetalhe { get; set; }
        public DbSet<NfeInformacaoPagamento> NfeInformacaoPagamento { get; set; }
        public DbSet<NfeItemRastreado> NfeItemRastreado { get; set; }
        public DbSet<NfeLocalEntrega> NfeLocalEntrega { get; set; }
        public DbSet<NfeLocalRetirada> NfeLocalRetirada { get; set; }
        public DbSet<NfeNfReferenciada> NfeNfReferenciada { get; set; }
        public DbSet<NfeNumero> NfeNumeros { get; set; }
        public DbSet<NfeNumeroInutilizado> NfeNumerosInutilizados { get; set; }
        public DbSet<NfeProcessoReferenciado> NfeProcessoReferenciado { get; set; }
        public DbSet<NfeProdRuralReferenciada> NfeProdRuralReferenciada { get; set; }
        public DbSet<NfeReferenciada> NfeReferenciada { get; set; }
        public DbSet<NfeResponsavelTecnico> NfeResponsavelTecnico { get; set; }
        public DbSet<NfeTransporte> NfeTransportes { get; set; }
        public DbSet<NfeTransporteReboque> NfeTransporteReboque { get; set; }
        public DbSet<NfeTransporteVolume> NfeTransporteVolume { get; set; }
        public DbSet<NfeTransporteVolumeLacre> NfeTransporteVolumeLacre { get; set; }
        public DbSet<TributCofins> TributCofins { get; set; }
        public DbSet<TributConfiguraOfGt> TributConfiguraOfGt { get; set; }
        public DbSet<TributGrupoTributario> TributGruposTributarios { get; set; }
        public DbSet<TributIcmsCustomCab> TributIcmsCustomCab { get; set; }
        public DbSet<TributIcmsCustomDet> TributIcmsCustomDet { get; set; }
        public DbSet<TributIcmsUf> TributIcmsUf { get; set; }
        public DbSet<TributIpi> TributIpi { get; set; }
        public DbSet<TributIss> TributIss { get; set; }
        public DbSet<TributOperacaoFiscal> TributOperacoesFiscais { get; set; }
        public DbSet<TributPis> TributPis { get; set; }

        // ===== PDV =====
        public DbSet<Comanda> Comandas { get; set; }
        public DbSet<ComandaDetalhe> ComandaDetalhes { get; set; }
        public DbSet<ComandaDetalheComplemento> ComandaDetalheComplementos { get; set; }
        public DbSet<ComandaObservacaoPadrao> ComandaObservacoesPadrao { get; set; }
        public DbSet<ComandaPedido> ComandaPedidos { get; set; }
        public DbSet<CompraPedidoCabecalho> CompraPedidosCabecalho { get; set; }
        public DbSet<CompraPedidoDetalhe> CompraPedidosDetalhe { get; set; }
        public DbSet<Delivery> Deliverys { get; set; }
        public DbSet<DeliveryAcerto> DeliveryAcertos { get; set; }
        public DbSet<DeliveryAcertoComanda> DeliveryAcertoComandas { get; set; }
        public DbSet<EmpresaDeliveryPedido> EmpresasDeliveryPedido { get; set; }
        public DbSet<EntregadorRota> EntregadorRotas { get; set; }
        public DbSet<EntregadorRotaDetalhe> EntregadorRotaDetalhes { get; set; }
        public DbSet<PdvCaixa> PdvCaixas { get; set; }
        public DbSet<PdvConfiguracao> PdvConfiguracoes { get; set; }
        public DbSet<PdvConfiguracaoBalanca> PdvConfiguracaoBalancas { get; set; }
        public DbSet<PdvConfiguracaoLeitorSerial> PdvConfiguracaoLeitoresSerial { get; set; }
        public DbSet<PdvFechamento> PdvFechamentos { get; set; }
        public DbSet<PdvMovimento> PdvMovimentos { get; set; }
        public DbSet<PdvOperador> PdvOperadores { get; set; }
        public DbSet<PdvSangria> PdvSangrias { get; set; }
        public DbSet<PdvSuprimento> PdvSuprimentos { get; set; }
        public DbSet<PdvTipoPagamento> PdvTiposPagamento { get; set; }
        public DbSet<PdvTotalTipoPagamento> PdvTotaisTipoPagamento { get; set; }
        public DbSet<PdvVendaCabecalho> PdvVendasCabecalho { get; set; }
        public DbSet<PdvVendaDetalhe> PdvVendasDetalhe { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configura o caminho do banco de dados relativo à pasta raiz do projeto
                string databasePath = Path.Combine(AppContext.BaseDirectory, "pdv.sqlite");
                optionsBuilder.UseSqlite($"Data Source={databasePath}");
            }

            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            var registrosAfetados = base.SaveChanges();
            SolicitarSincronizacaoSeNecessario(registrosAfetados);
            return registrosAfetados;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var registrosAfetados = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            SolicitarSincronizacaoSeNecessario(registrosAfetados);
            return registrosAfetados;
        }

        private void SolicitarSincronizacaoSeNecessario(int registrosAfetados)
        {
            if (registrosAfetados > 0)
                _syncCoordinator?.SolicitarSincronizacao("PdvContext.SaveChanges");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais de modelo podem ser adicionadas aqui
            // Por enquanto, confiamos nos atributos [Table] das classes
        }
    }
}
