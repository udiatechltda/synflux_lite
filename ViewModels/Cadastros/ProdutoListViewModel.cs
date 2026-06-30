using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels.Cadastros
{
    public class ProdutoListViewModel : ListViewModelBase<Produto>
    {
        private readonly IProdutoImagemService? _produtoImagemService;

        public ProdutoListViewModel() : this(null, null, null) { }
        public ProdutoListViewModel(IViewModelNavigationService? navigationService) : this(navigationService, null, null) { }
        public ProdutoListViewModel(
            IViewModelNavigationService? navigationService,
            PdvContext? context,
            IProdutoImagemService? produtoImagemService = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _produtoImagemService = produtoImagemService;
        }

        protected override void ExecuteNovo(object parameter)
        {
            var vm = new PDV.ViewModels.ProdutoFormViewModel(_navigationService, _context, _produtoImagemService);
            _navigationService.NavigateTo(vm);
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var vm = new PDV.ViewModels.ProdutoFormViewModel(_navigationService, _context, _produtoImagemService, item);
                _navigationService.NavigateTo(vm);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o produto");
        }

        protected override void LoadData()
        {
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var produtos = _context.Produtos?.ToList() ?? new List<Produto>();
                    foreach (var p in produtos) Items.Add(p);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}"); }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { LoadData(); return; }
            var filtered = Items.Where(c => (c.Descricao?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            Items = new ObservableCollection<Produto>(filtered);
        }
    }
}
