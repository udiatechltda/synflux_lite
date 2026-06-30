using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class ProdutoListViewModel : ListViewModelBase<Produto>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public ProdutoListViewModel() : this(null, null) { }

        public ProdutoListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public ProdutoListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("ProdutoForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("ProdutoForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o produto");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var produtos = _context.Produtos?.ToList() ?? new List<Produto>();
                    foreach (var produto in produtos)
                    {
                        Items.Add(produto);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar produtos: {ex.Message}");
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
                (c.Gtin?.Contains(SearchText) ?? false)
            ).ToList();

            Items = new ObservableCollection<Produto>(filtered);
        }
    }
}
