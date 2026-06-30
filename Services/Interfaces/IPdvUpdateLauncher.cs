namespace PDV.Services.Interfaces
{
    public interface IPdvUpdateLauncher
    {
        void TryLaunchOnExit(bool hasOpenCash);
    }
}
