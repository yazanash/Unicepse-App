using Unicepse.Core.Models.Authentication;
using Unicepse.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly AuthenticationService _authenticationService;
        private readonly AccountStore _accountStore;

        public AuthenticationStore(AuthenticationService authenticationService, AccountStore accountStore)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
        }
        
        public User CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }
        
        public bool IsLoggedIn => CurrentAccount != null;

      
        public event Action? StateChanged;
      
        public async Task Login(string username, string password)
        {
            CurrentAccount = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            CurrentAccount = null;
        }

        public async Task<RegistrationResult> Register( string username, string password, string confirmPassword)
        {
            return await _authenticationService.Register(username, password, confirmPassword);
        }
    }
}
