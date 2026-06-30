using System;
using System.Windows;
using PDV.Models.Pdv;
using PDV.Services;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class FormaPagamentoFormViewModel : FormViewModelBase<PdvTipoPagamento>
    {
        public FormaPagamentoFormViewModel() : this(null, null, null) { }

        public FormaPagamentoFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, PdvTipoPagamento? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model) { }

        protected override void ExecuteSalvar(object parameter)
        {
            if (!ValidarContexto()) return;
            if (string.IsNullOrWhiteSpace(Model.Descricao))
            {
                MessageBox.Show("O campo 'Descrição' é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (!IsEdicao) Model.Id = null;
                if (IsEdicao) _context!.PdvTiposPagamento.Update(Model);
                else _context!.PdvTiposPagamento.Add(Model);
                _context!.SaveChanges();
                MessageBox.Show("Forma de pagamento salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("FormaPagamentoList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "a forma de pagamento"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("FormaPagamentoList");
    }
}
