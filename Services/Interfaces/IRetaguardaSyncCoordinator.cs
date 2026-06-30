using System.Threading.Tasks;

namespace PDV.Services.Interfaces
{
    public interface IRetaguardaSyncCoordinator
    {
        bool SincronizacaoPendente { get; }

        void Start();

        void SolicitarSincronizacao(string origem);

        Task<RetaguardaSyncResult> SincronizarAgoraAsync(string origem);
    }
}
