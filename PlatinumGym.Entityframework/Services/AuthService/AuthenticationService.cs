using Microsoft.AspNet.Identity;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services.AuthService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AccountDataService _accountService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(AccountDataService accountService, IPasswordHasher passwordHasher)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Login(string username, string password)
        {
            User storedAccount = await _accountService.GetByUsername(username);

            if (storedAccount == null)
            {
                throw new UserNotFoundException();
            }

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedAccount.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException();
            }

            return storedAccount;
        }

        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            User emailAccount = await _accountService.GetByEmail(email);
            if (emailAccount != null)
            {
                result = RegistrationResult.EmailAlreadyExists;
            }

            User usernameAccount = await _accountService.GetByUsername(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);

                User user = new User()
                {
                    UserName = username,
                    Password = hashedPassword,
                    IsAdmin = false
                };

                
                await _accountService.Create(user);
            }

            return result;
        }
    }
}
