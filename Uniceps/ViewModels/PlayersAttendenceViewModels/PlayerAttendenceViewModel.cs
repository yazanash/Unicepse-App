using Uniceps.Commands.PlayerAttendenceCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.Core.Models.DailyActivity;

namespace Uniceps.ViewModels.PlayersAttendenceViewModels
{
    public class PlayerAttendenceViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;


        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;
        public int PlayerId;
        public PlayerAttendenceViewModel(int playerId,PlayersAttendenceStore playersAttendenceStore)
        {
            PlayerId = playerId;    
            _playersAttendenceStore = playersAttendenceStore;
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            _playersAttendenceStore.PlayerLoggingLoaded += _playersAttendenceStore_Loaded;
            LoadDailyReport = new GetPlayerLoggingCommand(_playersAttendenceStore);
            LoadDailyReport.Execute(PlayerId);
        }
        public ICommand LoadDailyReport { get; }
        private void _playersAttendenceStore_Loaded()
        {
            _playerAttendenceListItemViewModels.Clear();

            foreach (DailyPlayerReport dailyPlayerReport in _playersAttendenceStore.PlayersAttendence.OrderByDescending(x => x.Date).ThenByDescending(x => x.loginTime))
            {
                AddDailyPlayerLog(dailyPlayerReport);
            }
        }
        private void AddDailyPlayerLog(DailyPlayerReport dailyPlayerReport)
        {
            PlayerAttendenceListItemViewModel itemViewModel =
             new PlayerAttendenceListItemViewModel(dailyPlayerReport, _playersAttendenceStore);
            _playerAttendenceListItemViewModels.Add(itemViewModel);
            itemViewModel.IdSort = _playerAttendenceListItemViewModels.Count();
        }
        public override void Dispose()
        {

            _playersAttendenceStore.PlayerLoggingLoaded -= _playersAttendenceStore_Loaded;

            base.Dispose();
        }
    }
}
