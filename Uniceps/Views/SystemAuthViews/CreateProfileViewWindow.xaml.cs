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
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Views.SystemAuthViews
{
    /// <summary>
    /// Interaction logic for CreateProfileViewWindow.xaml
    /// </summary>
    public partial class CreateProfileViewWindow : Window
    {
        public CreateProfileViewWindow()
        {
            InitializeComponent();
            this.DataContextChanged += CreateProfileViewWindow_DataContextChanged; ;
        }

        private void CreateProfileViewWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as SystemProfileCreationViewModel;
            if (vm != null)
                vm.ProfileCreatedAction += () => this.Close();
        }
    }
}
