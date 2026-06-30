using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDV.Commands;

namespace PDV.Utilities.Behaviors
{
    public static class ButtonLoading
    {
        private static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached(
            "Adorner", typeof(LoadingAdorner), typeof(ButtonLoading));

        private static readonly DependencyProperty SubscribedCommandProperty = DependencyProperty.RegisterAttached(
            "SubscribedCommand", typeof(INotifyPropertyChanged), typeof(ButtonLoading));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", typeof(bool), typeof(ButtonLoading),
            new PropertyMetadata(false, OnIsEnabledChanged));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.RegisterAttached(
            "IsLoading", typeof(bool), typeof(ButtonLoading),
            new PropertyMetadata(false, OnIsLoadingChanged));

        public static void SetIsEnabled(DependencyObject element, bool value) => element.SetValue(IsEnabledProperty, value);
        public static bool GetIsEnabled(DependencyObject element) => (bool)element.GetValue(IsEnabledProperty);
        public static void SetIsLoading(DependencyObject element, bool value) => element.SetValue(IsLoadingProperty, value);
        public static bool GetIsLoading(DependencyObject element) => (bool)element.GetValue(IsLoadingProperty);

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button) Update(button);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Button button) return;

            button.Loaded -= ButtonLoaded;
            button.Unloaded -= ButtonUnloaded;
            DependencyPropertyDescriptor.FromProperty(Button.CommandProperty, typeof(Button))
                .RemoveValueChanged(button, ButtonCommandChanged);

            if ((bool)e.NewValue)
            {
                button.Loaded += ButtonLoaded;
                button.Unloaded += ButtonUnloaded;
                DependencyPropertyDescriptor.FromProperty(Button.CommandProperty, typeof(Button))
                    .AddValueChanged(button, ButtonCommandChanged);
                Subscribe(button, button.Command);
            }
            else
            {
                Unsubscribe(button, button.Command);
                RemoveAdorner(button);
            }
        }

        private static void ButtonLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) Update(button);
        }

        private static void ButtonUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button button) RemoveAdorner(button);
        }

        private static void ButtonCommandChanged(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;
            Subscribe(button, button.Command);
            Update(button);
        }

        private static void Subscribe(Button button, ICommand? command)
        {
            if (button.GetValue(SubscribedCommandProperty) is INotifyPropertyChanged previous)
                previous.PropertyChanged -= CommandPropertyChanged;

            if (command is INotifyPropertyChanged current)
            {
                current.PropertyChanged += CommandPropertyChanged;
                button.SetValue(SubscribedCommandProperty, current);
            }
            else
                button.ClearValue(SubscribedCommandProperty);
        }

        private static void Unsubscribe(Button button, ICommand? command)
        {
            if (button.GetValue(SubscribedCommandProperty) is INotifyPropertyChanged current)
                current.PropertyChanged -= CommandPropertyChanged;
            button.ClearValue(SubscribedCommandProperty);
        }

        private static void CommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AsyncRelayCommand.IsRunning)) return;

            foreach (Window window in Application.Current.Windows)
            {
                foreach (var button in FindVisualChildren<Button>(window))
                {
                    if (ReferenceEquals(button.Command, sender) && GetIsEnabled(button))
                        Update(button);
                }
            }
        }

        private static void Update(Button button)
        {
            var isLoading = GetIsLoading(button) ||
                            button.Command is AsyncRelayCommand command && command.IsRunning;
            button.IsHitTestVisible = !isLoading;

            if (isLoading)
                AddAdorner(button);
            else
                RemoveAdorner(button);
        }

        private static void AddAdorner(Button button)
        {
            if (button.GetValue(AdornerProperty) is LoadingAdorner) return;
            var layer = AdornerLayer.GetAdornerLayer(button);
            if (layer == null) return;

            var adorner = new LoadingAdorner(button);
            button.SetValue(AdornerProperty, adorner);
            layer.Add(adorner);
        }

        private static void RemoveAdorner(Button button)
        {
            if (button.GetValue(AdornerProperty) is not LoadingAdorner adorner) return;
            AdornerLayer.GetAdornerLayer(button)?.Remove(adorner);
            button.ClearValue(AdornerProperty);
            button.IsHitTestVisible = true;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject root) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T match) yield return match;
                foreach (var descendant in FindVisualChildren<T>(child)) yield return descendant;
            }
        }

        private sealed class LoadingAdorner : Adorner
        {
            private readonly Grid _overlay;
            private VisualCollection _visuals;

            public LoadingAdorner(UIElement adornedElement) : base(adornedElement)
            {
                IsHitTestVisible = false;

                _visuals = new VisualCollection(this);

                var spinner = new Ellipse
                {
                    Width = 18,
                    Height = 18,
                    Stroke = Brushes.White,
                    StrokeThickness = 2.5,
                    StrokeDashArray = new DoubleCollection { 1, 2 },
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform()
                };

                ((RotateTransform)spinner.RenderTransform).BeginAnimation(
                    RotateTransform.AngleProperty,
                    new DoubleAnimation(0, 360, TimeSpan.FromMilliseconds(700))
                    {
                        RepeatBehavior = RepeatBehavior.Forever
                    });

                _overlay = new Grid
                {
                    Background = new SolidColorBrush(Color.FromArgb(150, 28, 62, 114))
                };

                _overlay.Children.Add(spinner);

                _visuals.Add(_overlay);
            }

            protected override int VisualChildrenCount => _visuals?.Count ?? 0;

            protected override Visual GetVisualChild(int index)
            {
                return _visuals[index];
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                _overlay.Arrange(new Rect(finalSize));
                return finalSize;
            }
        }
    }
}
