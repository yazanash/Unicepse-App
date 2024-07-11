using Unicepse.Commands;
using Unicepse.Commands.AuthCommands;
using Unicepse.ViewModels.SportsViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Authentication
{
    public class AddUserViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UsersDataStore _usersDataStore;
        private UsersListViewModel _usersListViewModel;
        private readonly EmployeeStore _trainerStore;
        private readonly ObservableCollection<EmployeeListItemViewModel> _employeeListItemViewModels;
        public IEnumerable<EmployeeListItemViewModel> EmployeeList => _employeeListItemViewModels;
        public AddUserViewModel(UsersDataStore usersDataStore, NavigationStore navigationStore, UsersListViewModel usersListViewModel, EmployeeStore trainerStore)
        {
            _usersDataStore = usersDataStore;
            _navigationStore = navigationStore;
            _usersListViewModel = usersListViewModel;
            _trainerStore = trainerStore;
            _trainerStore.Loaded += _trainerStore_Loaded;
            _employeeListItemViewModels = new ObservableCollection<EmployeeListItemViewModel>();
            CancelCommand = new NavaigateCommand<UsersListViewModel>(new NavigationService<UsersListViewModel>(_navigationStore, () => _usersListViewModel));
            NavigationStore PlayerMainPageNavigation = new NavigationStore();
            SubmitCommand = new SubmitUserCommand(_usersDataStore, new NavigationService<UsersListViewModel>(_navigationStore, () => _usersListViewModel), this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            LoadEmployeeCommand = new LoadEmployeeForUsersCommand(_trainerStore);
        }

        private void _trainerStore_Loaded()
        {
            _employeeListItemViewModels.Clear();
            foreach (var emp in _trainerStore.Employees)
            {
                EmployeeListItemViewModel employeeListItemViewModel = new EmployeeListItemViewModel(emp);
                _employeeListItemViewModels.Add(employeeListItemViewModel);
            }
        }

        public EmployeeListItemViewModel? SelectedEmployee
        {
            get
            {
                return EmployeeList
                    .FirstOrDefault(y => y?.trainer == _usersDataStore.SelectedEmployee);
            }
            set
            {
                _usersDataStore.SelectedEmployee = value?.trainer;

            }
        }

        internal static AddUserViewModel loadViewModel(UsersDataStore usersDataStore, NavigationStore navigatorStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore)
        {
            AddUserViewModel addUserViewModel = new AddUserViewModel(usersDataStore, navigatorStore, usersListViewModel, employeeStore);
            addUserViewModel.LoadEmployeeCommand.Execute(null);
            return addUserViewModel;
        }

        private bool? _submited = false;
        public bool? Submited
        {
            get { return _submited; }
            set
            {
                _submited = value;
                OnPropertyChanged(nameof(Submited));

            }
        }
        private string? _submitMessage;
        public string? SubmitMessage
        {
            get { return _submitMessage; }
            set
            {
                _submitMessage = value;
                OnPropertyChanged(nameof(SubmitMessage));

            }
        }
        #region Properties
        public int Id { get; }

        private string? _userName;
        public string? UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));

            }
        }
        private string? _password;
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value; OnPropertyChanged(nameof(Password));

            }
        }





        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion
        public ICommand LoadEmployeeCommand { get; }

        public bool CanSubmit => !HasErrors;

        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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

        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }
    }
}
