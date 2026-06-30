using PDV.Models;
using System.Collections.ObjectModel;

namespace PDV.Services.Interfaces
{
    public interface IAlertService
    {
        ObservableCollection<AlertModel> Alerts { get; }
        void ShowAlert(string message, AlertType type, int duration = 5000);
        void DismissAlert(string alertId);
        void DismissAll();
    }
}