using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels.Cadastros
{
    public class ClienteListViewModel : ListViewModelBase<Cliente>
    {
        public ClienteListViewModel() : this(null, null) { }
        public ClienteListViewModel(IViewModelNavigationService? navigationService) : this(navigationService, null) { }
        public ClienteListViewModel(IViewModelNavigationService? navigationService, PdvContext? context)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context) { }

        protected override void ExecuteNovo(object parameter)
        {
            var vm = new PDV.ViewModels.ClienteFormViewModel(_navigationService, _context);
            _navigationService.NavigateTo(vm);
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var vm = new PDV.ViewModels.ClienteFormViewModel(_navigationService, _context, item);
                _navigationService.NavigateTo(vm);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o cliente");
        }

        protected override void LoadData()
        {
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var clientes = _context.Clientes?.ToList() ?? new List<Cliente>();
                    foreach (var c in clientes) Items.Add(c);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}"); }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { LoadData(); return; }
            var filtered = Items.Where(c =>
                (c.Nome?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
            Items = new ObservableCollection<Cliente>(filtered);
        }
    }
}
