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

namespace Uniceps.Views.RoutineTemplateViews.RoutineComponent
{
    /// <summary>
    /// Interaction logic for ExercisesListViewWindow.xaml
    /// </summary>
    public partial class ExercisesListViewWindow : Window
    {
        public ExercisesListViewWindow()
        {
            InitializeComponent();
            this.DataContextChanged += ExercisesListViewWindow_DataContextChanged;
        }

        private void ExercisesListViewWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as ExercisesListViewModel;
            if (vm != null)
                vm.RoutineItemsCreate += () => this.Close();
        }
    }
}
