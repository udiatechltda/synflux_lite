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
    public class EmpresaFormViewModel : FormViewModelBase<Empresa>
    {
        public EmpresaFormViewModel() : this(null, null, null) { }

        public EmpresaFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, Empresa? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model)
        {
            BuscarCepCommand = new RelayCommand<object>(ExecuteBuscarCep);
            CarregarLogotipoCommand = new RelayCommand<object>(ExecuteCarregarLogotipo);
        }

        public ICommand BuscarCepCommand { get; }
        public ICommand CarregarLogotipoCommand { get; }

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

        private void ExecuteCarregarLogotipo(object parameter) { }

        protected override void ExecuteSalvar(object parameter)
        {
            if (!ValidarContexto()) return;
            if (string.IsNullOrWhiteSpace(Model.RazaoSocial))
            {
                MessageBox.Show("O campo 'Razão Social' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (!IsEdicao)
                {
                    Model.Id = null;
                    Model.DataRegistro = DateTime.Now;
                    Model.HoraRegistro = DateTime.Now.ToString("HH:mm:ss");
                }
                if (IsEdicao) _context!.Empresas.Update(Model);
                else _context!.Empresas.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Empresa salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("EmpresaList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "a empresa"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("EmpresaList");
    }
}
