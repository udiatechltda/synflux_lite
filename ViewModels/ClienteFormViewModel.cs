using System;
using System.Windows;
using PDV.Models.Pdv.Cadastros;
using PDV.Services;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;
using System.Windows.Input;
using PDV.Utilities.Converters;

namespace PDV.ViewModels
{
    public class ClienteFormViewModel : FormViewModelBase<Cliente>
    {
        public ClienteFormViewModel() : this(null, null, null) { }

        public ClienteFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, Cliente? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model)
        {
            BuscarCepCommand = new RelayCommand<object>(ExecuteBuscarCep);
        }

        public ICommand BuscarCepCommand { get; }

        private async void ExecuteBuscarCep(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Model.Cep)) return;
            if (Model.Cep == "01001-000")
            {
                Model.Logradouro = "Praça da Sé";
                Model.Bairro = "Sé";
                Model.Cidade = "São Paulo";
                Model.Uf = "SP";
                OnPropertyChanged(nameof(Model));
            }
        }

        protected override void ExecuteSalvar(object parameter)
        {
            if (!ValidarContexto()) return;
            if (string.IsNullOrWhiteSpace(Model.Nome))
            {
                MessageBox.Show("O campo 'Nome' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (!IsEdicao) { Model.Id = null; Model.DataCadastro = DateTime.Now; }
                if (IsEdicao) _context!.Clientes.Update(Model);
                else _context!.Clientes.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Cliente salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("ClienteList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "o cliente"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("ClienteList");
    }
}
