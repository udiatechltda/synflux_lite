using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels.Cadastros
{
    public class EmpresaListViewModel : ListViewModelBase<Empresa>
    {
        public EmpresaListViewModel() : this(null, null) { }
        public EmpresaListViewModel(IViewModelNavigationService? navigationService) : this(navigationService, null) { }
        public EmpresaListViewModel(IViewModelNavigationService? navigationService, PdvContext? context)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context) { }

        protected override void ExecuteNovo(object parameter)
        {
            var vm = new PDV.ViewModels.EmpresaFormViewModel(_navigationService, _context);
            _navigationService.NavigateTo(vm);
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                var vm = new PDV.ViewModels.EmpresaFormViewModel(_navigationService, _context, item);
                _navigationService.NavigateTo(vm);
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a empresa");
        }

        protected override void LoadData()
        {
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var empresas = _context.Empresas?.ToList() ?? new List<Empresa>();
                    foreach (var e in empresas) Items.Add(e);
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}"); }
            }
        }

        protected override void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) { LoadData(); return; }
            var filtered = Items.Where(c => (c.RazaoSocial?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
            Items = new ObservableCollection<Empresa>(filtered);
        }
    }
}
