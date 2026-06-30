using System;
using System.Collections.ObjectModel;
using PDV.Models.Pdv.Cadastros;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using PDV.Services;
using System.Linq;

namespace PDV.ViewModels
{
    public class ColaboradorListViewModel : ListViewModelBase<Colaborador>
    {
        private readonly PdvContext? _context;
        private bool _initialized = false;

        public ColaboradorListViewModel() : this(null, null) { }

        public ColaboradorListViewModel(IViewModelNavigationService? navigationService) 
            : this(navigationService, null) { }

        public ColaboradorListViewModel(IViewModelNavigationService? navigationService, PdvContext? context) 
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context)
        {
            _context = context;
            _initialized = true;
            LoadData();
        }

        protected override void ExecuteNovo(object parameter)
        {
            _navigationService.NavigateTo("ColaboradorForm");
        }

        protected override void ExecuteEditar(object parameter)
        {
            var item = ObterItemSelecionado(parameter);
            if (item != null)
            {
                _navigationService.NavigateTo("ColaboradorForm");
            }
        }

        protected override void ExecuteExcluir(object parameter)
        {
            ObterItemSelecionado(parameter);
            ExcluirSelecionado("o colaborador");
        }

        protected override void LoadData()
        {
            if (!_initialized) return;
            
            Items.Clear();
            if (_context != null)
            {
                try
                {
                    var colaboradores = _context.Colaboradores?.ToList() ?? new List<Colaborador>();
                    foreach (var colaborador in colaboradores)
                    {
                        Items.Add(colaborador);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao carregar colaboradores: {ex.Message}");
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
                (c.Cpf?.Contains(SearchText) ?? false)
            ).ToList();

            Items = new ObservableCollection<Colaborador>(filtered);
        }
    }
}
