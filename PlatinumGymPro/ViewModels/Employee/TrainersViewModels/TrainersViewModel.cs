using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels
{
    public class TrainersViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public TrainersViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            navigatorStore.CurrentViewModel = CreateTrainerViewModel(_navigatorStore, _employeeStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private TrainersListViewModel CreateTrainerViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore)
        {
            return TrainersListViewModel.LoadViewModel(navigatorStore, employeeStore);
        }
    }
}
