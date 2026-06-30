using PDV.Services.Retaguarda;

namespace PDV.Services.Interfaces
{
    public interface ILocalTenantService
    {
        void GarantirTenantLocal(RetaguardaAuthSession session);
    }
}
