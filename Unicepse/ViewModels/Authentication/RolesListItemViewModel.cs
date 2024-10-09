using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.Authentication
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
