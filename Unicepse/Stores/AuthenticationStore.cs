using Unicepse.Core.Models.Authentication;
using Unicepse.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Unicepse.Stores
{
    public class AuthenticationStore
    {
        private readonly AuthenticationService _authenticationService;
        private readonly AccountStore _accountStore;
        private readonly ILogger<AuthenticationStore> _logger;
        string LogFlag = "[Authentication] ";
        public AuthenticationStore(AuthenticationService authenticationService, AccountStore accountStore, ILogger<AuthenticationStore> logger)
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
        public bool HasUser()
        {
            _logger.LogInformation(LogFlag+"checking for users");
            bool data = _authenticationService.HasUsers();
            _logger.LogInformation(LogFlag + "Has Users : " +data.ToString());
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
            }
           
        }

        public async Task<RegistrationResult> Register(string username, string password, string confirmPassword,Roles role)
        {
            _logger.LogInformation(LogFlag + "register user");
            return await _authenticationService.Register(username, password, confirmPassword, role);
        }
    }
}
