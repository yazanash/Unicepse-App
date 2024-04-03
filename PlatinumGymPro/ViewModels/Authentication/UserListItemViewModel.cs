﻿using PlatinumGym.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Authentication
{
    public class UserListItemViewModel
    {
        public User user;

        public int Id { get; }
        public string? username => user.UserName;
        public string? EmployeeName => user.Employee==null?"اسم الموظف": user.Employee!.FullName;
        public string? Role => "سكرتارية";
        public string? Postion => user.Employee == null ? "اسم الوظيفة" : user.Employee!.Position;
        public UserListItemViewModel(User user)
        {
            this.user = user;
        }
        public void update(User user)
        {
            this.user = user;
        }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
    }
}
