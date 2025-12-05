
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Authentication;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.AuthServiceTest
{
    [TestFixture]
    public class AccountDataServiceTest
    {
        UnicepsDbContextFactory? db;
        UserFactory? userFactory;
        AccountDataService? accountDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDBD; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new UnicepsDbContextFactory(CONNECTION_STRING, false);

            using (UnicepsDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            userFactory = new();
            accountDataService = new(db!, new PasswordHasher<User>());

        }

        [OneTimeTearDown]
        public void Onetimetear()
        {

        }

        [SetUp]
        public void SetUp()
        {

        }
        [TearDown]
        public void TearDown()
        {
            //using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            //{
            //    var users = platinumGymDbContext.Users!.ToList();
            //    platinumGymDbContext.Users!.RemoveRange(users);
            //    platinumGymDbContext.SaveChanges();
            //    var x = platinumGymDbContext.Users!.Count();
            //}
        }
        ////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        /// 
        //////////////////////////
        private async Task create_user(int count)
        {
            for (int i = 0; i < count; i++)
            {
                User actual_employee = await accountDataService!.Create(userFactory!.FakeUser());
            }
        }
        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////

        [Test]
        //it sholud create an employee and assert that it created
        public async Task CreateUser()
        {
            User expected_user = userFactory!.FakeUser();
            User actual_user = await accountDataService!.Create(expected_user);
            Assert.Equals(expected_user.UserName!, actual_user.UserName!);
        }
        [Test]
        //it sholud try to create ana existed user and throw confilct exception
        public async Task CreateExistingUser()
        {
            User expected_user = userFactory!.FakeUser();
            User actual_user = await accountDataService!.Create(expected_user);
            Assert.ThrowsAsync<ConflictException>(
                () => accountDataService.Create(actual_user));
        }
        [Test]
        /// it should get user info and assert it informations
        public async Task GetUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            User test_user = await accountDataService!.Create(expected_user);
            User actual_user = await accountDataService!.Get(test_user.Id);
            //Assert
            Assert.Equals(expected_user.UserName!, actual_user.UserName!);
        }

        [Test]
        /// it should try get not exist user and throw not exist exception 
        public void GetNotExistUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act

            //Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await accountDataService!.Get(expected_user.Id));
        }
        [Test]
        /// it should update user and assert it information updated 
        public async Task UpdateUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            User test_user = await accountDataService!.Create(expected_user);
            User actual_user = await accountDataService.Get(test_user.Id);
            actual_user.UserName = "updated Name";
            User updated_user = await accountDataService.Update(actual_user);
            //Assert
            Assert.Equals(actual_user.UserName!, updated_user.UserName!);
        }

        [Test]
        /// it should try update not exist user and throw not exist exception
        public void UpdateNotExistUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            expected_user.UserName = "updated Name";
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await accountDataService!.Update(expected_user));
        }
        [Test]
        /// it should delete user and assert it deleted
        public async Task DeleteUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            User test_user = await accountDataService!.Create(expected_user);
            await accountDataService.Delete(test_user.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await accountDataService!.Get(test_user.Id));
        }

        [Test]
        /// it should try delete not exist user and throw not exist exception
        public void DeleteNotExistUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await accountDataService!.Delete(expected_user.Id));
        }
        [Test]
        /// it should List All users
        public async Task ListAllUsers()
        {
            //Arrange
            int count = 5;
            //Act
            await create_user(count);
            var users = await accountDataService!.GetAll();
            //Assert
            Assert.Equals(users.Count(), count);
        }
        [Test]
        /// it should disable user
        public async Task DisableUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            User test_user = await accountDataService!.Create(expected_user);
            User actual_user = await accountDataService.Get(test_user.Id);
            actual_user.UserName = "updated Name";
            User updated_user = await accountDataService.Disable(actual_user);
            //Assert
            Assert.Equals(updated_user.Disable, true);

        }
        [Test]
        /// it should try to disable not exist user and throw not exist exception
        public void DisableNotExistUser()
        {
            //Arrange
            User expected_user = userFactory!.FakeUser();
            //Act
            Assert.ThrowsAsync<NotExistException>(
                 async () => await accountDataService!.Disable(expected_user));

        }

    }

}
