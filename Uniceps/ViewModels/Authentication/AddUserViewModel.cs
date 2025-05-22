using Uniceps.Commands;
using Uniceps.Commands.AuthCommands;
using Uniceps.ViewModels.SportsViewModels;
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
using Uniceps.navigation;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.navigation.Stores;
using Uniceps.Core.Common;

namespace Uniceps.ViewModels.Authentication
{
    public class AddUserViewModel : ErrorNotifyViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UsersDataStore _usersDataStore;
        private UsersListViewModel _usersListViewModel;
        private readonly EmployeeStore _trainerStore;
        private readonly ObservableCollection<RolesListItemViewModel> _rolesListItemViewModels;
        public IEnumerable<RolesListItemViewModel> RolesList => _rolesListItemViewModels;
        public AddUserViewModel(UsersDataStore usersDataStore, NavigationStore navigationStore, UsersListViewModel usersListViewModel, EmployeeStore trainerStore)
        {
            _usersDataStore = usersDataStore;
            _navigationStore = navigationStore;
            _usersListViewModel = usersListViewModel;
            _trainerStore = trainerStore;
            _rolesListItemViewModels = new ObservableCollection<RolesListItemViewModel>();
            _rolesListItemViewModels.Add(new RolesListItemViewModel("مستخدم", Roles.User));
            _rolesListItemViewModels.Add(new RolesListItemViewModel("مشرف", Roles.Supervisor));
            _rolesListItemViewModels.Add(new RolesListItemViewModel("مسؤول", Roles.Admin));
            _rolesListItemViewModels.Add(new RolesListItemViewModel("محاسب", Roles.Accountant));
            CancelCommand = new NavaigateCommand<UsersListViewModel>(new NavigationService<UsersListViewModel>(_navigationStore, () => _usersListViewModel));
            NavigationStore PlayerMainPageNavigation = new NavigationStore();
            SubmitCommand = new SubmitUserCommand(_usersDataStore, new NavigationService<UsersListViewModel>(_navigationStore, () => _usersListViewModel), this);
            RoleItem = _rolesListItemViewModels.FirstOrDefault();
        }
        private RolesListItemViewModel? _roleItem;
        public RolesListItemViewModel? RoleItem
        {
            get { return _roleItem; }
            set { _roleItem = value; OnPropertyChanged(nameof(RoleItem)); }
        }

        internal static AddUserViewModel loadViewModel(UsersDataStore usersDataStore, NavigationStore navigatorStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore)
        {
            AddUserViewModel addUserViewModel = new AddUserViewModel(usersDataStore, navigatorStore, usersListViewModel, employeeStore);
            return addUserViewModel;
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
                ClearError(nameof(UserName));
                if (string.IsNullOrEmpty(UserName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(UserName));
                    OnErrorChanged(nameof(UserName));
                }

            }
        }
        private string? _password;
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value; OnPropertyChanged(nameof(Password));
                ClearError(nameof(Password));
                if (string.IsNullOrEmpty(Password?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(Password));
                    OnErrorChanged(nameof(Password));
                }
            }
        }

        private string? _postion;
        public string? Position
        {
            get { return _postion; }
            set
            {
                _postion = value; OnPropertyChanged(nameof(Position));
                ClearError(nameof(Position));
                if (string.IsNullOrEmpty(Position?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(Position));
                    OnErrorChanged(nameof(Position));
                }

            }
        }
        private string? _ownerName;
        public string? OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value; OnPropertyChanged(nameof(OwnerName));
                ClearError(nameof(OwnerName));
                if (string.IsNullOrEmpty(OwnerName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(OwnerName));
                    OnErrorChanged(nameof(OwnerName));
                }

            }
        }



        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion

    }
}
