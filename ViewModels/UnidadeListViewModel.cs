using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class UnidadeListViewModel : ListViewModelBase<ProdutoUnidade>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public UnidadeListViewModel() : this(null, null) { }

        public UnidadeListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public UnidadeListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("UnidadeForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("UnidadeForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a unidade");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var unidades = _context.ProdutosUnidades?.ToList() ?? new List<ProdutoUnidade>();
                    foreach (var unidade in unidades)
                    {
                        Items.Add(unidade);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar unidades: {ex.Message}");
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
                (c.Descricao?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) || 
                (c.Sigla?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();

            Items = new ObservableCollection<ProdutoUnidade>(filtered);
        }
    }
}
