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
using Unicepse.navigation;
using Unicepse.Commands.Player;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class SelectRoutineDaysMuscleGroupViewModel : ErrorNotifyViewModelBase
    {
        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;
        public IEnumerable<DayGroupListItemViewModel> DayGroupList => _dayGroupListItemViewModels;

        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        private readonly NavigationStore _navigationStore;
        private readonly LicenseDataStore _licenseDataStore;
        public SelectRoutineDaysMuscleGroupViewModel(RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, PlayersDataStore playersDataStore, AddRoutineViewModel addRoutineViewModel, NavigationStore navigationStore, LicenseDataStore licenseDataStore)
        {
            _routineDataStore = routineDataStore;
            _navigationStore = navigationStore;
            _licenseDataStore = licenseDataStore;
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
            _navigationService = navigationService;
            _playersDataStore = playersDataStore;
            if (_routineDataStore.DaysItems.Count() > 0)
            {
                _dayGroupListItemViewModels.Clear();
                _routineDataStore.ReloadDaysItems();
            }
            AddDaysCommand = new AddDaysCommand(_routineDataStore);
            SubmitCommand = new SubmitRoutineCommand(_routineDataStore, _playersDataStore, _navigationService, this);
            string filename = _playersDataStore.SelectedPlayer!.FullName + "_" + Date.ToShortDateString() + "_Routine";
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new RoutinePrintViewModel(_routineDataStore, _playersDataStore, this,_licenseDataStore), new NavigationStore()), filename);
            CancelCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => addRoutineViewModel));
           
        }
        public SelectRoutineDaysMuscleGroupViewModel(RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, PlayersDataStore playersDataStore, bool isTemp, AddRoutineViewModel addRoutineViewModel, NavigationStore navigationStore, LicenseDataStore licenseDataStore)
        {
            _routineDataStore = routineDataStore;
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            _navigationService = navigationService;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _licenseDataStore = licenseDataStore;
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
            if (_routineDataStore.SelectedRoutine != null)
            {
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
            }
            
            //if (_routineDataStore.DaysItems.Count()>0)
            //{
            //    foreach (var key in _routineDataStore.DaysItems)
            //    {
            //            _routineDataStore.AddDaysItem(key);
            //    }
            //}
            //if(_routineDataStore.SelectedRoutine!=null)
            //    Number = _routineDataStore.SelectedRoutine.RoutineNo;

            AddDaysCommand = new AddDaysCommand(_routineDataStore);
            SubmitCommand = new SubmitRoutineCommand(_routineDataStore, _playersDataStore, _navigationService, this);
            string filename = _playersDataStore.SelectedPlayer!.FullName + "_" + Date.ToShortDateString() + "_Routine";
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new RoutinePrintViewModel(_routineDataStore, _playersDataStore, this,_licenseDataStore), new NavigationStore()), filename);
            CancelCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => addRoutineViewModel));
           
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
                _number = value; OnPropertyChanged(nameof(Number));
                ClearError(nameof(Number));
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
        public ICommand CancelCommand { get; }

    }
}
