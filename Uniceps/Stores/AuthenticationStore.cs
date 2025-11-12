using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Logging;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Common;
using Uniceps.Entityframework.Services.AuthService;

namespace Uniceps.Stores
{
    public class AuthenticationStore
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AccountStore _accountStore;
        private readonly ILogger<AuthenticationStore> _logger;
        string LogFlag = "[Authentication] ";
        public event Action? LogoutAction;
        public AuthenticationStore(IAuthenticationService authenticationService, AccountStore accountStore, ILogger<AuthenticationStore> logger)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _logger = logger;
        }

        public User? CurrentAccount
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
        public async Task<bool> HasUser()
        {
            _logger.LogInformation(LogFlag + "checking for users");
            bool data = await _authenticationService.HasUsers();
            _logger.LogInformation(LogFlag + "Has Users : " + data.ToString());
            return data;
        }

        public event Action? StateChanged;

        public async Task Login(string username, string password)
        {
            _logger.LogInformation(LogFlag + "login user");
            CurrentAccount = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            _logger.LogInformation(LogFlag + "logout user");
            if (CurrentAccount != null)
            {
                var acc = CurrentAccount;
                CurrentAccount = null;
                _authenticationService.Logout(acc!);
                CurrentAccount = null;
                LogoutAction?.Invoke();
            }

        }

        public async Task<RegistrationResult> Register(string username, string password, string confirmPassword, Roles role)
        {
            _logger.LogInformation(LogFlag + "register user");
            return await _authenticationService.Register(username, password, confirmPassword, role);
        }
    }
}
