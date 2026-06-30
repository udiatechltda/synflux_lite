using System.Windows.Input;

namespace PDV.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string CommandParameter { get; set; }
        public bool IsSelected { get; set; }
        public ICommand Command { get; set; }
    }

    public class MenuSection
    {
        public string Title { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<MenuItem> Items { get; set; }
    }
}