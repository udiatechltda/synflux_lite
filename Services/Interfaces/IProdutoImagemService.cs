namespace PDV.Services.Interfaces
{
    public interface IProdutoImagemService
    {
        string? ObterImagemLocalPath(int? produtoId);
        IReadOnlyDictionary<int, string> ObterImagensLocalPath(IEnumerable<int?> produtoIds);
        void SalvarImagemProduto(int? produtoId, string origemPath);
        void RemoverImagemProduto(int? produtoId);
    }
}
