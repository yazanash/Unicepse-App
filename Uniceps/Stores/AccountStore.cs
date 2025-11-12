using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Common;
using Uniceps.Core.Models.SystemAuthModels;

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
        public event Action? ProfileChanged;
        public event Action? UserContextChanged;

        private UserContextState _userContext;
        public UserContextState UserContext
        {
            get
            {
                return _userContext;
            }
            set
            {
                _userContext = value;
                UserContextChanged?.Invoke();
            }
        }

        private SystemProfile? _systemProfile;
        public SystemProfile? SystemProfile
        {
            get
            {
                return _systemProfile;
            }
            set
            {
                _systemProfile = value;
                ProfileChanged?.Invoke();
            }
        }
        private string? _businessId;
        public string? BusinessId
        {
            get
            {
                return _businessId;
            }
            set
            {
                _businessId = value;
            }
        }
    }
}
