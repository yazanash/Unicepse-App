using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
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
    public class DausesDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
        EmployeeFactory? employeeFactory;
        EmployeeDataService? employeeDataService;

        DausesDataService? dausesDataService;

        SubscriptionFactory? subscriptionFactory;
        SubscriptionDataService? subscriptionDataService;

        PlayerFactory? playerFactory;
        PlayerDataService? playerDataService;

        PaymentFactory? paymentFactory;
        PaymentDataService? paymentDataService;

        SportFactory? sportFactory;
        SportServices? sportDataService;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            employeeFactory = new();
            employeeDataService = new(db!);
            subscriptionFactory = new();
            subscriptionDataService = new(db!);
            dausesDataService = new(db!);
            playerFactory = new();
            playerDataService = new(db!);

            paymentFactory = new();
            paymentDataService = new(db!);

            sportFactory = new();
            sportDataService = new(db!);
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
                var payments = platinumGymDbContext.PlayerPayments!.ToList();
                platinumGymDbContext.PlayerPayments!.RemoveRange(payments);
                var subscriptions = platinumGymDbContext.Subscriptions!.ToList();
                platinumGymDbContext.Subscriptions!.RemoveRange(subscriptions);
                var employee = platinumGymDbContext.Employees!.ToList();
                platinumGymDbContext.Employees!.RemoveRange(employee);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Employees!.Count();
            }
        }
        ////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        /// 
        //////////////////////////

        public async Task<Player> create_player()
        {
            Player player = playerFactory!.FakePlayer();
            return await playerDataService!.Create(player);
        }
        public async Task<Sport> create_sport(double sportprice)
        {
            Sport sport = sportFactory!.FakeSport();
            sport.Price = sportprice;
            return await sportDataService!.Create(sport);
        }
        public async Task<Employee> create_trainer()
        {
            Employee trainer = employeeFactory!.FakeEmployee();
            trainer.IsTrainer = true;
            return await employeeDataService!.Create(trainer);
        }
        public async Task<Subscription> create_subscription(Employee trainer, double sportprice, DateTime rolldate)
        {
            Player player = await create_player();
            Sport sport = await create_sport(sportprice);
            Subscription subscription = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            subscription.RollDate = rolldate;
            subscription.EndDate = rolldate.AddDays(subscription.DaysCount);
            return await subscriptionDataService!.Create(subscription);
        }
        //public async Task GenerateData(Employee triner)
        //{
        //    Subscription subscription1 = await create_subscription(triner, 30000, new DateTime(2024, 4, 12));

        //    await create_paymentforsubscription(subscription1, new DateTime(2024, 4, 12), 30000, new DateTime(2024, 4, 12));



        //    Subscription subscription2 = await create_subscription(triner, 30000, new DateTime(2024, 4, 16));

        //    await create_paymentforsubscription(subscription2, new DateTime(2024, 4, 20), 20000, new DateTime(2024, 4, 16));
        //    await create_paymentforsubscription(subscription2, new DateTime(2024, 5, 1), 10000, new DateTime(2024, 5, 1));



        //    Subscription subscription3 = await create_subscription(triner, 30000, new DateTime(2024, 4, 18));
        //    await create_paymentforsubscription(subscription3, new DateTime(2024, 4, 18), 10000, new DateTime(2024, 4, 18));
        //    await create_paymentforsubscription(subscription3, new DateTime(2024, 5, 10), 20000, new DateTime(2024, 5, 10));



        //    Subscription subscription4 = await create_subscription(triner, 30000, new DateTime(2024, 4, 13));
        //    await create_paymentforsubscription(subscription4, new DateTime(2024, 4, 13), 30000, new DateTime(2024, 4, 13));


        //    //////////////////////////////////////////
        //    Subscription subscription5 = await create_subscription(triner, 30000, new DateTime(2024, 4, 13));
        //    await create_paymentforsubscription(subscription5, new DateTime(2024, 5, 2), 15000, new DateTime(2024, 4, 13));



        //    //////////////////////////////////////////
        //    Subscription subscription6 = await create_subscription(triner, 30000, new DateTime(2024, 4, 12));
        //    await create_paymentforsubscription(subscription6, new DateTime(2024, 4, 12), 15000, new DateTime(2024, 4, 12));
        //    await create_paymentforsubscription(subscription6, new DateTime(2024, 4, 30), 15000, new DateTime(2024, 4, 27));



        //    Subscription subscription7 = await create_subscription(triner, 30000, new DateTime(2024, 4, 6));
        //    await create_paymentforsubscription(subscription7, new DateTime(2024, 4, 6), 15000, new DateTime(2024, 4, 6));
        //    await create_paymentforsubscription(subscription7, new DateTime(2024, 5, 2), 15000, new DateTime(2024, 4, 21));



        //    Subscription subscription8 = await create_subscription(triner, 30000, new DateTime(2024, 4, 10));
        //    await create_paymentforsubscription(subscription8, new DateTime(2024, 4, 10), 30000, new DateTime(2024, 4, 10));



        //    Subscription subscription9 = await create_subscription(triner, 30000, new DateTime(2024, 4, 25));
        //    await create_paymentforsubscription(subscription9, new DateTime(2024, 5, 30), 30000, new DateTime(2024, 4, 25));


        //}
        public async Task<List<Subscription>> GenerateData(Employee triner)
        {
            List<Subscription> subscriptions = new List<Subscription>();
            Subscription subscription1 = await create_subscription(triner, 30000, new DateTime(2024, 4, 12));

            await create_paymentforsubscription(subscription1, new DateTime(2024, 4, 12), 30000, new DateTime(2024, 4, 12));



            Subscription subscription2 = await create_subscription(triner, 60000, new DateTime(2024, 4, 16));

            await create_paymentforsubscription(subscription2, new DateTime(2024, 4, 20), 30000, new DateTime(2024, 4, 16));
            await create_paymentforsubscription(subscription2, new DateTime(2024, 5, 1), 30000, new DateTime(2024, 5, 1));



            Subscription subscription3 = await create_subscription(triner, 80000, new DateTime(2024, 4, 18));
            await create_paymentforsubscription(subscription3, new DateTime(2024, 4, 18), 60000, new DateTime(2024, 4, 18));
            await create_paymentforsubscription(subscription3, new DateTime(2024, 5, 10), 20000, new DateTime(2024, 5, 10));



            Subscription subscription4 = await create_subscription(triner, 30000, new DateTime(2024, 4, 13));
            await create_paymentforsubscription(subscription4, new DateTime(2024, 4, 13), 30000, new DateTime(2024, 4, 13));


            //////////////////////////////////////////
            Subscription subscription5 = await create_subscription(triner, 15000, new DateTime(2024, 4, 13));
            await create_paymentforsubscription(subscription5, new DateTime(2024, 5, 2), 15000, new DateTime(2024, 4, 13));



            //////////////////////////////////////////
            Subscription subscription6 = await create_subscription(triner, 30000, new DateTime(2024, 4, 12));
            await create_paymentforsubscription(subscription6, new DateTime(2024, 4, 12), 15000, new DateTime(2024, 4, 12));
            await create_paymentforsubscription(subscription6, new DateTime(2024, 4, 30), 15000, new DateTime(2024, 4, 27));



            Subscription subscription7 = await create_subscription(triner, 30000, new DateTime(2024, 4, 6));
            await create_paymentforsubscription(subscription7, new DateTime(2024, 4, 6), 15000, new DateTime(2024, 4, 6));
            await create_paymentforsubscription(subscription7, new DateTime(2024, 5, 2), 15000, new DateTime(2024, 4, 21));



            Subscription subscription8 = await create_subscription(triner, 30000, new DateTime(2024, 4, 10));
            await create_paymentforsubscription(subscription8, new DateTime(2024, 4, 10), 30000, new DateTime(2024, 4, 10));



            Subscription subscription9 = await create_subscription(triner,80000, new DateTime(2024, 4, 25));
            await create_paymentforsubscription(subscription9, new DateTime(2024, 5, 30), 80000, new DateTime(2024, 4, 25));

            subscriptions.Add(subscription1);
            subscriptions.Add(subscription2);
            subscriptions.Add(subscription3);
            subscriptions.Add(subscription4);
            subscriptions.Add(subscription5);
            subscriptions.Add(subscription6);
            subscriptions.Add(subscription7);
            subscriptions.Add(subscription8);
            subscriptions.Add(subscription9);
            return subscriptions;
        }
        private async Task<PlayerPayment> create_paymentforsubscription(Subscription subscription, DateTime paydate,double pay,DateTime from)
        {

            PlayerPayment payment = new()
            {
                PaymentValue = pay,
                Des = "",
                PayDate = paydate,
                Player = subscription.Player,
                Subscription = subscription,
                From = from,

            };
            return await paymentDataService!
                   .Create(payment);

        }
        private async Task<PlayerPayment> create_payments(PlayerPayment payment)
        {

            //Subscription subscribtion = await create_subscription();
            return await paymentDataService!
                   .Create(payment);

        }
        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////

        //[Test]
        ////it sholud create an employee and assert that it created
        //public async Task SetDausesDefault()
        //{
        //    //Employee triner = await create_trainer();
        //    //Subscription subscription = await create_subscription(triner);
        //    //PlayerPayment playerPayment = new()
        //    //{
        //    //    PaymentValue = subscription.PriceAfterOffer,
        //    //    Des = "",
        //    //    PayDate = DateTime.Now.AddDays(-20),
        //    //    Player = subscription.Player,
        //    //    Subscription = subscription,
        //    //    From = subscription.LastPaid,

        //    //};
        //    //double dayPrice = subscription.PriceAfterOffer / subscription.DaysCount;
        //    //playerPayment.CoverDays = (int)(playerPayment.PaymentValue / dayPrice);
        //    //playerPayment.To = playerPayment.From.AddDays(playerPayment.CoverDays);
        //    //await create_payments(playerPayment);

        //    //IEnumerable<PlayerPayment> playerPayments = await paymentDataService!.GetTrainerPayments(subscription.Trainer!, DateTime.Now);
        //    //Assert.AreEqual(1, playerPayments.Count());
        //    //Assert.AreEqual(30, playerPayment.CoverDays);
        //    //double parcent = ((playerPayments.Sum(x => x.PaymentValue) * subscription.Trainer!.ParcentValue)) / 100;
        //    //Assert.AreEqual(7500, parcent);
        //}
        [Test]
        public async Task GetDausesDefault()
        {
            DateTime date1 = new DateTime(2024, 4, 30);
            DateTime date2 = new DateTime(2024, 5, 31);
            Employee triner = await create_trainer();
           List<Subscription> subscriptions = await GenerateData(triner);



            IEnumerable<PlayerPayment> playerPayments = await paymentDataService!.GetTrainerPayments(triner, date1);

            double date1TD = 0;
            foreach (var pay in playerPayments)
            {
                date1TD += dausesDataService!.GetParcent(pay, date1);
            }

           



           


            IEnumerable<PlayerPayment> playerPayments2 = await paymentDataService!.GetTrainerPayments(triner, date2);

            double date2TD = 0;
            foreach (var pay in playerPayments2)
            {
                date2TD += dausesDataService!.GetParcent(pay, date2);
            }

            double total = date1TD + date2TD;
            double allTotal = subscriptions.Sum(x => x.PriceAfterOffer);
            double allDauses = (allTotal * triner.ParcentValue)/100;
            //Assert.AreEqual(37000, (int)date1TD);
            //Assert.AreEqual(58916, (int) date2TD);
            Assert.AreEqual(allDauses, (int)((date1TD+date2TD)*triner.ParcentValue)/100);

        }


    }
}
