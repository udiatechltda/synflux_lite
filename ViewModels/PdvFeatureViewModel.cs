using PDV.Models;
using PDV.Services.Interfaces;
using PDV.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class PdvFeatureViewModel : ViewModelBase
    {
        private readonly IPdvFeatureService? _featureService;
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IAlertService? _alertService;
        private readonly string _modulo;

        public ObservableCollection<PdvScreenDefinition> Telas { get; } = new();
        public ObservableCollection<PdvFeatureRow> Linhas { get; } = new();

        private PdvScreenDefinition? _telaSelecionada;
        public PdvScreenDefinition? TelaSelecionada
        {
            get => _telaSelecionada;
            set
            {
                if (SetProperty(ref _telaSelecionada, value))
                    CarregarLinhas();
            }
        }

        private PdvFeatureRow? _linhaSelecionada;
        public PdvFeatureRow? LinhaSelecionada
        {
            get => _linhaSelecionada;
            set => SetProperty(ref _linhaSelecionada, value);
        }

        public string ModuloTitulo { get; }
        public string TotalTelas => $"{Telas.Count} telas";
        public string TotalRegistros => $"{Linhas.Count} registro(s)";
        public bool MostrarBotaoRecarregar =>
            _modulo != "CadastrosPlus" &&
            _modulo != "Tributacao" &&
            _modulo != "Fiscal";

        private bool _mostrarCodigo = true;
        public bool MostrarCodigo
        {
            get => _mostrarCodigo;
            private set => SetProperty(ref _mostrarCodigo, value);
        }

        private bool _mostrarNome = true;
        public bool MostrarNome
        {
            get => _mostrarNome;
            private set => SetProperty(ref _mostrarNome, value);
        }

        private bool _mostrarDescricao = true;
        public bool MostrarDescricao
        {
            get => _mostrarDescricao;
            private set => SetProperty(ref _mostrarDescricao, value);
        }

        private bool _mostrarStatus = true;
        public bool MostrarStatus
        {
            get => _mostrarStatus;
            private set => SetProperty(ref _mostrarStatus, value);
        }

        private bool _mostrarValor = true;
        public bool MostrarValor
        {
            get => _mostrarValor;
            private set => SetProperty(ref _mostrarValor, value);
        }

        private bool _mostrarData = true;
        public bool MostrarData
        {
            get => _mostrarData;
            private set => SetProperty(ref _mostrarData, value);
        }

        public ICommand VoltarCommand { get; }
        public ICommand NovoCommand { get; }
        public ICommand AtualizarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand RecarregarCommand { get; }

        protected PdvFeatureViewModel(
            string modulo,
            string moduloTitulo,
            IPdvFeatureService featureService,
            IViewModelNavigationService navigationService,
            IAlertService alertService)
        {
            _modulo = modulo;
            ModuloTitulo = moduloTitulo;
            _featureService = featureService;
            _navigationService = navigationService;
            _alertService = alertService;

            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            NovoCommand = new RelayCommand(Novo);
            AtualizarCommand = new RelayCommand(Atualizar);
            ExcluirCommand = new RelayCommand(Excluir);
            RecarregarCommand = new RelayCommand(CarregarLinhas);

            CarregarTelas();
        }

        public PdvFeatureViewModel()
        {
            _modulo = string.Empty;
            ModuloTitulo = "PDV";
            VoltarCommand = new RelayCommand(() => { });
            NovoCommand = new RelayCommand(() => { });
            AtualizarCommand = new RelayCommand(() => { });
            ExcluirCommand = new RelayCommand(() => { });
            RecarregarCommand = new RelayCommand(() => { });
        }

        private void CarregarTelas()
        {
            if (_featureService == null)
                return;

            Telas.Clear();
            foreach (var tela in _featureService.ListarTelas(_modulo))
                Telas.Add(tela);

            TelaSelecionada = Telas.FirstOrDefault();
            OnPropertyChanged(nameof(TotalTelas));
        }

        private void CarregarLinhas()
        {
            Linhas.Clear();
            if (_featureService == null || TelaSelecionada == null)
                return;

            AplicarLayoutListagem(_featureService.ObterLayoutListagem(TelaSelecionada.Key));

            foreach (var linha in _featureService.CarregarLinhas(TelaSelecionada.Key))
                Linhas.Add(linha);

            LinhaSelecionada = Linhas.FirstOrDefault();
            OnPropertyChanged(nameof(TotalRegistros));
        }

        private void AplicarLayoutListagem(PdvFeatureColumnLayout layout)
        {
            MostrarCodigo = layout.MostrarCodigo;
            MostrarNome = layout.MostrarNome;
            MostrarDescricao = layout.MostrarDescricao;
            MostrarStatus = layout.MostrarStatus;
            MostrarValor = layout.MostrarValor;
            MostrarData = layout.MostrarData;
        }

        private void Novo()
        {
            if (_featureService == null || TelaSelecionada == null)
                return;

            try
            {
                var model = _featureService.CriarModeloEdicao(TelaSelecionada.Key);
                var dialog = new PdvFeatureEditDialog(model)
                {
                    Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                };

                if (dialog.ShowDialog() != true)
                    return;

                _featureService.SalvarRegistro(TelaSelecionada.Key, null, dialog.Model.ToDictionary());
                CarregarLinhas();
                _alertService?.ShowAlert("Registro criado.", AlertType.Success);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void Atualizar()
        {
            if (_featureService == null || TelaSelecionada == null || LinhaSelecionada?.Id == null)
                return;

            try
            {
                var model = _featureService.CriarModeloEdicao(TelaSelecionada.Key, LinhaSelecionada.Id.Value);
                var dialog = new PdvFeatureEditDialog(model)
                {
                    Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                };

                if (dialog.ShowDialog() != true)
                    return;

                _featureService.SalvarRegistro(TelaSelecionada.Key, LinhaSelecionada.Id.Value, dialog.Model.ToDictionary());
                CarregarLinhas();
                _alertService?.ShowAlert("Registro atualizado.", AlertType.Success);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void Excluir()
        {
            if (_featureService == null || TelaSelecionada == null || LinhaSelecionada?.Id == null)
                return;

            try
            {
                _featureService.ExcluirRegistro(TelaSelecionada.Key, LinhaSelecionada.Id.Value);
                CarregarLinhas();
                _alertService?.ShowAlert("Registro excluido.", AlertType.Success);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }
    }

    public class CadastrosPlusViewModel : PdvFeatureViewModel
    {
        public CadastrosPlusViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("CadastrosPlus", "Cadastros Complementares", s, n, a) { }
        public CadastrosPlusViewModel() { }
    }

    public class MovimentoPlusViewModel : PdvFeatureViewModel
    {
        public MovimentoPlusViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("MovimentoPlus", "Movimento Completo", s, n, a) { }
        public MovimentoPlusViewModel() { }
    }

    public class PdvPlusViewModel : PdvFeatureViewModel
    {
        public PdvPlusViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("PdvPlus", "Operações Auxiliares do PDV", s, n, a) { }
        public PdvPlusViewModel() { }
    }

    public class FiscalViewModel : PdvFeatureViewModel
    {
        public FiscalViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("Fiscal", "Fiscal / NFC-e", s, n, a) { }
        public FiscalViewModel() { }
    }

    public class TributacaoViewModel : PdvFeatureViewModel
    {
        public TributacaoViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("Tributacao", "Tributação", s, n, a) { }
        public TributacaoViewModel() { }
    }

    public class FoodViewModel : PdvFeatureViewModel
    {
        public FoodViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("Food", "Food / Mesas / Comandas", s, n, a) { }
        public FoodViewModel() { }
    }

    public class DeliveryViewModel : PdvFeatureViewModel
    {
        public DeliveryViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("Delivery", "Delivery", s, n, a) { }
        public DeliveryViewModel() { }
    }

    public class RelatoriosViewModel : PdvFeatureViewModel
    {
        public RelatoriosViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("Relatorios", "Relatórios", s, n, a) { }
        public RelatoriosViewModel() { }
    }

    public class ConfiguracoesPlusViewModel : PdvFeatureViewModel
    {
        public ConfiguracoesPlusViewModel(IPdvFeatureService s, IViewModelNavigationService n, IAlertService a)
            : base("ConfiguracoesPlus", "Configurações Avançadas", s, n, a) { }
        public ConfiguracoesPlusViewModel() { }
    }
}
