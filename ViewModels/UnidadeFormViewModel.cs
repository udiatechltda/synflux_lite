using System;
using System.Windows;
using PDV.Models.Pdv.Cadastros;
using PDV.Services;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class UnidadeFormViewModel : FormViewModelBase<ProdutoUnidade>
    {
        public UnidadeFormViewModel() : this(null, null, null) { }

        public UnidadeFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, ProdutoUnidade? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model) { }

        protected override void ExecuteSalvar(object parameter)
        {
            if (!ValidarContexto()) return;
            if (string.IsNullOrWhiteSpace(Model.Sigla))
            {
                MessageBox.Show("O campo 'Sigla' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (!IsEdicao) Model.Id = null;
                if (IsEdicao) _context!.ProdutosUnidades.Update(Model);
                else _context!.ProdutosUnidades.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Unidade salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("UnidadeList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "a unidade"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("UnidadeList");
    }
}
