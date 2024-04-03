using PlatinumGym.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.Authentication
{
    public class UserListItemViewModel
    {
        public User user;

        public string? username => user.UserName;
        //public string? EmployeeName => user.Employee!.FullName;
        public string? Role => "سكرتارية";
        public string? Postion => "سكرتارية";
        public UserListItemViewModel(User user)
        {
            this.user = user;
        }
        public void update(User user)
        {
            this.user = user;
        }
    }
}
