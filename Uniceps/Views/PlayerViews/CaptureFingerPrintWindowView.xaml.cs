using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Views.PlayerViews
{
    /// <summary>
    /// Interaction logic for CaptureFingerPrintWindowView.xaml
    /// </summary>
    public partial class CaptureFingerPrintWindowView : Window
    {
        public CaptureFingerPrintWindowView()
        {
            InitializeComponent();
            DataContextChanged += CaptureFingerPrintWindowView_DataContextChanged;
        }

        private void CaptureFingerPrintWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(DataContext is EnrollFingerprintViewModel enrollFingerprintViewModel)
            {
                enrollFingerprintViewModel.RequestClose += () =>
                {
                    Dispatcher.Invoke(() => Close());
                };
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is EnrollFingerprintViewModel enrollFingerprintViewModel)
            {
                enrollFingerprintViewModel.Dispose();
            }
            base.OnClosing(e);
        }
    }
}
