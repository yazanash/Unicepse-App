using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.AuthCommands;
using Uniceps.Commands.Player;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SportsViewModels;

namespace Uniceps.ViewModels.Authentication
{
    public class AddUserViewModel : ErrorNotifyViewModelBase,IQueryAttributable
    {
        private readonly UsersDataStore _usersDataStore;
        public ObservableCollection<Roles> RolesList { get; set; } = new();
        public bool IsEditMode = false;
        public int UserId = -1;
        public void ApplyEditMode(User user)
        {
            IsEditMode = true;
            UserId = user.Id;
            UserName = user.UserName;
            Position = user.Position;
            OwnerName = user.OwnerName;
            RoleItem = RolesList.FirstOrDefault(x => x == user.Role);
        }
        public AddUserViewModel(UsersDataStore usersDataStore)
        {
            _usersDataStore = usersDataStore;
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                RolesList.Add((Roles)item);
            }
            SubmitCommand = new SubmitUserCommand(_usersDataStore, this);
            RoleItem = RolesList.FirstOrDefault();
        }
        private Roles _roleItem;
        public Roles RoleItem
        {
            get { return _roleItem; }
            set { _roleItem = value; OnPropertyChanged(nameof(RoleItem)); }
        }
        public Action? UserCreated;
        internal void OnUserCreated()
        {
            UserCreated?.Invoke();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<User>("User", out var user))
            {
                if (user != null)
                    ApplyEditMode(user);
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
        #endregion

    }
}
