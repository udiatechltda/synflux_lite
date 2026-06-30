using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PDV.Models.Pdv.Cadastros;
using PDV.Services;
using PDV.ViewModels.Base;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class ProdutoFormViewModel : FormViewModelBase<Produto>
    {
        private static readonly JsonSerializerOptions LocalizacaoJsonOptions = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly IProdutoImagemService? _produtoImagemService;
        private string? _imagemSelecionadaOrigem;
        private bool _removerImagem;
        private string? _imagemProdutoPath;
        private bool _carregandoLocalizacao;
        private string? _deposito;
        private string? _corredor;
        private string? _prateleira;
        private string? _nivel;
        private string? _posicao;
        private string? _enderecoCompleto;
        private string? _observacaoArmazenagem;
        private double? _pontoReposicao;
        private double? _estoqueReservado;
        private string? _tipoArmazenagem;
        private string? _observacoesEstoque;

        public ProdutoFormViewModel() : this(null, null, null, null) { }

        public ProdutoFormViewModel(IViewModelNavigationService? navigationService, PdvContext? context, Produto? model)
            : this(navigationService, context, null, model) { }

        public ProdutoFormViewModel(
            IViewModelNavigationService? navigationService,
            PdvContext? context,
            IProdutoImagemService? produtoImagemService = null,
            Produto? model = null)
            : base(navigationService ?? new PDV.Services.ViewModelNavigationService(), context, model)
        {
            _produtoImagemService = produtoImagemService;
            SelecionarImagemCommand = new PDV.Utilities.Converters.RelayCommand<object>(_ => ExecuteSelecionarImagem());
            RemoverImagemCommand = new PDV.Utilities.Converters.RelayCommand<object>(_ => ExecuteRemoverImagem());
            ImagemProdutoPath = _produtoImagemService?.ObterImagemLocalPath(Model.Id);
            CarregarLocalizacaoEstoque();
        }

        public ICommand SelecionarImagemCommand { get; }
        public ICommand RemoverImagemCommand { get; }
        public new string Titulo => IsEdicao ? "Editar Produto" : "Novo Produto";
        public string Subtitulo => "Dados comerciais, fiscais, estoque e localização";

        public string? Deposito
        {
            get => _deposito;
            set => SetProperty(ref _deposito, value);
        }

        public string? Corredor
        {
            get => _corredor;
            set
            {
                if (SetProperty(ref _corredor, value))
                    AtualizarEnderecoCompleto();
            }
        }

        public string? Prateleira
        {
            get => _prateleira;
            set
            {
                if (SetProperty(ref _prateleira, value))
                    AtualizarEnderecoCompleto();
            }
        }

        public string? Nivel
        {
            get => _nivel;
            set
            {
                if (SetProperty(ref _nivel, value))
                    AtualizarEnderecoCompleto();
            }
        }

        public string? Posicao
        {
            get => _posicao;
            set
            {
                if (SetProperty(ref _posicao, value))
                    AtualizarEnderecoCompleto();
            }
        }

        public string? EnderecoCompleto
        {
            get => _enderecoCompleto;
            set => SetProperty(ref _enderecoCompleto, value);
        }

        public string? ObservacaoArmazenagem
        {
            get => _observacaoArmazenagem;
            set => SetProperty(ref _observacaoArmazenagem, value);
        }

        public double? PontoReposicao
        {
            get => _pontoReposicao;
            set => SetProperty(ref _pontoReposicao, value);
        }

        public double? EstoqueReservado
        {
            get => _estoqueReservado;
            set => SetProperty(ref _estoqueReservado, value);
        }

        public string? TipoArmazenagem
        {
            get => _tipoArmazenagem;
            set => SetProperty(ref _tipoArmazenagem, value);
        }

        public string? ObservacoesEstoque
        {
            get => _observacoesEstoque;
            set => SetProperty(ref _observacoesEstoque, value);
        }

        public string? ImagemProdutoPath
        {
            get => _imagemProdutoPath;
            set
            {
                if (SetProperty(ref _imagemProdutoPath, value))
                    OnPropertyChanged(nameof(TemImagemProduto));
            }
        }

        public bool TemImagemProduto => !string.IsNullOrWhiteSpace(ImagemProdutoPath);

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
                SalvarLocalizacaoEstoque();
                if (!IsEdicao) Model.Id = null;
                if (IsEdicao) _context!.Produtos.Update(Model);
                else _context!.Produtos.Add(Model);
                _context!.SaveChanges();

                SalvarImagemProduto();

                MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo("ProdutoList");
            }
            catch (Exception ex) { MostrarErroBanco(ex, "o produto"); }
        }

        protected override void OnVoltar() => _navigationService.NavigateTo("ProdutoList");

        private void ExecuteSelecionarImagem()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Selecionar imagem do produto",
                Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
                return;

            _imagemSelecionadaOrigem = dialog.FileName;
            _removerImagem = false;
            ImagemProdutoPath = dialog.FileName;
        }

        private void ExecuteRemoverImagem()
        {
            _imagemSelecionadaOrigem = null;
            _removerImagem = true;
            ImagemProdutoPath = null;
        }

        private void SalvarImagemProduto()
        {
            if (_produtoImagemService == null || !Model.Id.HasValue)
                return;

            if (!string.IsNullOrWhiteSpace(_imagemSelecionadaOrigem))
            {
                _produtoImagemService.SalvarImagemProduto(Model.Id, _imagemSelecionadaOrigem);
                return;
            }

            if (_removerImagem)
                _produtoImagemService.RemoverImagemProduto(Model.Id);
        }

        private void CarregarLocalizacaoEstoque()
        {
            var localizacao = DesserializarLocalizacao(Model.Localizacao) ?? new ProdutoLocalizacaoEstoque();

            _carregandoLocalizacao = true;
            Deposito = localizacao.Deposito;
            Corredor = localizacao.Corredor;
            Prateleira = localizacao.Prateleira;
            Nivel = localizacao.Nivel;
            Posicao = localizacao.Posicao;
            EnderecoCompleto = localizacao.EnderecoCompleto;
            ObservacaoArmazenagem = localizacao.ObservacaoArmazenagem;
            PontoReposicao = localizacao.PontoReposicao;
            EstoqueReservado = localizacao.EstoqueReservado;
            TipoArmazenagem = localizacao.TipoArmazenagem;
            ObservacoesEstoque = localizacao.Observacoes;
            _carregandoLocalizacao = false;

            AtualizarEnderecoCompleto();
        }

        private void SalvarLocalizacaoEstoque()
        {
            var localizacao = new ProdutoLocalizacaoEstoque
            {
                Deposito = NormalizarTexto(Deposito),
                Corredor = NormalizarTexto(Corredor),
                Prateleira = NormalizarTexto(Prateleira),
                Nivel = NormalizarTexto(Nivel),
                Posicao = NormalizarTexto(Posicao),
                EnderecoCompleto = NormalizarTexto(EnderecoCompleto),
                ObservacaoArmazenagem = NormalizarTexto(ObservacaoArmazenagem),
                PontoReposicao = PontoReposicao,
                EstoqueReservado = EstoqueReservado,
                TipoArmazenagem = NormalizarTexto(TipoArmazenagem),
                Observacoes = NormalizarTexto(ObservacoesEstoque)
            };

            Model.Localizacao = TemLocalizacaoPreenchida(localizacao)
                ? JsonSerializer.Serialize(localizacao, LocalizacaoJsonOptions)
                : null;
        }

        private void AtualizarEnderecoCompleto()
        {
            if (_carregandoLocalizacao)
                return;

            EnderecoCompleto = string.Join("-",
                new[] { Corredor, Prateleira, Nivel, Posicao }
                    .Where(valor => !string.IsNullOrWhiteSpace(valor)));
        }

        private static string? NormalizarTexto(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool TemLocalizacaoPreenchida(ProdutoLocalizacaoEstoque localizacao)
        {
            return !string.IsNullOrWhiteSpace(localizacao.Deposito)
                || !string.IsNullOrWhiteSpace(localizacao.Corredor)
                || !string.IsNullOrWhiteSpace(localizacao.Prateleira)
                || !string.IsNullOrWhiteSpace(localizacao.Nivel)
                || !string.IsNullOrWhiteSpace(localizacao.Posicao)
                || !string.IsNullOrWhiteSpace(localizacao.EnderecoCompleto)
                || !string.IsNullOrWhiteSpace(localizacao.ObservacaoArmazenagem)
                || localizacao.PontoReposicao.HasValue
                || localizacao.EstoqueReservado.HasValue
                || !string.IsNullOrWhiteSpace(localizacao.TipoArmazenagem)
                || !string.IsNullOrWhiteSpace(localizacao.Observacoes);
        }

        private static ProdutoLocalizacaoEstoque? DesserializarLocalizacao(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                return JsonSerializer.Deserialize<ProdutoLocalizacaoEstoque>(value, LocalizacaoJsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private sealed class ProdutoLocalizacaoEstoque
        {
            public string? Deposito { get; set; }
            public string? Corredor { get; set; }
            public string? Prateleira { get; set; }
            public string? Nivel { get; set; }
            public string? Posicao { get; set; }
            public string? EnderecoCompleto { get; set; }
            public string? ObservacaoArmazenagem { get; set; }
            public double? PontoReposicao { get; set; }
            public double? EstoqueReservado { get; set; }
            public string? TipoArmazenagem { get; set; }
            public string? Observacoes { get; set; }
        }
    }
}
