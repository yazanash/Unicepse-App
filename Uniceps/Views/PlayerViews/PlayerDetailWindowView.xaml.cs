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
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Views.PlayerViews
{
    /// <summary>
    /// Interaction logic for PlayerDetailWindowView.xaml
    /// </summary>
    public partial class PlayerDetailWindowView : Window
    {
        public PlayerDetailWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += PlayerDetailWindowView_DataContextChanged;
        }

        private void PlayerDetailWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddPlayerViewModel;
            if (vm != null)
                vm.PlayerCreated += () => this.Close();
            var editVm = DataContext as EditPlayerViewModel;
            if (editVm != null)
                editVm.PlayerUpdated += () => this.Close();
        }
    }
}
