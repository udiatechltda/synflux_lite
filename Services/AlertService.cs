using PDV.Models;
using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PDV.Services
{
    public class AlertService : IAlertService
    {
        public ObservableCollection<AlertModel> Alerts { get; } = new ObservableCollection<AlertModel>();

        public void ShowAlert(string message, AlertType type, int duration = 5000)
        {
            var alert = new AlertModel
            {
                Message = message,
                Type = type,
                Duration = duration,
                BackgroundColor = GetBackgroundColor(type),
                TextColor = GetTextColor(type),
                BorderColor = GetBorderColor(type)
            };

            // Executar na thread da UI
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Alerts.Add(alert);
            });

            if (alert.AutoDismiss)
            {
                Task.Delay(duration).ContinueWith(_ =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        Alerts.Remove(alert);
                    });
                });
            }
        }

        public void DismissAlert(string alertId)
        {
            var alert = Alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Alerts.Remove(alert);
                });
            }
        }

        public void DismissAll()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Alerts.Clear();
            });
        }

        private SolidColorBrush GetBackgroundColor(AlertType type)
        {
            return type switch
            {
                AlertType.Success => new SolidColorBrush(Color.FromRgb(212, 237, 218)),
                AlertType.Error => new SolidColorBrush(Color.FromRgb(248, 215, 218)),
                AlertType.Warning => new SolidColorBrush(Color.FromRgb(255, 243, 205)),
                AlertType.Info => new SolidColorBrush(Color.FromRgb(209, 236, 241)),
                _ => new SolidColorBrush(Color.FromRgb(209, 236, 241))
            };
        }

        private SolidColorBrush GetTextColor(AlertType type)
        {
            return type switch
            {
                AlertType.Success => new SolidColorBrush(Color.FromRgb(21, 87, 36)),
                AlertType.Error => new SolidColorBrush(Color.FromRgb(114, 28, 36)),
                AlertType.Warning => new SolidColorBrush(Color.FromRgb(133, 100, 4)),
                AlertType.Info => new SolidColorBrush(Color.FromRgb(12, 84, 96)),
                _ => new SolidColorBrush(Color.FromRgb(12, 84, 96))
            };
        }

        private SolidColorBrush GetBorderColor(AlertType type)
        {
            return type switch
            {
                AlertType.Success => new SolidColorBrush(Color.FromRgb(195, 230, 203)),
                AlertType.Error => new SolidColorBrush(Color.FromRgb(245, 198, 203)),
                AlertType.Warning => new SolidColorBrush(Color.FromRgb(255, 238, 186)),
                AlertType.Info => new SolidColorBrush(Color.FromRgb(190, 229, 235)),
                _ => new SolidColorBrush(Color.FromRgb(190, 229, 235))
            };
        }
    }
}