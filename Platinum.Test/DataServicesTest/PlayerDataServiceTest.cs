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

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class PlayerDataServiceTest
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
                var subscriptions = platinumGymDbContext.Subscriptions!.ToList();
                platinumGymDbContext.Subscriptions!.RemoveRange(subscriptions);
                var players = platinumGymDbContext.Players!.ToList();
                platinumGymDbContext.Players!.RemoveRange(players);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Players!.Count();
            }
        }

        /////////////////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        ///
        ///////////////////////////////////
        
        public async Task create_players(int count)
        {
            for(int i = 0; i < count; i++)
            {
                Player actual_player = await playerDataService!.Create(playerFactory!.FakePlayer());
            }
        }
        
        public async Task create_female_players(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Player player = playerFactory!.FakePlayer();
                player.GenderMale = false;
                Player actual_player = await playerDataService!.Create(player);
            }
        }
        public async Task create_players_withdebt(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Player player = playerFactory!.FakePlayer();
                player.Balance = 80000;
                Player actual_player = await playerDataService!.Create(player);
            }
        }
        /////////////////////////////////////////////
        ///
        /// T E S T  C A S E S
        ///
        ///////////////////////////////////
       
        [Test]
        /// it should create player and assert that is created
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
        /// it should try create an existing player and throw conflict exception exception
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
        /// it should get player info and assert it informations
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
        /// it should try get not exist player and throw player not exist exception 
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
        /// it should update player and assert it information updated 
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
        /// it should try update not exist player and throw player not exist exception
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
        /// it should delete player and assert it deleted
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
        /// it should try delete not exist player and throw player not exist exception
        public void DeleteNotExistPlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayer();
            //Act
            //Assert
            Assert.ThrowsAsync<PlayerNotExistException>(
               async () => await playerDataService!.Delete(expected_player.Id));
        }

        [Test]
        /// it should List all players
        public async Task ListAllPlayers()
        {
            //Arrange
            int count = 5;
            //Act
            await create_players(count);
            var players = await playerDataService!.GetAll();
            //Assert
            Assert.AreEqual(players.Count(), count);
        }

    
    }
}
