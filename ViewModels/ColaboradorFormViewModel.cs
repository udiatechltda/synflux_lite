using System;
using System.Windows;
using PDV.Models.Pdv.Cadastros;
using PDV.Services;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class ColaboradorFormViewModel : FormViewModelBase<Colaborador>
    {
        public ColaboradorFormViewModel() : this(null, null, null) { }

        public ColaboradorFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, Colaborador? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model) { }

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
                if (!IsEdicao) Model.Id = null;
                if (IsEdicao) _context!.Colaboradores.Update(Model);
                else _context!.Colaboradores.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Colaborador salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("ColaboradorList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "o colaborador"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("ColaboradorList");
    }
}
