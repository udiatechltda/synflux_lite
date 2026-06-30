using System.Threading.Tasks;

namespace PDV.Services.Interfaces
{
    public interface IFiscalNfceService
    {
        Task<FiscalNfceEmissionResult> EmitirNfceVendaAsync(int vendaId, bool contingencia = false);
        Task<FiscalNfceStatusResult> ConsultarStatusSefazAsync(string? uf = null);
        Task<FiscalNfceEmissionResult> TransmitirContingenciaAsync(int nfeCabecalhoId);
        Task<FiscalNfceEmissionResult> CancelarNfceAsync(int nfeCabecalhoId, string justificativa);
        Task<FiscalNfceEmissionResult> InutilizarNumeroAsync(string serie, int numeroInicial, int numeroFinal, string justificativa);
    }

    public sealed class FiscalNfceEmissionResult
    {
        public bool Sucesso { get; init; }
        public int? NfeCabecalhoId { get; init; }
        public string Numero { get; init; } = string.Empty;
        public string ChaveAcesso { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string Mensagem { get; init; } = string.Empty;
        public string CaminhoPdf { get; init; } = string.Empty;
        public string CaminhoXml { get; init; } = string.Empty;
    }

    public sealed class FiscalNfceStatusResult
    {
        public bool Disponivel { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Mensagem { get; init; } = string.Empty;
    }
}
