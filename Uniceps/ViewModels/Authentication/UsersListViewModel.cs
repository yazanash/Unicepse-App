using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.AuthCommands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Authentication;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.AuthView;

namespace Uniceps.ViewModels.Authentication
{
    public class UsersListViewModel : ListingViewModelBase
    {

        private readonly ObservableCollection<UserListItemViewModel> _userListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly UsersDataStore _usersDataStore;
        private readonly AuthenticationStore _authenticationStore;
        public IEnumerable<UserListItemViewModel> UsersList => _userListItemViewModels;
        public ICommand AddUserCommand { get; }
        public ICommand LoadUsersCommand { get; }
        public ICommand LoadUsersLogsCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public bool HasData => _userListItemViewModels.Count > 0;
        public UsersListViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore, AuthenticationStore authenticationStore)
        {
            _navigatorStore = navigatorStore;
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;

            _userListItemViewModels = new ObservableCollection<UserListItemViewModel>();
            _usersDataStore.Created += _usersDataStore_Created;
            _usersDataStore.Deleted += _usersDataStore_Deleted;
            _usersDataStore.Updated += _usersDataStore_Updated;
            _usersDataStore.Loaded += _usersDataStore_Loaded;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;

            LoadUsersCommand = new LoadUsersCommand(_usersDataStore);
            AddUserCommand = new RelayCommand(ExecuteAddUserCommand);
            LoadUsersLogsCommand = new NavaigateCommand<AuthenticationLoggingList>(new NavigationService<AuthenticationLoggingList>(_navigatorStore,
               () => CreateLogsViewModel(_usersDataStore)));
        }
        private void ExecuteAddUserCommand()
        {
            AddUserViewModel addUserViewModel = AddUserViewModel.loadViewModel(_usersDataStore);
            AddUserViewWindow addUserViewWindow = new AddUserViewWindow();
            addUserViewWindow.DataContext = addUserViewModel;
            addUserViewWindow.ShowDialog();
        }
        public UserListItemViewModel? SelectedUser
        {
            get
            {
                return UsersList
                    .FirstOrDefault(y => y?.user == _usersDataStore.SelectedUser);
            }
            set
            {
                _usersDataStore.SelectedUser = value?.user;

            }
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _userListItemViewModels.Clear();

            foreach (User user in _usersDataStore.Users.Where(x => x.UserName!.ToLower().Contains(obj!.ToLower())))
            {
                AddUser(user);
            }
        }

        public static UsersListViewModel LoadViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore, AuthenticationStore authenticationStore)
        {
            UsersListViewModel usersListViewModel = new UsersListViewModel(navigatorStore, usersDataStore, authenticationStore);
            usersListViewModel.LoadUsersCommand.Execute(null);
            return usersListViewModel;
        }

        private void _usersDataStore_Loaded()
        {
            _userListItemViewModels.Clear();

            foreach (User user in _usersDataStore.Users)
            {
                AddUser(user);
            }
        }

        private void _usersDataStore_Updated(User obj)
        {
            UserListItemViewModel? itemViewModel =
                  _userListItemViewModels.FirstOrDefault(y => y.user.Id == obj.Id);

            if (itemViewModel != null)
            {
                itemViewModel.update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _usersDataStore_Deleted(int id)
        {
            UserListItemViewModel? itemViewModel = _userListItemViewModels.FirstOrDefault(y => y.user?.Id == id);

            if (itemViewModel != null)
            {
                _userListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _usersDataStore_Created(User obj)
        {
            AddUser(obj);
        }
        public override void Dispose()
        {
            _usersDataStore.Created -= _usersDataStore_Created;
            _usersDataStore.Deleted -= _usersDataStore_Deleted;
            _usersDataStore.Updated -= _usersDataStore_Updated;
            _usersDataStore.Loaded -= _usersDataStore_Loaded;
            base.Dispose();
        }
        private void AddUser(User user)
        {
            UserListItemViewModel userListItemViewModel = new UserListItemViewModel(user, _usersDataStore, _authenticationStore);
            _userListItemViewModels.Add(userListItemViewModel);
            userListItemViewModel.Order = _userListItemViewModels.Count();
            OnPropertyChanged(nameof(HasData));

        }
        private AuthenticationLoggingList CreateLogsViewModel(UsersDataStore usersDataStore)
        {
            return AuthenticationLoggingList.loadViewModel(usersDataStore);
        }
    }
}
