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
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Views.SportViews
{
    /// <summary>
    /// Interaction logic for SportDetailWindowView.xaml
    /// </summary>
    public partial class SportDetailWindowView : Window
    {
        public SportDetailWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += SportDetailWindowView_DataContextChanged;
        }

        private void SportDetailWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddSportViewModel;
            if (vm != null)
                vm.SportCreated += () => this.Close();
            var editVm = DataContext as EditSportViewModel;
            if (editVm != null)
                editVm.SportUpdated += () => this.Close();
        }
    }
}
