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
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Views.RoutineTemplateViews
{
    /// <summary>
    /// Interaction logic for ExportDialogWindow.xaml
    /// </summary>
    public partial class ExportDialogWindow : Window
    {
        public ExportDialogWindow()
        {
            InitializeComponent();
            this.DataContextChanged += ExportDialogWindow_DataContextChanged;
        }

        private void ExportDialogWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ExportDialogViewModel;
            if (vm != null)
                vm.RoutineExported += () => this.Close();
        }
    }
}
