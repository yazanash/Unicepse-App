using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.ViewModels.Authentication
{
    public class RolesListItemViewModel : ViewModelBase
    {
        public string? RoleName { get; set; }
        public Roles role { get; set; }

        public RolesListItemViewModel(string? roleName, Roles role)
        {
            RoleName = roleName;
            this.role = role;
        }
    }
}
