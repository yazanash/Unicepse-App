using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.AuthServiceTest
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        Mock<IAccountDataService<User>>? mockAccountDataService;
        Mock<IPasswordHasher>? mockPasswordhasher;
        AuthenticationService? authenticationService;

     [SetUp]
        public void SetUp()
        {
            mockAccountDataService = new Mock<IAccountDataService<User>>();
            mockPasswordhasher = new Mock<IPasswordHasher>();
            authenticationService = new AuthenticationService(mockAccountDataService.Object, mockPasswordhasher.Object);
        }
        [Test]
        public async Task LoginUserWithCorrectUserNameAndPassword()
        {
            string userName = "testuser";
            string password = "password";
            mockAccountDataService!.Setup(s=>s.GetByUsername(userName)).ReturnsAsync(new User { UserName = userName,Password=password});
            mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Success);
            User user = await authenticationService!.Login(userName,password);

            Assert.AreEqual(userName, user.UserName);
        }
        [Test]
        public void LoginUserWithCorrectUserNameAndWrongPassword()
        {
            string userName = "testuser";
            string password = "password";
            mockAccountDataService!.Setup(s => s.GetByUsername(userName)).ReturnsAsync(new User { UserName = userName, Password = password });
            mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            InvalidPasswordException? exception = Assert.ThrowsAsync<InvalidPasswordException>(
                async () => await authenticationService!.Login(userName, password));

            Assert.AreEqual(userName, exception!.Username);
        }
        [Test]
        public void LoginUserWithNotExistUserName()
        {
            string userName = "testuser";
            string password = "password";
            mockPasswordhasher!.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Failed);

            UserNotFoundException? exception = Assert.ThrowsAsync<UserNotFoundException>(
                async () => await authenticationService!.Login(userName, password));

            Assert.AreEqual(userName, exception!.Username);
        }
    }
}
