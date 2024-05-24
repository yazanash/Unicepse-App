using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGymPro.Commands.PlayerAttendenceCommands;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersAttendenceViewModels
{
    public class PlayersAttendenceViewModel :ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;


        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;

        public PlayersAttendenceViewModel(PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded; 
            LoadDailyReport = new GetLoggedPlayerCommand(_playersAttendenceStore);

        }
        public ICommand LoadDailyReport { get; }
        private void _playersAttendenceStore_Loaded()
        {
            _playerAttendenceListItemViewModels.Clear();

            foreach (DailyPlayerReport dailyPlayerReport in _playersAttendenceStore.PlayersAttendence)
            {
                AddDailyPlayerLog(dailyPlayerReport);
            }
        }
        private void AddDailyPlayerLog(DailyPlayerReport dailyPlayerReport)
        {
            PlayerAttendenceListItemViewModel itemViewModel =
             new PlayerAttendenceListItemViewModel(dailyPlayerReport, _playersAttendenceStore);
            _playerAttendenceListItemViewModels.Add(itemViewModel);
        }
    }
}
