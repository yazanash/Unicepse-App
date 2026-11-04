using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Uniceps.ViewModels.Metrics;

namespace Uniceps.Views.Metrics
{
    /// <summary>
    /// Interaction logic for AddMetricWindowView.xaml
    /// </summary>
    public partial class AddMetricWindowView : Window
    {
        public AddMetricWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += AddMetricWindowView_DataContextChanged;
        }

        private void AddMetricWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is AddMetricsViewModel vm)
            {
                vm.OnSaveAction += () => Close();
            }
        }
        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !textBox.IsKeyboardFocusWithin)
            {
                textBox.Focus();
                e.Handled = true;
                textBox.SelectAll();
            }
        }
    }
}
