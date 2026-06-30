using PDV.Services.Retaguarda;

namespace PDV.Services.Interfaces
{
    public interface ILocalDatabaseService
    {
        string CurrentDatabasePath { get; }
        string CurrentDatabaseKey { get; }
        string LoginDatabasePath { get; }
        string GetTenantDatabasePath(RetaguardaAuthSession session);
        void UseTenantDatabase(RetaguardaAuthSession session);
    }
}
