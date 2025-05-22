
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Services;
using Uniceps.Core.Common;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.AuthServiceTest
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        Mock<IAccountDataService<User>>? mockAccountDataService;
        Mock<IPasswordHasher<User>>? mockPasswordhasher;
        AuthenticationService? authenticationService;
        UserFactory? userFactory;

        [SetUp]
        public void SetUp()
        {
            mockAccountDataService = new Mock<IAccountDataService<User>>();
            mockPasswordhasher = new Mock<IPasswordHasher<User>>();
            authenticationService = new AuthenticationService(mockAccountDataService.Object, mockPasswordhasher.Object);
            userFactory = new();
        }
        [Test]
        public async Task LoginUserWithCorrectUserNameAndPassword()
        {
            string userName = "testuser";
            string password = "password";
            User userItem = new User { UserName = userName, Password = password };
            mockAccountDataService!.Setup(s => s.GetByUsername(userName)).ReturnsAsync(userItem);
            mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(userItem, It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Success);
            User user = await authenticationService!.Login(userName, password);

            Assert.AreEqual(userName, user.UserName);
        }
        [Test]
        public void LoginUserWithCorrectUserNameAndWrongPassword()
        {
            string userName = "testuser";
            string password = "password";
            mockAccountDataService!.Setup(s => s.GetByUsername(userName)).ReturnsAsync(new User { UserName = userName, Password = password });
            //mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            InvalidPasswordException? exception = Assert.ThrowsAsync<InvalidPasswordException>(
                async () => await authenticationService!.Login(userName, password));

            Assert.AreEqual(userName, exception!.Username);
        }
        [Test]
        public void LoginUserWithNotExistUserName()
        {
            string userName = "testuser";
            string password = "password";
            //mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            UserNotFoundException? exception = Assert.ThrowsAsync<UserNotFoundException>(
                async () => await authenticationService!.Login(userName, password));

            Assert.AreEqual(userName, exception!.Username);
        }

        [Test]
        public async Task RegisterUserWithCorrectUserNameAndNotMatchPassword()
        {
            string userName = "testuser";
            string password = "password";
            string confirm_password = "notconfirmpassword";
            RegistrationResult expected = RegistrationResult.PasswordsDoNotMatch;
            RegistrationResult actual = await authenticationService!.Register(userName, password, confirm_password, Roles.Admin);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task RegisterUserWithExistedUserName()
        {
            string userName = "testuser";
            string password = "password";
            string confirm_password = "notconfirmpassword";
            mockAccountDataService!.Setup(s => s.GetByUsername(userName)).ReturnsAsync(new User { UserName = userName, Password = password, Role = Roles.Admin });
            RegistrationResult expected = RegistrationResult.UsernameAlreadyExists;
            RegistrationResult actual = await authenticationService!.Register(userName, password, confirm_password, Roles.Admin);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task RegisterUserWithCorrectUserNameAndCorrectPassword()
        {
            User user = userFactory!.FakeUser();
            //mockAccountDataService!.Setup(s => s.Create(user)).ReturnsAsync(user);
            RegistrationResult expected = RegistrationResult.Success;
            RegistrationResult actual = await authenticationService!.Register(user!.UserName!, user!.Password!, user!.Password!, user.Role);

            Assert.AreEqual(expected, actual);
        }
    }
}
