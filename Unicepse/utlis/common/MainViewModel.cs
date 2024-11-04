using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.navigation.Navigator;
using Unicepse.Stores;
using Unicepse.navigation.Stores;
using System.Windows.Media;
using Unicepse.ViewModels.Accountant;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.utlis.common
{
    public class MainViewModel : ViewModelBase
    {

        public INavigator Navigator { get; set; }
        private readonly UsersDataStore _usersDataStore;
        private readonly AccountingViewModel _accountingViewModel;
        private readonly HomeNavViewModel _homeNavViewModel;
        private readonly BackgroundServiceStore _backgroundServiceStore;
        private readonly AuthenticationStore _authenticationStore;
        public StatusBarViewModel StatusBarViewModel { get; set; }
        public MainViewModel(UsersDataStore usersDataStore, BackgroundServiceStore backgroundServiceStore, AuthenticationStore authenticationStore, INavigator navigator, AccountingViewModel accountingViewModel, HomeNavViewModel homeNavViewModel)
        {
            Navigator = navigator;
            _accountingViewModel = accountingViewModel;
            _homeNavViewModel = homeNavViewModel;
           
            _usersDataStore = usersDataStore;
           
            _authenticationStore = authenticationStore;
         
            _backgroundServiceStore = backgroundServiceStore;
            _backgroundServiceStore.StateChanged += _backgroundServiceStore_StateChanged;
            _backgroundServiceStore.SyncStatus += _backgroundServiceStore_SyncStatus;
            _usersDataStore.Updated += _usersDataStore_Updated;
            
            if (_authenticationStore.CurrentAccount!.Role == Core.Common.Roles.Accountant)
            {
                Navigator.CurrentViewModel = _accountingViewModel;
            }
            else
            {
                Navigator.CurrentViewModel = _homeNavViewModel;
            }

            StatusBarViewModel = new StatusBarViewModel(_authenticationStore.CurrentAccount!.UserName,
                _authenticationStore.CurrentAccount!.Position,
                _authenticationStore.CurrentAccount!.OwnerName);
            switch (_authenticationStore.CurrentAccount!.Role)
            {
                case Core.Common.Roles.Admin:
                    StatusBarViewModel.Role = "مدير النظام";
                    break;
                case Core.Common.Roles.User:
                    StatusBarViewModel.Role = "مستخدم";
                    break;
                case Core.Common.Roles.Accountant:
                    StatusBarViewModel.Role = "محاسب";
                    break;
                case Core.Common.Roles.Supervisor:
                    StatusBarViewModel.Role = "مسؤول";
                    break;
            }
            StatusBarViewModel.SyncState = _backgroundServiceStore.SyncStateProp;
            StatusBarViewModel.SyncMessage = _backgroundServiceStore.SyncMessage;
            StatusBarViewModel.BackMessage = _backgroundServiceStore.BackMessage;
            StatusBarViewModel.Connection = _backgroundServiceStore.Connection ? Brushes.Green : Brushes.Red;
          
        }

        private void _usersDataStore_Updated(Core.Models.Authentication.User obj)
        {
            if (_authenticationStore.CurrentAccount!.Id == obj.Id)
            {
                StatusBarViewModel.UserName = _authenticationStore.CurrentAccount!.UserName;
                StatusBarViewModel.Position = _authenticationStore.CurrentAccount!.Position;
                StatusBarViewModel.OwnerName = _authenticationStore.CurrentAccount!.OwnerName;
                switch (_authenticationStore.CurrentAccount!.Role)
                {
                    case Core.Common.Roles.Admin:
                        StatusBarViewModel.Role = "مدير النظام";
                        break;
                    case Core.Common.Roles.User:
                        StatusBarViewModel.Role = "مستخدم";
                        break;
                    case Core.Common.Roles.Accountant:
                        StatusBarViewModel.Role = "محاسب";
                        break;
                    case Core.Common.Roles.Supervisor:
                        StatusBarViewModel.Role = "مسؤول";
                        break;
                }
            }
        }
        private void _backgroundServiceStore_SyncStatus(bool obj,string? message)
        {
            StatusBarViewModel.SyncState = obj;
            StatusBarViewModel.SyncMessage = message;
        }
        private void _backgroundServiceStore_StateChanged(string? obj,bool connectionStatus)
        {
            StatusBarViewModel.BackMessage = obj;
            StatusBarViewModel.Connection=connectionStatus ? Brushes.Green : Brushes.Red;
        }
        
    }
}
