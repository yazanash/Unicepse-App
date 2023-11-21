using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.AuthServiceTest
{
    [TestFixture]
    public class AccountDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
        UserFactory? userFactory;
        AccountDataService? accountDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            userFactory = new();
            accountDataService = new(db!);

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
            using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var users = platinumGymDbContext.Users!.ToList();
                platinumGymDbContext.Users!.RemoveRange(users);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Users!.Count();
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
            Assert.AreEqual(expected_user.UserName, actual_user.UserName);
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
    }

}
