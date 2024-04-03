using PlatinumGym.Core.Models.Authentication;
using PlatinumGymPro.Commands.AuthCommands;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Authentication
{
    public class UsersListViewModel : ListingViewModelBase
    {

        private readonly ObservableCollection<UserListItemViewModel> _userListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly UsersDataStore _usersDataStore;
        public IEnumerable<UserListItemViewModel> UsersList => _userListItemViewModels;
        //public ICommand AddUserCommand { get; }
        public ICommand LoadUsersCommand { get; }

        public UsersListViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore)
        {
            _navigatorStore = navigatorStore;
            _usersDataStore = usersDataStore;
            _userListItemViewModels = new ObservableCollection<UserListItemViewModel>();
            _usersDataStore.Created += _usersDataStore_Created;
            _usersDataStore.Deleted += _usersDataStore_Deleted;
            _usersDataStore.Updated += _usersDataStore_Updated;
            _usersDataStore.Loaded += _usersDataStore_Loaded;
            LoadUsersCommand = new LoadUsersCommand(_usersDataStore);
        }

        public static UsersListViewModel LoadViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore)
        {
            UsersListViewModel usersListViewModel = new UsersListViewModel(navigatorStore,usersDataStore);
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
        }

        private void _usersDataStore_Deleted(int id)
        {
            UserListItemViewModel? itemViewModel = _userListItemViewModels.FirstOrDefault(y => y.user?.Id == id);

            if (itemViewModel != null)
            {
                _userListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _usersDataStore_Created(User obj)
        {
            AddUser(obj);
        }
        private void AddUser(User user) 
        {
            UserListItemViewModel userListItemViewModel = new UserListItemViewModel(user);
            _userListItemViewModels.Add(userListItemViewModel);

        }
    }
}
