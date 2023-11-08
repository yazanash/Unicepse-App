using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Core.Models.Player;
using Microsoft.EntityFrameworkCore;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;

namespace Platinum.Test.DataServices_test
{
    [TestFixture]
    public class PlayerDataService_test
    {
        PlatinumGymDbContextFactory? db;
        PlayerFactory? playerFactory;
        PlayerDataService? playerDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            playerFactory = new();
            playerDataService = new(db!);
        }

        [OneTimeTearDown]
        public void Onetimetear()
        {
            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                var players = platinumGymDbContext.Players.ToList();
                platinumGymDbContext.Players.RemoveRange(players);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Players.Count();
            }
        }

        [SetUp]
        public void SetUp()
        {

        }
        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public async Task CreatePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            Player actual_player = await playerDataService!.Create(expected_player);
            //Assert
            Assert.AreEqual(expected_player.FullName, actual_player.FullName);
        }

        [Test]
        public async Task CreateExistingPlayer()
        {
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            Player test_player = await playerDataService!.Create(expected_player);
            //Assert
            Assert.ThrowsAsync<PlayerConflictException>(
                () => playerDataService.Create(test_player));

        }

        [Test]
        public async Task GetPlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            Player test_player = await playerDataService!.Create(expected_player);
            Player actual_player = await playerDataService.Get(test_player.Id);
            //Assert
            Assert.AreEqual(expected_player.FullName, actual_player.FullName);
        }

        [Test]
        public void GetNotExistPlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act

            //Assert
            Assert.ThrowsAsync<PlayerNotExistException>(
                async () => await playerDataService!.Get(expected_player.Id));
        }
        [Test]
        public async Task UpdatePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            Player test_player = await playerDataService!.Create(expected_player);
            Player actual_player = await playerDataService.Get(test_player.Id);
            actual_player.FullName = "updated Name";
            Player updated_player = await playerDataService.Update(actual_player);
            //Assert
            Assert.AreEqual(actual_player.FullName, updated_player.FullName);
        }

        [Test]
        public void UpdateNotExistPlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            expected_player.FullName = "updated Name";
            //Assert
            Assert.ThrowsAsync<PlayerNotExistException>(
               async () => await playerDataService!.Update(expected_player));
        }

        [Test]
        public async Task DeletePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            Player test_player = await playerDataService!.Create(expected_player);
            await playerDataService.Delete(test_player.Id);
            //Assert
            Assert.ThrowsAsync<PlayerNotExistException>(
               async () => await playerDataService!.Get(test_player.Id));
        }

        [Test]
        public void DeleteNotExistPlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            //Assert
            Assert.ThrowsAsync<PlayerNotExistException>(
               async () => await playerDataService.Delete(expected_player.Id));
        }
    }
}
