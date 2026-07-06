using System;
using System.Threading.Tasks;

namespace PDV.Services.Interfaces
{
    public interface IRetaguardaSyncCoordinator
    {
        bool SincronizacaoPendente { get; }

        event Action? OnRestoreCompleto;

        void Start();

        void NotificarLoginRealizado();

        void SolicitarSincronizacao(string origem);

        Task<RetaguardaSyncResult> SincronizarAgoraAsync(string origem);
    }
}
