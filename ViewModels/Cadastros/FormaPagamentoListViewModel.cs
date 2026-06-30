using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels.Cadastros
{
    public class FormaPagamentoListViewModel : ListViewModelBase<PdvTipoPagamento>
    {
        public FormaPagamentoListViewModel() : this(null, null) { }
        public FormaPagamentoListViewModel(IViewModelNavigationService? navigationService) : this(navigationService, null) { }
        public FormaPagamentoListViewModel(IViewModelNavigationService? navigationService, PdvContext? context)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context) { }

        protected override void ExecuteNovo(object parameter)
        {
            var vm = new PDV.ViewModels.FormaPagamentoFormViewModel(_navigationService, _context);
            _navigationService.NavigateTo(vm);
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var vm = new PDV.ViewModels.FormaPagamentoFormViewModel(_navigationService, _context, item);
                _navigationService.NavigateTo(vm);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a forma de pagamento");
        }

        protected override void LoadData()
        {
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var formas = _context.PdvTiposPagamento?.ToList() ?? new List<PdvTipoPagamento>();
                    foreach (var f in formas) Items.Add(f);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}"); }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { LoadData(); return; }
            var filtered = Items.Where(c => (c.Descricao?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            Items = new ObservableCollection<PdvTipoPagamento>(filtered);
        }
    }
}
