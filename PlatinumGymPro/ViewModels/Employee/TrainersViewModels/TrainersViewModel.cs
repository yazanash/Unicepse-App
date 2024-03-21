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
        private readonly SportDataStore _sportDataStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public TrainersViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;

            navigatorStore.CurrentViewModel = CreateTrainerViewModel(_navigatorStore, _employeeStore,_sportDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private TrainersListViewModel CreateTrainerViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore,SportDataStore sportDataStore)
        {
            return TrainersListViewModel.LoadViewModel(navigatorStore, employeeStore, sportDataStore);
        }
    }
}
