﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Subscription;
using Unicepse.Entityframework.Services;
using Unicepse.Entityframework.DbContexts;
using Unicepse.Test.Fakes;
using Microsoft.EntityFrameworkCore;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services.PlayerQueries;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Exceptions;

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class SubscriptionDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
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
                var players = platinumGymDbContext.Players!.ToList();
                platinumGymDbContext.Players!.RemoveRange(players);
                var sports = platinumGymDbContext.Sports!.ToList();
                platinumGymDbContext.Sports!.RemoveRange(sports);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Subscriptions!.Count();
            }
        }
        /////////////////////////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        ///
        /////////////////////////////////////////////////////
        private async Task create_subscriptions(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Player player = await create_player();
                Sport sport = await create_sport();
                Employee trainer = await create_trainer();
                Subscription actual_subscribtion = await subscriptionDataService!
                    .Create(subscriptionFactory!.FakeSubscription(sport, player, trainer));
            }
        }

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

        ///////////////////////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////////////////////////////

        [Test]
        // it should create subscription and assert that it created
        public async Task CreateSubscription()
        {
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            Subscription actual_subscription = await subscriptionDataService!.Create(expected_subscription);
            //Assert
            Assert.AreEqual(expected_subscription.Player!.FullName, actual_subscription.Player!.FullName);
            Assert.AreEqual(expected_subscription.Sport!.Name, actual_subscription.Sport!.Name);
        }
    
        [Test]
        //it should try to create existed subscription and throw conflict exception
        public async Task CreateExisteingSubscription()
        {
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            Subscription existed_suscription = await subscriptionDataService!.Create(expected_subscription);
            Subscription recreate_subscription = subscriptionFactory.FakeSubscription(sport, player, trainer);
            Assert.ThrowsAsync<ConflictException>(
                () => subscriptionDataService.Create(recreate_subscription));
        }
        [Test]
        /// it should get subscription info and assert it informations
        public async Task GetSubscription()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act
            Subscription test_subscription = await subscriptionDataService!.Create(expected_subscription);
            Subscription actual_subscription = await subscriptionDataService.Get(test_subscription.Id);
            //Assert
            Assert.AreEqual(expected_subscription.Player!.FullName, actual_subscription.Player!.FullName);
            Assert.AreEqual(expected_subscription.Sport!.Name, actual_subscription.Sport!.Name);
        }

        [Test]
        /// it should try get not exist subscription and throw not exist exception 
        public async Task GetNotExistSubscription()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act

            //Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await subscriptionDataService!.Get(expected_subscription.Id));
        }

        [Test]
        /// it should update subscription and assert it information updated 
        public async Task UpdateSubscription()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport,player,trainer);
            //Act
            Subscription test_subscription = await subscriptionDataService!.Create(expected_subscription);
            Subscription actual_subscription = await subscriptionDataService.Get(test_subscription.Id);
            //actual_subscription.Trainer = await create_trainer();
            actual_subscription.TrainerId = null;
            actual_subscription.Trainer = null;
            Subscription updated_subscription = await subscriptionDataService.Update(actual_subscription);
            //Assert
            Assert.AreEqual(updated_subscription.Trainer, null);
        }
      
        [Test]
        /// it should try update not exist subscription and throw not exist exception
        public async Task UpdateNotExistSubsciption()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory!.FakeSubscription(sport , player,trainer);
            //Act
            expected_subscription.Price = 75000;
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await subscriptionDataService!.Update(expected_subscription));
        }
        [Test]
        /// it should delete subscription and assert it deleted
        public async Task DeleteSubsciption()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subsciption = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act
            Subscription test_subsciption = await subscriptionDataService!.Create(expected_subsciption);
            await subscriptionDataService.Delete(test_subsciption.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await subscriptionDataService!.Get(test_subsciption.Id));
        }

        [Test]
        /// it should try delete not exist subscription and throw not exist exception
        public async Task DeleteNotExistSubscription()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subsciption = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act

            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await subscriptionDataService!.Delete(expected_subsciption.Id));
        }
        [Test]
        /// it should List all Subscriptions
        public async Task ListAllSubscriptions()
        {
            //Arrange
            int count = 5;
            //Act
            await create_subscriptions(count);
            var subscription = await subscriptionDataService!.GetAll();
            //Assert
            Assert.AreEqual(subscription.Count(), count);
        }

        [Test]
        /// it should Move subscribtion to new trainer with save previous trainer id
        public async Task MoveSubscriptionToNewTrainer()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subsciption = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act
            Subscription created_subscription = await subscriptionDataService!.Create(expected_subsciption);
            Employee new_trainer = await create_trainer();
            Subscription get_subscription = await subscriptionDataService!.Get(created_subscription.Id);
            //Subscription subscription = await subscriptionDataService.MoveToNewTrainer(get_subscription, new_trainer, DateTime.Now);
            //Assert
            //Assert.AreEqual(subscription.PrevTrainer_Id, trainer.Id);
            //Assert.AreEqual(subscription.Trainer!.Id, new_trainer.Id);
        }
        [Test]
        /// it should thro exception because cannot move to new trainer twice
        public async Task Move_MovedSubscriptionToNewTrainer()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subsciption = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act
            Subscription created_subscription = await subscriptionDataService!.Create(expected_subsciption);
            Employee new_trainer = await create_trainer();
            Subscription get_subscription = await subscriptionDataService!.Get(created_subscription.Id);
            get_subscription.IsMoved = true;
            //Subscription subscription = await subscriptionDataService.MoveToNewTrainer(get_subscription, new_trainer, DateTime.Now);
            //Assert
            //Assert.ThrowsAsync<MovedBeforeException>(
               //async () => await subscriptionDataService!.MoveToNewTrainer(get_subscription, new_trainer, DateTime.Now));

        }

        [Test]
        /// it should stop subscription
        public async Task StopSubscription()
        {
            //Arrange
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subsciption = subscriptionFactory!.FakeSubscription(sport, player, trainer);
            //Act
            Subscription created_subscription = await subscriptionDataService!.Create(expected_subsciption);
            DateTime stop_date = DateTime.Now;
            Subscription get_subscription = await subscriptionDataService!.Get(expected_subsciption.Id);
            //Subscription stopped_subscription = await subscriptionDataService!.Stop(get_subscription, stop_date);

            //Assert
            int days = Convert.ToInt32((stop_date - created_subscription.RollDate).TotalDays);
            double dayPrice = created_subscription.PriceAfterOffer / created_subscription.Sport!.DaysCount;
            double price = dayPrice * days;
            //Assert.AreEqual(stopped_subscription.IsStopped, true);
            //Assert.AreEqual(stopped_subscription.EndDate, stop_date);
            //Assert.AreEqual(stopped_subscription.PriceAfterOffer, price);
        }
    }
}
