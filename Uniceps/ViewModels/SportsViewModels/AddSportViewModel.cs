using Uniceps.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.Commands.Sport;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class AddSportViewModel : ListingViewModelBase, INotifyDataErrorInfo
    {
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _trainerStore;
        private readonly ObservableCollection<TrainersListItemViewModel> trainerListItemViewModels;
        public IEnumerable<TrainersListItemViewModel> TrainerList => trainerListItemViewModels;
        public Action? SportCreated;
        public void OnSportCreated()
        {
            SportCreated?.Invoke();
        }
        public AddSportViewModel(SportDataStore sportStore, EmployeeStore trainerStore)
        {
            _sportStore = sportStore;
            _trainerStore = trainerStore;
            //CancelCommand = new NavaigateCommand<SportListViewModel>(new NavigationService<SportListViewModel>(_navigationStore, () => sportListViewModel));
            SubmitCommand = new SubmitSportCommand(this, _sportStore);
            LoadTrainersCommand = new LoadTrainersForSportCommand(_trainerStore, this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            trainerListItemViewModels = new ObservableCollection<TrainersListItemViewModel>();
            _sportStore = sportStore;
            _trainerStore = trainerStore;
            _trainerStore.Loaded += _trainerStore_TrainersLoaded;
            WeeklyTrainingDays = 6;
            SubscribeLength = 30;
        }



        private void _trainerStore_TrainersLoaded()
        {
            trainerListItemViewModels.Clear();

            foreach (Core.Models.Employee.Employee trainer in _trainerStore.Employees.Where(x => x.IsTrainer))
            {
                AddTrainer(trainer);
            }
        }
        private void AddTrainer(Core.Models.Employee.Employee trainer)
        {
            TrainersListItemViewModel itemViewModel =
                new TrainersListItemViewModel(trainer);
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

        private void OnErrorChanged(string? PropertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(PropertyName));
            OnPropertyChanged(nameof(CanSubmit));
        }
        public bool CanSubmit => !HasErrors;
       
        private int _weeklyTrainingDays;
        public int WeeklyTrainingDays
        {
            get { return _weeklyTrainingDays; }
            set
            {
                _weeklyTrainingDays = value;
                OnPropertyChanged(nameof(WeeklyTrainingDays));
                ClearError(nameof(WeeklyTrainingDays));
                if (WeeklyTrainingDays == 0 || WeeklyTrainingDays > 7)
                {
                    AddError("لا يمكن ان يكون هذا الرقم اقل من 1 او اكثر من 7", nameof(WeeklyTrainingDays));
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
        public static AddSportViewModel LoadViewModel(SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            AddSportViewModel viewModel = new AddSportViewModel(sportDataStore, employeeStore);

            viewModel.LoadTrainersCommand.Execute(null);

            return viewModel;
        }

        internal void ClearForm()
        {
            SportName = "";
            MonthlyPrice = 0;
        }
    }
}
