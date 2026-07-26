using System;
using System.Collections.Generic;
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
using Uniceps.ViewModels.PaymentsViewModels;

namespace Uniceps.Views.PaymentViews
{
    /// <summary>
    /// Interaction logic for PaymentDetailWindowView.xaml
    /// </summary>
    public partial class PaymentDetailWindowView : Window
    {
        public PaymentDetailWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += PaymentDetailWindowView_DataContextChanged;
        }

        private void PaymentDetailWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
          if(DataContext is AddPaymentViewModel vm)
            {
                vm.RequestClose += () => Close();
            }
        }
    }
}
