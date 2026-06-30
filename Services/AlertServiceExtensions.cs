using PDV.Models;
using PDV.Services.Interfaces;

namespace PDV.Services
{
    public static class AlertServiceExtensions
    {
        public static void ShowSuccess(this IAlertService alertService, string message, int duration = 5000)
        {
            alertService.ShowAlert(message, AlertType.Success, duration);
        }

        public static void ShowError(this IAlertService alertService, string message, int duration = 5000)
        {
            alertService.ShowAlert(message, AlertType.Error, duration);
        }

        public static void ShowWarning(this IAlertService alertService, string message, int duration = 5000)
        {
            alertService.ShowAlert(message, AlertType.Warning, duration);
        }

        public static void ShowInfo(this IAlertService alertService, string message, int duration = 5000)
        {
            alertService.ShowAlert(message, AlertType.Info, duration);
        }
    }
}