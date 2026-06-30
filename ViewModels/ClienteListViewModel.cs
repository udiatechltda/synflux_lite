using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class ClienteListViewModel : ListViewModelBase<Cliente>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public ClienteListViewModel() : this(null, null) { }

        public ClienteListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public ClienteListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            // Carregar dados APÓS _context estar pronto
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            // Navegar para o formulário de novo cliente
            _navigationService.NavigateTo("ClienteForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var formViewModel = new ClienteFormViewModel(_navigationService, _context, item);
                _navigationService.NavigateTo(formViewModel);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o cliente");
        }

        protected override void LoadData()
        {
            // Ignora chamada do construtor base (quando _initialized = false)
            if (!_initialized) return;

            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var clientes = _context.Clientes?.ToList() ?? new List<Cliente>();
                    foreach (var cliente in clientes)
                    {
                        Items.Add(cliente);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar clientes: {ex.Message}");
                }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadData();
                return;
            }

            var filtered = Items.Where(c => 
                (c.Nome?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) || 
                (c.CpfCnpj?.Contains(SearchText) ?? false)
            ).ToList();

            Items = new ObservableCollection<Cliente>(filtered);
        }
    }
}
