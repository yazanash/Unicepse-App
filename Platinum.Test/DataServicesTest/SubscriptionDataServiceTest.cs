﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Entityframework.Services;
using PlatinumGym.Entityframework.DbContexts;
using Platinum.Test.Fakes;
using Microsoft.EntityFrameworkCore;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Exceptions;

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class SubscriptionDataServiceTest
    {
        PlatinumGymDbContextFactory db;
        SubscriptionFactory subscriptionFactory;
        PlayerFactory playerFactory;
        SportFactory sportFactory;
        EmployeeFactory employeeFactory;
        SubscriptionDataService subscriptionDataService;
        PlayerDataService playerDataService;
        SportServices sportDataService;
        EmployeeDataService employeeDataService;

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
            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
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

        public async Task<Player> create_player()
        {
            Player player = playerFactory.FakePlayer();
            return await playerDataService.Create(player);
        }
        public async Task<Sport> create_sport()
        {
            Sport sport = sportFactory.FakeSport();
            return await sportDataService.Create(sport);
        }
        public async Task<Employee> create_trainer()
        {
            Employee trainer = employeeFactory.FakeEmployee();
            trainer.IsTrainer = true;
            return await employeeDataService.Create(trainer);
        }

        ///////////////////////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////////////////////////////

        [Test]
        public async Task CreateSubscription()
        {
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory.FakeSubscription(sport, player, trainer);
            Subscription actual_suscription = await subscriptionDataService.Create(expected_subscription);
            Assert.AreEqual(expected_subscription.Player.FullName, actual_suscription.Player.FullName);
            Assert.AreEqual(expected_subscription.Sport.Name, actual_suscription.Sport.Name);
        }
        [Test]
        public async Task CreateExisteingSubscription()
        {
            Player player = await create_player();
            Sport sport = await create_sport();
            Employee trainer = await create_trainer();
            Subscription expected_subscription = subscriptionFactory.FakeSubscription(sport, player, trainer);
            Subscription existed_suscription = await subscriptionDataService.Create(expected_subscription);
            Subscription recreate_subscription = subscriptionFactory.FakeSubscription(sport, player, trainer);
            Assert.ThrowsAsync<ConflictException>(
                () => subscriptionDataService.Create(recreate_subscription));
        }
    }
}
