using Unicepse.Commands;
using Unicepse.Commands.RoutinesCommand;
using Unicepse.ViewModels.PrintViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.navigation.Stores;
using Unicepse.utlis.common;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.navigation;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class EditSelectRoutineDaysMuscleGroupViewModel : ErrorNotifyViewModelBase
    {
        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;
        public IEnumerable<DayGroupListItemViewModel> DayGroupList => _dayGroupListItemViewModels;

        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        public EditSelectRoutineDaysMuscleGroupViewModel(RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, PlayersDataStore playersDataStore)
        {
            _routineDataStore = routineDataStore;
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
            _navigationService = navigationService;
            _playersDataStore = playersDataStore;
            foreach (var key in _routineDataStore.SelectedRoutine!.DaysGroupMap!)
            {
                DayGroupListItemViewModel? dayGroupListItemViewModel = new(key.Key, _routineDataStore);
                if (dayGroupListItemViewModel != null)
                {
                    dayGroupListItemViewModel.Groups = key.Value;
                    _routineDataStore.AddDaysItem(dayGroupListItemViewModel);
                }
            }
            Number = _routineDataStore.SelectedRoutine.RoutineNo;
            Date = _routineDataStore.SelectedRoutine.RoutineData;
            SubmitCommand = new UpdateRoutineCommand(_routineDataStore, _playersDataStore, _navigationService, this);
            string filename = _playersDataStore.SelectedPlayer!.FullName + "_" + Date.ToShortDateString() + "_Routine";
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new EditRoutinePrintViewModel(_routineDataStore, _playersDataStore, this), new NavigationStore()), filename);
            AddDaysCommand = new AddDaysCommand(_routineDataStore);
        }

        private void _routineDataStore_daysItemDeleted(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Remove(obj);
        }

        private void _routineDataStore_daysItemCreated(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Add(obj);
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        private string? _number;
        public string? Number
        {
            get { return _number; }
            set
            {
                _number = value; OnPropertyChanged(nameof(Number)); ClearError(nameof(Number));
                if (string.IsNullOrEmpty(Number))
                {
                    AddError("هذا الحقل مطلوب", nameof(Number));
                    OnErrorChanged(nameof(Number));
                }
            }
        }
        private bool _isTemplate;
        public bool IsTemplate
        {
            get { return _isTemplate; }
            set
            {
                _isTemplate = value;
                OnPropertyChanged(nameof(IsTemplate));

            }
        }

        public override void Dispose()
        {
            _routineDataStore.daysItemCreated -= _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted -= _routineDataStore_daysItemDeleted;
            base.Dispose();
        }
        public ICommand SubmitCommand { get; }
        public ICommand PrintCommand { get; }
        public ICommand AddDaysCommand { get; }
    }
}
