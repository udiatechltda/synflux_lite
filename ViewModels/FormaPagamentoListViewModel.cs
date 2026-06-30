using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class FormaPagamentoListViewModel : ListViewModelBase<PdvTipoPagamento>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public FormaPagamentoListViewModel() : this(null, null) { }

        public FormaPagamentoListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public FormaPagamentoListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("FormaPagamentoForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("FormaPagamentoForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a forma de pagamento");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var formaPagamentos = _context.PdvTiposPagamento?.ToList() ?? new List<PdvTipoPagamento>();
                    foreach (var formaPagamento in formaPagamentos)
                    {
                        Items.Add(formaPagamento);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar formas de pagamento: {ex.Message}");
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
                (c.Codigo?.Contains(SearchText) ?? false)
            ).ToList();

            Items = new ObservableCollection<PdvTipoPagamento>(filtered);
        }
    }
}
