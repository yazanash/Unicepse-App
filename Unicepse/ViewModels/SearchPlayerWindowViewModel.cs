using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.navigation.Stores;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.PlayersAttendenceViewModels;

namespace Unicepse.ViewModels
{
    public class SearchPlayerWindowViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public ViewModelBase? CurrentPrint => _navigatorStore.CurrentViewModel;

        public SearchPlayerWindowViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigatorStore)
        {
            _navigatorStore = navigatorStore;
            _playersDataStore = playersDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            //_navigatorStore.CurrentViewModel = CreatePlayerSearchViewModel(_playersDataStore, _playersAttendenceStore);
            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;
        }
      
        private void _navigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPrint));
        }
    }
}
