using System.Net.Http;
using System.Threading.Tasks;

namespace PDV.Services.Interfaces
{
    public interface IProdutoImagemSyncService
    {
        Task<int> SincronizarAsync(HttpClient httpClient);
    }
}
