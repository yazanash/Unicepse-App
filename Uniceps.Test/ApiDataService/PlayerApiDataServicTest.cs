using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.Core.Models.Player;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.ApiDataService
{
    [TestFixture]
    public class PlayerApiDataServicTest
    {
        PlayerFactory? playerFactory;
        PlayerApiDataService? playerDataService;
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
            PlayerDto playerDto = new PlayerDto();
            playerDto.FromPlayer(expected_player);
            int actual_player = await playerDataService!.Create(playerDto);
            //Assert
            Assert.IsTrue(actual_player == 201);
            PlayerDto created_player = await playerDataService!.Get(playerDto);
            Player player = created_player.ToPlayer();
            Assert.AreEqual(expected_player.FullName, player.FullName);
        }
        [Test]
        /// it should create player and assert that is created
        public async Task UpdatePlayer()
        {
            //Arrange
            Player expected_player = playerFactory!.FakePlayerWithId();
            //Act
            PlayerDto playerDto = new PlayerDto();
            playerDto.FromPlayer(expected_player);
            int actual_player = await playerDataService!.Create(playerDto);
            //Assert
            Assert.IsTrue(actual_player == 201);
            expected_player.FullName = "yazan ash";
            playerDto.FromPlayer(expected_player);
            int updated_player = await playerDataService!.Update(playerDto);
            //Assert
            Assert.IsTrue(updated_player == 201);

            PlayerDto updated_player_dto = await playerDataService!.Get(playerDto);
            Player player = updated_player_dto.ToPlayer();
            Assert.AreEqual(expected_player.FullName, player.FullName);
        }
    }
}
