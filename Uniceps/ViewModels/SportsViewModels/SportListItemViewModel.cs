using Uniceps.Commands;
using Uniceps.Commands.Sport;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.Core.Models.Sport;
using Uniceps.Views.SportViews;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class SportListItemViewModel : ViewModelBase
    {
        public Sport Sport;
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

        public SportListItemViewModel(Sport sport, SportDataStore sportStore)
        {
            Sport = sport;
            _sportStore = sportStore;

            EditCommand = new RelayCommand(ExecuteEditSportCommand);
            DeleteCommand = new DeleteSportCommand(_sportStore);
          }
        public void ExecuteEditSportCommand()
        {
            AddSportViewModel editSportViewModel = new AddSportViewModel(_sportStore!, Sport);
            SportDetailWindowView sportDetailWindowView = new SportDetailWindowView();
            sportDetailWindowView.DataContext = editSportViewModel;
            sportDetailWindowView.ShowDialog();
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
