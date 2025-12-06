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
        private readonly PlayersDataStore _playersDataStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;

        public PlayerAttendenceViewModel(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;

            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            _playersAttendenceStore.PlayerLoggingLoaded += _playersAttendenceStore_Loaded;
            LoadDailyReport = new GetPlayerLoggingCommand(_playersAttendenceStore, _playersDataStore);
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
        public static PlayerAttendenceViewModel LoadViewModel(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore)
        {
            PlayerAttendenceViewModel viewModel = new(playersAttendenceStore, playersDataStore);

            viewModel.LoadDailyReport.Execute(null);
            return viewModel;
        }
        public override void Dispose()
        {

            _playersAttendenceStore.PlayerLoggingLoaded -= _playersAttendenceStore_Loaded;

            base.Dispose();
        }
    }
}
