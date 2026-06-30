using System;
using System.Windows.Media;

namespace PDV.Models
{
    public class AlertModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Message { get; set; }
        public AlertType Type { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }
        public SolidColorBrush TextColor { get; set; }
        public SolidColorBrush BorderColor { get; set; }
        public int Duration { get; set; } = 5000; // 5 segundos padrão
        public bool AutoDismiss { get; set; } = true;
    }

    public enum AlertType
    {
        Success,
        Error,
        Warning,
        Info
    }
}