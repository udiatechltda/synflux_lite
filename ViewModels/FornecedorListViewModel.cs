using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class FornecedorListViewModel : ListViewModelBase<Fornecedor>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public FornecedorListViewModel() : this(null, null) { }

        public FornecedorListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public FornecedorListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("FornecedorForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("FornecedorForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o fornecedor");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var fornecedores = _context.Fornecedores?.ToList() ?? new List<Fornecedor>();
                    foreach (var fornecedor in fornecedores)
                    {
                        Items.Add(fornecedor);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar fornecedores: {ex.Message}");
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

            Items = new ObservableCollection<Fornecedor>(filtered);
        }
    }
}
