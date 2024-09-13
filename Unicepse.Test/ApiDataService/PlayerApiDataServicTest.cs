using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API;
using Unicepse.API.Models;
using Unicepse.Core.Models.Player;
using Unicepse.Test.Fakes;

namespace Unicepse.Test.ApiDataService
{
    [TestFixture]
    public class PlayerApiDataServicTest
    {
        PlayerFactory? playerFactory;
        API.Services.PlayerApiDataService? playerDataService;
        UnicepseApiPrepHttpClient? _client;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/")
            };
            _client = new UnicepseApiPrepHttpClient(_httpClient, new UnicepsePrepAPIKey(""));
            playerFactory = new();
            playerDataService = new(_client);
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
           
        }


        [Test]
        /// it should create player and assert that is created
        public async Task CreatePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayerWithId();
            //Act
            int actual_player = await playerDataService!.Create(expected_player);
            //Assert
            Assert.IsTrue(actual_player==201);
            Player created_player = await playerDataService!.Get(expected_player);
            Assert.AreEqual(expected_player.FullName, created_player.FullName);
        }
        [Test]
        /// it should create player and assert that is created
        public async Task UpdatePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayerWithId();
            //Act
            int actual_player = await playerDataService!.Create(expected_player);
            //Assert
            Assert.IsTrue(actual_player==201);
            expected_player.FullName = "yazan ash";
            int updated_player = await playerDataService!.Update(expected_player);
            //Assert
            Assert.IsTrue(updated_player==201);

            Player updated_player_dto = await playerDataService!.Get(expected_player);
            Assert.AreEqual(expected_player.FullName, updated_player_dto.FullName);
        }
    }
}
