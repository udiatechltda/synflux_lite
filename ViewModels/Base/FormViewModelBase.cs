using PDV.Services;
using PDV.Services.Interfaces;
using PDV.Utilities.Converters;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PDV.ViewModels.Base
{
    public abstract class FormViewModelBase<T> : ViewModelBase where T : class, new()
    {
        protected readonly IViewModelNavigationService _navigationService;
        protected PdvContext? _context;
        private T _model;
        private bool _isEdicao;

        public T Model { get => _model; set => SetProperty(ref _model, value); }
        public bool IsEdicao { get => _isEdicao; set => SetProperty(ref _isEdicao, value); }
        public string Titulo => IsEdicao ? "Editar" : "Novo";

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        protected FormViewModelBase(IViewModelNavigationService navigationService, T? model = null)
            : this(navigationService, null, model) { }

        protected FormViewModelBase(IViewModelNavigationService navigationService, PdvContext? context, T? model = null)
        {
            _navigationService = navigationService;
            _context = context;
            Model = model ?? new T();
            IsEdicao = model != null;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            SalvarCommand = new RelayCommand<object>(ExecuteSalvar);
            CancelarCommand = new RelayCommand<object>(ExecuteCancelar);
        }

        protected bool ValidarContexto()
        {
            if (_context != null) return true;
            MessageBox.Show("Erro: contexto de banco de dados não disponível.", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        protected string ExtrairMensagemRaiz(Exception ex)
        {
            var innermost = ex;
            while (innermost.InnerException != null)
                innermost = innermost.InnerException;
            return innermost == ex ? ex.Message : $"{ex.Message}\n\nCausa raiz: {innermost.Message}";
        }

        protected void MostrarErroBanco(Exception ex, string nomeEntidade)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao salvar {nomeEntidade}: {ex}");
            MessageBox.Show($"Erro ao salvar {nomeEntidade}:\n\n{ExtrairMensagemRaiz(ex)}", "Erro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected abstract void ExecuteSalvar(object parameter);
        protected virtual void ExecuteCancelar(object parameter) { OnVoltar(); }
        protected abstract void OnVoltar();
    }
}