using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;
using PDV.ViewModels;
using System;
using System.Collections.Generic;

namespace PDV.Services
{
    public class ViewModelNavigationService : IViewModelNavigationService
    {
        private ViewModelBase? _currentViewModel;
        private readonly IServiceProvider? _serviceProvider;
        private readonly Dictionary<string, Type> _viewModelMappings;

        public event Action<ViewModelBase>? CurrentViewModelChanged;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel ?? new HomeViewModel();
            private set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke(_currentViewModel);
            }
        }

        public ViewModelNavigationService()
        {
            _serviceProvider = null;
            _viewModelMappings = CreateMappings();
            CurrentViewModel = new HomeViewModel();
        }

        public ViewModelNavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _viewModelMappings = CreateMappings();
            CurrentViewModel = new HomeViewModel();
        }

        private Dictionary<string, Type> CreateMappings()
        {
            return new Dictionary<string, Type>
            {
                { "Home",           typeof(HomeViewModel) },
                { "PDV",            typeof(HomeViewModel) },
                { "AberturaCaixa",  typeof(AberturaCaixaViewModel) },
                { "Movimento",      typeof(MovimentoViewModel) },
                { "MovimentoPlus",  typeof(MovimentoPlusViewModel) },
                { "Suprimento",     typeof(SuprimentoViewModel) },
                { "Sangria",        typeof(SangriaViewModel) },
                { "Compras",        typeof(ComprasViewModel) },
                { "Estoque",        typeof(EstoqueViewModel) },
                { "Financeiro",     typeof(FinanceiroViewModel) },
                { "Dashboard",      typeof(DashboardViewModel) },
                { "Vendas",         typeof(VendasViewModel) },
                { "Configuracoes",  typeof(ConfiguracoesViewModel) },
                { "CadastrosPlus",  typeof(CadastrosPlusViewModel) },
                { "PdvPlus",        typeof(PdvPlusViewModel) },
                { "Fiscal",         typeof(FiscalViewModel) },
                { "Tributacao",     typeof(TributacaoViewModel) },
                { "Food",           typeof(FoodViewModel) },
                { "Delivery",       typeof(DeliveryViewModel) },
                { "Relatorios",     typeof(RelatoriosViewModel) },
                { "ConfiguracoesPlus", typeof(ConfiguracoesPlusViewModel) },
                { "Ajuda",          typeof(AjudaViewModel) },
                { "Contato",        typeof(ContatoViewModel) },
                { "Sair",           typeof(SairViewModel) },
                { "Cadastros",      typeof(CadastrosPanelViewModel) },

                // List ViewModels
                { "ClienteList",        typeof(PDV.ViewModels.Cadastros.ClienteListViewModel) },
                { "FornecedorList",     typeof(PDV.ViewModels.Cadastros.FornecedorListViewModel) },
                { "ColaboradorList",    typeof(PDV.ViewModels.Cadastros.ColaboradorListViewModel) },
                { "EmpresaList",        typeof(PDV.ViewModels.Cadastros.EmpresaListViewModel) },
                { "FormaPagamentoList", typeof(PDV.ViewModels.Cadastros.FormaPagamentoListViewModel) },
                { "UnidadeList",        typeof(PDV.ViewModels.Cadastros.UnidadeListViewModel) },
                { "ProdutoList",        typeof(PDV.ViewModels.Cadastros.ProdutoListViewModel) },

                // Form ViewModels
                { "ClienteForm",        typeof(ClienteFormViewModel) },
                { "FornecedorForm",     typeof(FornecedorFormViewModel) },
                { "ColaboradorForm",    typeof(ColaboradorFormViewModel) },
                { "EmpresaForm",        typeof(EmpresaFormViewModel) },
                { "FormaPagamentoForm", typeof(FormaPagamentoFormViewModel) },
                { "UnidadeForm",        typeof(UnidadeFormViewModel) },
                { "ProdutoForm",        typeof(ProdutoFormViewModel) },
                { "ComprasForm",        typeof(ComprasFormViewModel) },

                // Importar Produto
                { "ImportarProduto",  typeof(ImportarProdutoViewModel) },
                { "NovoProduto",      typeof(NovoProdutoViewModel) },

                // ✅ Produto Inserindo — tela de cadastro de produto
                { "ProdutoInserindo", typeof(PDV.ViewModels.NovoProduto.ProdutoInserindoViewModel) },

                // Financeiro
                { "ContasPagar",    typeof(ContasPagarViewModel) },
                { "ContasReceber",  typeof(ContasReceberViewModel) },
            };
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            try
            {
                CurrentViewModel = ResolveViewModel<T>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao navegar para {typeof(T).Name}: {ex.Message}");
                CurrentViewModel = new HomeViewModel();
            }
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }

        public void NavigateTo(string viewName)
        {
            try
            {
                if (_viewModelMappings.TryGetValue(viewName, out Type viewModelType))
                {
                    CurrentViewModel = ResolveViewModel(viewModelType);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Tipo de ViewModel não encontrado: {viewName}");
                    CurrentViewModel = new HomeViewModel();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao navegar para {viewName}: {ex.Message}");
                CurrentViewModel = new HomeViewModel();
            }
        }

        private ViewModelBase ResolveViewModel<T>() where T : ViewModelBase
        {
            if (_serviceProvider != null)
            {
                var instance = _serviceProvider.GetService(typeof(T));
                if (instance is ViewModelBase viewModel)
                    return viewModel;
            }

            try { return (ViewModelBase)Activator.CreateInstance(typeof(T), this); }
            catch { return (ViewModelBase)Activator.CreateInstance(typeof(T)); }
        }

        private ViewModelBase ResolveViewModel(Type viewModelType)
        {
            if (_serviceProvider != null)
            {
                var instance = _serviceProvider.GetService(viewModelType);
                if (instance is ViewModelBase viewModel)
                    return viewModel;
            }

            try { return (ViewModelBase)Activator.CreateInstance(viewModelType, this); }
            catch { return (ViewModelBase)Activator.CreateInstance(viewModelType); }
        }
    }
}
