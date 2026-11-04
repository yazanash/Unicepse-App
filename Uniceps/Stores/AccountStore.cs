using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Common;

namespace Uniceps.Stores
{
    public class AccountStore
    {
        public AccountStore()
        {
            new User()
            {
                OwnerName = "مستخدم غير مسجل",
                Role = Roles.Admin,
            };
        }
        private User? _currentAccount;
        public User? CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public event Action? StateChanged;
    }
}
