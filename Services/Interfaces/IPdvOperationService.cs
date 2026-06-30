using PDV.Models.Pdv;

namespace PDV.Services.Interfaces
{
    public interface IPdvOperationService
    {
        PdvMovimento? ObterMovimentoAberto();
        bool ExisteMovimentoAberto();
        PdvMovimento AbrirMovimento(decimal fundoTroco);
        IReadOnlyList<ProdutoVendaDto> BuscarProdutos(string termo, int limite = 20);
        VendaResumoDto FinalizarVenda(IEnumerable<VendaItemDto> itens, decimal valorRecebido);
        VendaResumoDto FinalizarVenda(VendaInput input);
        void RegistrarSangria(decimal valor, string? observacao);
        void RegistrarSuprimento(decimal valor, string? observacao);
        PdvMovimento FecharMovimento(decimal valorInformado, string? tipoPagamento);
        IReadOnlyList<VendaResumoDto> ListarVendas(DateTime? mes = null, string? status = null);
        IReadOnlyList<EstoqueItemDto> ListarEstoque(string? status = null);
        IReadOnlyList<ContaPagarDto> ListarContasPagar();
        IReadOnlyList<ContaReceberDto> ListarContasReceber();
        IReadOnlyList<CompraPedidoDto> ListarCompras();
        CompraPedidoDto SalvarPedidoCompra(CompraPedidoInput input);
    }

    public record ProdutoVendaDto(
        int? Id,
        string Gtin,
        string CodigoInterno,
        string Descricao,
        decimal ValorVenda,
        decimal QuantidadeEstoque,
        string? ImagemLocalPath);

    public record VendaItemDto(
        int? ProdutoId,
        string Gtin,
        string Descricao,
        decimal Quantidade,
        decimal ValorUnitario,
        decimal Desconto);

    public record VendaPagamentoDto(
        string TipoPagamento,
        decimal Valor,
        DateTime? DataVencimento = null,
        string? Nsu = null,
        string? Rede = null);

    public record VendaInput(
        IEnumerable<VendaItemDto> Itens,
        IEnumerable<VendaPagamentoDto> Pagamentos,
        int? ClienteId = null,
        string? NomeCliente = null,
        string? CpfCnpjCliente = null,
        bool ClienteFiado = false);

    public record VendaResumoDto(
        int? Id,
        int? Movimento,
        DateTime? DataVenda,
        TimeSpan? HoraVenda,
        decimal ValorVenda,
        decimal TaxaDesconto,
        decimal ValorDesconto,
        decimal ValorFinal,
        decimal ValorRecebido,
        decimal ValorTroco,
        string Status);

    public record EstoqueItemDto(
        int? Id,
        decimal QuantidadeEstoque,
        decimal EstoqueMinimo,
        decimal EstoqueMaximo,
        string Nome,
        string Unidade,
        string Gtin,
        string CodigoInterno,
        string Descricao,
        bool Critico);

    public record ContaPagarDto(
        string Fornecedor,
        string Status,
        DateTime DataLancamento,
        DateTime DataVencimento,
        DateTime? DataPagamento,
        decimal ValorAPagar,
        decimal TaxaJuros,
        decimal ValorJuros,
        decimal ValorPago);

    public record ContaReceberDto(
        string Cliente,
        string Status,
        DateTime DataLancamento,
        DateTime DataVencimento,
        DateTime? DataRecebimento,
        decimal ValorAReceber,
        decimal TaxaJuros,
        decimal ValorJuros,
        decimal ValorRecebido);

    public record CompraPedidoDto(
        int? Id,
        string Colaborador,
        string Fornecedor,
        DateTime? DataPedido,
        DateTime? DataPrevistaEntrega,
        DateTime? DataPrevisaoPagamento,
        string LocalEntrega,
        string LocalCobranca,
        string NomeContato);

    public record CompraPedidoInput(
        string? Colaborador,
        string Fornecedor,
        string? NomeContato,
        DateTime? DataPedido,
        DateTime? DataPrevistaEntrega,
        DateTime? DataPrevisaoPagamento,
        string? CondicaoPagamento,
        DateTime? DataRecebimentoItens,
        string? HoraRecebimentoItens,
        string? LocalEntrega,
        string? LocalCobranca,
        string? NumeroDocumentoEntrada,
        decimal ValorSubtotal,
        decimal TaxaDesconto,
        decimal ValorDesconto,
        decimal ValorTotal,
        int QuantidadeParcelas,
        int IntervaloParcelas,
        DateTime? DiaPrimeiroVencimento,
        string? DiaFixoParcela);
}
