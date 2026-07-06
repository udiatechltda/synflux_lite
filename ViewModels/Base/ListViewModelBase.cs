using PDV.Services;
using PDV.Services.Interfaces;
using PDV.Utilities.Converters;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PDV.ViewModels.Base
{
    public abstract class ListViewModelBase<T> : ViewModelBase
    {
        protected readonly IViewModelNavigationService _navigationService;
        protected PdvContext? _context;
        private readonly IRetaguardaSyncCoordinator? _syncCoordinator;
        private ObservableCollection<T> _items;
        private T _selectedItem;
        private string _searchText;

        public ObservableCollection<T> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public T SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    OnSearch();
            }
        }

        public ICommand NovoCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand VoltarCommand { get; }

        protected ListViewModelBase(IViewModelNavigationService navigationService, PdvContext? context = null, IRetaguardaSyncCoordinator? syncCoordinator = null)
        {
            _navigationService = navigationService;
            _context = context;
            _syncCoordinator = syncCoordinator;
            Items = new ObservableCollection<T>();
            NovoCommand = new RelayCommand<object>(ExecuteNovo);
            EditarCommand = new RelayCommand<object>(ExecuteEditar);
            ExcluirCommand = new RelayCommand<object>(ExecuteExcluir);
            VoltarCommand = new RelayCommand<object>(ExecuteVoltar);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            if (_syncCoordinator != null)
                _syncCoordinator.OnRestoreCompleto += OnRestoreCompleto;

            LoadData();
        }

        private void OnRestoreCompleto()
        {
            Application.Current?.Dispatcher.Invoke(LoadData);
        }

        protected virtual void ExecuteVoltar(object parameter)
        {
            _navigationService.NavigateTo("Cadastros");
        }

        protected T? ObterItemSelecionado(object parameter)
        {
            if (parameter is T item)
            {
                SelectedItem = item;
                return item;
            }

            return SelectedItem;
        }

        protected void ExcluirSelecionado(string nomeEntidade)
        {
            if (SelectedItem == null)
            {
                MessageBox.Show($"Selecione {nomeEntidade} para excluir.", "Exclusao",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_context == null)
            {
                MessageBox.Show("Erro: contexto de banco de dados nao disponivel.", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirmacao = MessageBox.Show(
                $"Confirma a exclusao de {nomeEntidade} selecionado?",
                "Confirmar exclusao",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
                return;

            try
            {
                _context.Remove(SelectedItem);
                _context.SaveChanges();
                Items.Remove(SelectedItem);
                SelectedItem = default;
                MessageBox.Show($"{nomeEntidade} excluido com sucesso.", "Sucesso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                var innermost = ex;
                while (innermost.InnerException != null)
                    innermost = innermost.InnerException;

                MessageBox.Show(
                    $"Nao foi possivel excluir {nomeEntidade}.\n\nCausa: {innermost.Message}",
                    "Erro ao excluir",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                LoadData();
            }
        }

        protected abstract void ExecuteNovo(object parameter);
        protected abstract void ExecuteEditar(object parameter);
        protected abstract void ExecuteExcluir(object parameter);
        protected abstract void LoadData();
        protected virtual void OnSearch() { }
    }
}
