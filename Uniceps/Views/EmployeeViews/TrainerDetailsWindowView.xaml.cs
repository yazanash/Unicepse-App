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
using Uniceps.ViewModels.SportsViewModels;

namespace Uniceps.Views.EmployeeViews
{
    /// <summary>
    /// Interaction logic for TrainerDetailsWindowView.xaml
    /// </summary>
    public partial class TrainerDetailsWindowView : Window
    {
        public TrainerDetailsWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += TrainerDetailsWindowView_DataContextChanged;
        }

        private void TrainerDetailsWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddTrainerViewModel;
            if (vm != null)
                vm.TrainerCreated += () => this.Close();
            var editVm = DataContext as EditTrainerViewModel;
            if (editVm != null)
                editVm.TrainerUpdated += () => this.Close();
        }
    }
}
