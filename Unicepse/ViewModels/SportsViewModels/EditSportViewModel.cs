using Unicepse.Core.Models.Employee;
using Unicepse.Commands;
using Unicepse.Commands.Sport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.ViewModels;
using Unicepse.Stores;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.SportsViewModels
{
    public class EditSportViewModel : ListingViewModelBase, INotifyDataErrorInfo
    {
        private readonly NavigationStore _navigationStore;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _trainerStore;
        private readonly ObservableCollection<TrainersListItemViewModel> trainerListItemViewModels;
        public IEnumerable<TrainersListItemViewModel> TrainerList => trainerListItemViewModels;
        public EditSportViewModel(NavigationStore navigationStore, SportListViewModel sportListViewModel, SportDataStore sportStore, EmployeeStore trainerStore)
        {
            _navigationStore = navigationStore;
            _sportStore = sportStore;
            _trainerStore = trainerStore;
            CancelCommand = new NavaigateCommand<SportListViewModel>(new NavigationService<SportListViewModel>(_navigationStore, () => sportListViewModel));
            SubmitCommand = new EditSportCommand(new NavigationService<SportListViewModel>(_navigationStore, () => sportListViewModel), this, _sportStore);
            LoadTrainersCommand = new LoadTrainersForSportCommand(_trainerStore, this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            trainerListItemViewModels = new ObservableCollection<TrainersListItemViewModel>();
            _sportStore = sportStore;
            _trainerStore = trainerStore;
            _trainerStore.Loaded += _trainerStore_TrainersLoaded;

            SportName = _sportStore.SelectedSport!.Name;
            DailyPrice = _sportStore.SelectedSport!.DailyPrice;
            SubscribeLength = _sportStore.SelectedSport!.DaysCount;
            WeeklyTrainingDays = _sportStore.SelectedSport!.DaysInWeek;
            MonthlyPrice = _sportStore.SelectedSport!.Price;
        }



        private void _trainerStore_TrainersLoaded()
        {
            trainerListItemViewModels.Clear();

            foreach (emp.Employee trainer in _trainerStore.Employees.Where(x => x.IsTrainer))
            {

                AddTrainer(trainer);


            }
            foreach (var t in _sportStore.SelectedSport!.Trainers!)
            {
                TrainerList.SingleOrDefault(x => x.trainer.Id == t.Id)!.IsSelected = true;
            }
        }
        private void AddTrainer(emp.Employee trainer)
        {

            TrainersListItemViewModel itemViewModel =
                new TrainersListItemViewModel(trainer);
            if (_sportStore.SelectedSport != null)
            {
                if (_sportStore.SelectedSport!.Trainers!.Where(x => x.Id == trainer.Id).Any())
                    itemViewModel.IsSelected = true;

            }
            trainerListItemViewModels.Add(itemViewModel);
        }

        public int Id { get; }

        private string? _sportName;
        public string? SportName
        {
            get { return _sportName; }
            set
            {
                _sportName = value;
                OnPropertyChanged(nameof(SportName));
                ClearError(nameof(SportName));
                if (string.IsNullOrEmpty(SportName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(SportName));
                    OnErrorChanged(nameof(SportName));
                }
            }
        }
        private double _monthlyPrice;
        public double MonthlyPrice
        {
            get { return _monthlyPrice; }
            set
            {
                _monthlyPrice = value; OnPropertyChanged(nameof(MonthlyPrice));
                ClearError(nameof(MonthlyPrice));
                if (MonthlyPrice < 0)
                {
                    AddError("لايمكن ان تكون القيمة اقل من 0", nameof(MonthlyPrice));
                    OnErrorChanged(nameof(MonthlyPrice));
                }
            }
        }

        private void AddError(string? ErrorMsg, string? propertyName)
        {
            if (!PropertyNameToErrorsDictionary.ContainsKey(propertyName!))
            {
                PropertyNameToErrorsDictionary.Add(propertyName!, new List<string>());

            }
            PropertyNameToErrorsDictionary[propertyName!].Add(ErrorMsg!);
            OnErrorChanged(propertyName);
        }

        private void ClearError(string? propertyName)
        {
            PropertyNameToErrorsDictionary.Remove(propertyName!);
            OnErrorChanged(propertyName);
        }
        public bool CanSubmit => !HasErrors;
        private void OnErrorChanged(string? PropertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(PropertyName));
            OnPropertyChanged(nameof(CanSubmit));
        }

        private double _dailyPrice;
        public double DailyPrice
        {
            get { return _dailyPrice; }
            set
            {
                _dailyPrice = value; OnPropertyChanged(nameof(DailyPrice));
                ClearError(nameof(DailyPrice));
                if (MonthlyPrice < 0)
                {
                    AddError("لايمكن ان تكون القيمة اقل من 0", nameof(DailyPrice));
                    OnErrorChanged(nameof(DailyPrice));
                }
            }
        }
        private int _weeklyTrainingDays;
        public int WeeklyTrainingDays
        {
            get { return _weeklyTrainingDays; }
            set
            {
                _weeklyTrainingDays = value;
                OnPropertyChanged(nameof(WeeklyTrainingDays));
                ClearError(nameof(WeeklyTrainingDays));
                if (WeeklyTrainingDays == 0)
                {
                    AddError("لا يمكن ان يكون هذا الرقم اقل من 1", nameof(WeeklyTrainingDays));
                    OnErrorChanged(nameof(WeeklyTrainingDays));
                }
            }
        }
        private int _subscribeLength;
        public int SubscribeLength
        {
            get { return _subscribeLength; }
            set
            {
                _subscribeLength = value;
                OnPropertyChanged(nameof(SubscribeLength));
                ClearError(nameof(SubscribeLength));
                if (SubscribeLength == 0)
                {
                    AddError("لا يمكن ان يكون هذا الرقم اقل من 1", nameof(SubscribeLength));
                    OnErrorChanged(nameof(SubscribeLength));
                }
            }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        public ICommand LoadTrainersCommand { get; }

        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }
        public static EditSportViewModel LoadViewModel(NavigationStore navigatorStore, SportListViewModel sportListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            EditSportViewModel viewModel = new EditSportViewModel(navigatorStore, sportListViewModel, sportDataStore, employeeStore);

            viewModel.LoadTrainersCommand.Execute(null);

            return viewModel;
        }
    }

}
