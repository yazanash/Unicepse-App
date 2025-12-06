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
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Views.AuthView
{
    /// <summary>
    /// Interaction logic for AddUserViewWindow.xaml
    /// </summary>
    public partial class AddUserViewWindow : Window
    {
        public AddUserViewWindow()
        {
            InitializeComponent();
            this.DataContextChanged += AddUserViewWindow_DataContextChanged; ;
        }
        private void AddUserViewWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddUserViewModel;
            if (vm != null)
                vm.UserCreated += () => this.Close();
            var editVm = DataContext as EditUserViewModel;
            if (editVm != null)
                editVm.UserUpdated += () => this.Close();
        }
    }
}
