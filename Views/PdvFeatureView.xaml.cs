using PDV.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PDV.Views
{
    public partial class PdvFeatureView : UserControl
    {
        private PdvFeatureViewModel? _viewModel;

        public PdvFeatureView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Loaded += (_, _) => AplicarVisibilidadeColunas();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;

            _viewModel = e.NewValue as PdvFeatureViewModel;

            if (_viewModel != null)
                _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            AplicarVisibilidadeColunas();
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(PdvFeatureViewModel.MostrarCodigo)
                or nameof(PdvFeatureViewModel.MostrarNome)
                or nameof(PdvFeatureViewModel.MostrarDescricao)
                or nameof(PdvFeatureViewModel.MostrarStatus)
                or nameof(PdvFeatureViewModel.MostrarValor)
                or nameof(PdvFeatureViewModel.MostrarData))
            {
                AplicarVisibilidadeColunas();
            }
        }

        private void AplicarVisibilidadeColunas()
        {
            if (_viewModel == null || CodigoColumn == null)
                return;

            CodigoColumn.Visibility = Visibilidade(_viewModel.MostrarCodigo);
            NomeColumn.Visibility = Visibilidade(_viewModel.MostrarNome);
            DescricaoColumn.Visibility = Visibilidade(_viewModel.MostrarDescricao);
            StatusColumn.Visibility = Visibilidade(_viewModel.MostrarStatus);
            ValorColumn.Visibility = Visibilidade(_viewModel.MostrarValor);
            DataColumn.Visibility = Visibilidade(_viewModel.MostrarData);
        }

        private static Visibility Visibilidade(bool visivel)
        {
            return visivel ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
