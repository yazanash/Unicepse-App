using PlatinumGym.Core.Models.Sport;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Commands.TrainersCommands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.TrainersViewModels;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.TrainersViewModels
{
    public class AddTrainerViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SportTrainerListItemViewModel> _sportListItemViewModels;
        private readonly SportDataStore  _sportDataStore ;
        private readonly NavigationStore _navigationStore;
        private readonly TrainersListViewModel _trinersListViewModel;
        private readonly EmployeeStore _employeeStore;
        public IEnumerable<SportTrainerListItemViewModel> SportList => _sportListItemViewModels;
        public AddTrainerViewModel(NavigationStore navigationStore, TrainersListViewModel trinersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            _sportDataStore = sportDataStore;
            _navigationStore = navigationStore;
            _trinersListViewModel = trinersListViewModel;
            _employeeStore = employeeStore;
            _sportListItemViewModels = new();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            CancelCommand = new NavaigateCommand<TrainersListViewModel>(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel));
            SubmitCommand = new SubmitTrainerCommand(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel), this,_employeeStore);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore); 
        }

        private void _sportDataStore_Loaded()
        {
            _sportListItemViewModels.Clear();
            foreach(var sport in _sportDataStore.Sports)
            {
                AddSport(sport);
            }
        }

        private void AddSport(Sport sport)
        {
            SportTrainerListItemViewModel sportListItemViewModel = new(sport);
            _sportListItemViewModels.Add(sportListItemViewModel);
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string? _phone;
        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private int _birthDate;
        public int BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        private bool _genderMale;
        public bool GenderMale
        {
            get { return _genderMale; }
            set
            {
                _genderMale = value;
                OnPropertyChanged(nameof(GenderMale));
            }
        }

        private double _salaryValue;
        public double SalaryValue
        {
            get { return _salaryValue; }
            set
            {
                _salaryValue = value;
                OnPropertyChanged(nameof(SalaryValue));
            }
        }

        private int _parcentValue;
        public int ParcentValue
        {
            get { return _parcentValue; }
            set
            {
                _parcentValue = value;
                OnPropertyChanged(nameof(ParcentValue));
            }
        }

        private string? _position;
        public string? Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private DateTime _startDate;
        public DateTime StartDate 
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
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
        }

       
       
        
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        public ICommand LoadSportsCommand { get; }
        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable? GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }
        public static AddTrainerViewModel LoadViewModel(NavigationStore navigatorStore, TrainersListViewModel trainersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            AddTrainerViewModel viewModel = new AddTrainerViewModel(navigatorStore, trainersListViewModel, sportDataStore, employeeStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
    }
}
