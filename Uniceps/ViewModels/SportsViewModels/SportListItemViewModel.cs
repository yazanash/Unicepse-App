using Uniceps.Commands;
using Uniceps.Commands.Sport;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.Core.Models.Sport;
using Uniceps.Views.SportViews;
using Uniceps.navigation.DialogService;
using System.Collections.Generic;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class SportListItemViewModel : ViewModelBase
    {
        public Sport Sport;
        private IDialogService? _dialogService;
        private readonly SportDataStore? _sportStore;
        public int Id => Sport.Id;
        public string? SportName => Sport.Name;
        public double Price => Sport.Price;
        public bool IsActive => Sport.IsActive;
        public int DaysInWeek => Sport.DaysInWeek;

        public int DaysCount => Sport.DaysCount;

        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public ICommand? SubscriptionsCommand { get; }

        public SportListItemViewModel(Sport sport, SportDataStore sportStore, IDialogService dialogService)
        {
            Sport = sport;
            _sportStore = sportStore;

            EditCommand = new RelayCommand(ExecuteEditSportCommand);
            DeleteCommand = new DeleteSportCommand(_sportStore);
            _dialogService = dialogService;
        }
        public void ExecuteEditSportCommand()
        {
            if (_dialogService != null)
            {
                var navParams = new Dictionary<string, object>()
            {
                {"Sport",Sport }
            };
                _dialogService.ShowDialog<SportDetailWindowView, AddSportViewModel>(navParams);
            }
        }
        public SportListItemViewModel(Sport sport)
        {
            Sport = sport;
        }

        public void Update(Sport sport)
        {
            Sport = sport;

            OnPropertyChanged(nameof(Sport));
            OnPropertyChanged(nameof(SportName));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(DaysCount));
        }
    }
}
