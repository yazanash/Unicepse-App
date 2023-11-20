using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Entityframework.Services;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class PaymentDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
        PaymentFactory? paymentFactory;
        PaymentDataService? paymentDataService;

        SubscriptionFactory? subscriptionFactory;
        PlayerFactory? playerFactory;
        SportFactory? sportFactory;
        EmployeeFactory? employeeFactory;
        SubscriptionDataService? subscriptionDataService;
        PlayerDataService? playerDataService;
        SportServices? sportDataService;
        EmployeeDataService? employeeDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            paymentFactory = new();
            paymentDataService = new(db!);

            subscriptionFactory = new();
            sportFactory = new();
            playerFactory = new();
            employeeFactory = new();
            subscriptionDataService = new(db!);
            playerDataService = new(db!);
            sportDataService = new(db!);
            employeeDataService = new(db!);
        }
     
        [TearDown]
        public void TearDown()
        {
            using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var payments = platinumGymDbContext.PlayerPayments!.ToList();
                platinumGymDbContext.PlayerPayments!.RemoveRange(payments);
                var subscriptions = platinumGymDbContext.Subscriptions!.ToList();
                platinumGymDbContext.Subscriptions!.RemoveRange(subscriptions);
                var sports = platinumGymDbContext.Sports!.ToList();
                platinumGymDbContext.Sports!.RemoveRange(sports);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Sports!.Count();
            }
        }

        ////////////////////////////////////////
        /// 
        ///   H E L P E R  F U N C T I O N S
        /// 
        ///////////////////////////////

        public async Task<Player> create_player()
        {
            Player player = playerFactory!.FakePlayer();
            return await playerDataService!.Create(player);
        }
        public async Task<Sport> create_sport()
        {
            Sport sport = sportFactory!.FakeSport();
            return await sportDataService!.Create(sport);
        }
        public async Task<Employee> create_trainer()
        {
            Employee trainer = employeeFactory!.FakeEmployee();
            trainer.IsTrainer = true;
            return await employeeDataService!.Create(trainer);
        }
        public async Task<Subscription> create_subscription()
        {
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription subscription = subscriptionFactory!.FakeSubscription(sport,player,trainer);
            return await subscriptionDataService!.Create(subscription);
        }

        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////
        [Test]
        public async Task CreatePayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            // Act
            PlayerPayment created_payment = await paymentDataService!.Create(payment);
            // Assert
            Assert.AreEqual(payment.Player!.Id , created_payment.Player!.Id);
            Assert.AreEqual(payment.Subscription!.Id, created_payment.Subscription!.Id);

        }

        [Test]
        public async Task GetPayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            // Act
            PlayerPayment created_payment = await paymentDataService!.Create(payment);
            PlayerPayment get_payment = await paymentDataService!.Get(payment.Id);
            // Assert
            Assert.AreEqual(created_payment.Player!.Id, get_payment.Player!.Id);
            Assert.AreEqual(created_payment.Subscription!.Id, get_payment.Subscription!.Id);

        }
        [Test]
        public async Task GetNotExistPayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            // Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await paymentDataService!.Get(payment.Id));

        }
        [Test]
        public async Task UpdatePayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            // Act
            PlayerPayment created_payment = await paymentDataService!.Create(payment);
            PlayerPayment get_payment = await paymentDataService!.Get(payment.Id);
            get_payment.PaymentValue = 30000;
            PlayerPayment updated_payment = await paymentDataService.Update(get_payment);
            // Assert
            Assert.AreEqual(updated_payment.PaymentValue, 30000);

        }

        [Test]
        public async Task UpdateNotExistPayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            // Act
            payment.PaymentValue = 30000;
            // Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await paymentDataService!.Update(payment));

        }

        [Test]
        public async Task DeletePayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            PlayerPayment created_payment = await paymentDataService!.Create(payment);
            // Act
            await paymentDataService.Delete(created_payment.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await paymentDataService!.Get(created_payment.Id));

        }
        [Test]
        public async Task DeleteNotExistPayment()
        {
            // Arrange
            Subscription subscription = await create_subscription();
            PlayerPayment payment = paymentFactory!.FakePayments(subscription);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await paymentDataService!.Delete(payment.Id));

        }
    }
}
