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
    public class FornecedorFormViewModel : FormViewModelBase<Fornecedor>
    {
        public FornecedorFormViewModel() : this(null, null, null) { }

        public FornecedorFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, Fornecedor? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model)
        {
            BuscarCepCommand = new RelayCommand<object>(ExecuteBuscarCep);
        }

        public ICommand BuscarCepCommand { get; }

        private void ExecuteBuscarCep(object parameter)
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
                MessageBox.Show("O campo 'Razão Social' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (!IsEdicao) { Model.Id = null; Model.DataCadastro = DateTime.Now; }
                if (IsEdicao) _context!.Fornecedores.Update(Model);
                else _context!.Fornecedores.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Fornecedor salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("FornecedorList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "o fornecedor"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("FornecedorList");
    }
}
