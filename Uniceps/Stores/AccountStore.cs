using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Authentication;

namespace Uniceps.Stores
{
    public class AccountStore
    {
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
