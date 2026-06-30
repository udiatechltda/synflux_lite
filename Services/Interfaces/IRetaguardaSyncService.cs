using System.Threading.Tasks;

namespace PDV.Services.Interfaces
{
    public interface IRetaguardaSyncService
    {
        Task<RetaguardaSyncResult> SincronizarTudoAsync();
    }

    public sealed class RetaguardaSyncResult
    {
        public bool Sincronizado { get; init; }
        public string Mensagem { get; init; } = string.Empty;
        public string BancoOperacional { get; init; } = string.Empty;
        public int TotalTabelas { get; init; }
        public int TotalRegistros { get; init; }
    }
}
