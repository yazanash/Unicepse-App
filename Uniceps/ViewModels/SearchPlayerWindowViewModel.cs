using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersAttendenceViewModels;

namespace Uniceps.ViewModels
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
