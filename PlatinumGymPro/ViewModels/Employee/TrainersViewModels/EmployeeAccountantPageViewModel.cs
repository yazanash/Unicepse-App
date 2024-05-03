using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountantPageViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly EmployeeStore _employeeStore;

        public EmployeeAccountantPageViewModel(NavigationStore navigationStore, EmployeeStore employeeStore)
        {
            _navigationStore = navigationStore;
            _employeeStore = employeeStore;
        }
    }
}
