using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels.Cadastros
{
    public class UnidadeListViewModel : ListViewModelBase<ProdutoUnidade>
    {
        public UnidadeListViewModel() : this(null, null) { }
        public UnidadeListViewModel(IViewModelNavigationService? navigationService) : this(navigationService, null) { }
        public UnidadeListViewModel(IViewModelNavigationService? navigationService, PdvContext? context)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context) { }

        protected override void ExecuteNovo(object parameter)
        {
            var vm = new PDV.ViewModels.UnidadeFormViewModel(_navigationService, _context);
            _navigationService.NavigateTo(vm);
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var vm = new PDV.ViewModels.UnidadeFormViewModel(_navigationService, _context, item);
                _navigationService.NavigateTo(vm);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a unidade");
        }

        protected override void LoadData()
        {
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var unidades = _context.ProdutosUnidades?.ToList() ?? new List<ProdutoUnidade>();
                    foreach (var u in unidades) Items.Add(u);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}"); }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { LoadData(); return; }
            var filtered = Items.Where(c =>
                (c.Sigla?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Descricao?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
            Items = new ObservableCollection<ProdutoUnidade>(filtered);
        }
    }
}
