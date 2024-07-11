using Microsoft.AspNet.Identity;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Authentication;
using Unicepse.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountDataService<User> _accountService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IAccountDataService<User> accountService, IPasswordHasher passwordHasher)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }
        public  bool HasUsers()
        {
           bool hasUsers =  _accountService.HasUsers();
            return hasUsers;
        }

        public async Task<User> Login(string username, string password)
        {
            User storedAccount = await _accountService.GetByUsername(username);

            if (storedAccount == null)
            {
                throw new UserNotFoundException("خطأ في اسم المستخدم او كلمة المرور",username);
            }

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedAccount.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException("خطأ في اسم المستخدم او كلمة المرور",username, password);
            }

            return storedAccount;
        }

        public async Task<RegistrationResult> Register(string username, string password, string confirmPassword)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            User usernameAccount = await _accountService.GetByUsername(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                
                User user = new User()
                {
                    UserName = username,
                    Password = password,
                    IsAdmin = false
                };


                await _accountService.Create(user);
            }

            return result;
        }

    }
}
