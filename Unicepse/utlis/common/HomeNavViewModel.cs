using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.navigation.Stores;
using Unicepse.Stores;

namespace Unicepse.utlis.common
{
    public class HomeNavViewModel:ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly EmployeeStore _employeeStore;

      public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public HomeNavViewModel(NavigationStore navigatorStore, PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore)
        {
            _navigatorStore = navigatorStore;
            _playersDataStore = playersDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _employeeStore = employeeStore;
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _navigatorStore.CurrentViewModel = CreateHomeViewModel(playersDataStore, _playersAttendenceStore, _employeeStore, _navigatorStore);

            
          
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private HomeViewModel CreateHomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, NavigationStore navigationStore)
        {
            return HomeViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, employeeStore, navigationStore);
        }
    }
}
