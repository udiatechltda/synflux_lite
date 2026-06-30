using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class EmpresaListViewModel : ListViewModelBase<Empresa>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public EmpresaListViewModel() : this(null, null) { }

        public EmpresaListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public EmpresaListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("EmpresaForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("EmpresaForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("a empresa");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var empresas = _context.Empresas?.ToList() ?? new List<Empresa>();
                    foreach (var empresa in empresas)
                    {
                        Items.Add(empresa);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar empresas: {ex.Message}");
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
                (c.RazaoSocial?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) || 
                (c.Cnpj?.Contains(SearchText) ?? false)
            ).ToList();

            Items = new ObservableCollection<Empresa>(filtered);
        }
    }
}
