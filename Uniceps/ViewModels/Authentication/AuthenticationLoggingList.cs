using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.Commands.AuthCommands;

namespace Uniceps.ViewModels.Authentication
{
    public class AuthenticationLoggingList : ListingViewModelBase
    {
        private readonly UsersDataStore _usersDataStore;
        private readonly ObservableCollection<AuthenticationLoggingListItemViewModel> _authenticationLoggingListItemViewModels;

        public IEnumerable<AuthenticationLoggingListItemViewModel> LogList => _authenticationLoggingListItemViewModels;
        public AuthenticationLoggingList(UsersDataStore usersDataStore)
        {
            _usersDataStore = usersDataStore;
            _authenticationLoggingListItemViewModels = new ObservableCollection<AuthenticationLoggingListItemViewModel>();
            _usersDataStore.logs_Loaded += _usersDataStore_logs_Loaded;
            Date = DateTime.Now;
            LoadLogsCommand = new LoadLogsCommand(_usersDataStore, this);
        }

        private void _usersDataStore_logs_Loaded()
        {
            _authenticationLoggingListItemViewModels.Clear();
            foreach (var log in _usersDataStore.Logs)
            {
                AuthenticationLoggingListItemViewModel authenticationLoggingListItemViewModel =
                    new AuthenticationLoggingListItemViewModel(log);
                _authenticationLoggingListItemViewModels.Add(authenticationLoggingListItemViewModel);
                authenticationLoggingListItemViewModel.Order = _authenticationLoggingListItemViewModels.Count();
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
        internal static AuthenticationLoggingList loadViewModel(UsersDataStore usersDataStore)
        {
            AuthenticationLoggingList editUserViewModel = new AuthenticationLoggingList(usersDataStore);
            editUserViewModel.LoadLogsCommand.Execute(null);
            return editUserViewModel;
        }
        public ICommand LoadLogsCommand { get; }
    }
}
