using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API;
using Unicepse.API.Models;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Test.Fakes;

namespace Unicepse.Test.ApiDataService
{
    [TestFixture]
    public class SubscriptionApiDataServiceTest
    {
        SubscriptionFactory? subscriptionFactory;
        API.Services.SubscriptionApiDataService? subscriptionDataService;
        UnicepseApiPrepHttpClient? _client;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/")
                //BaseAddress = new Uri(" http://127.0.0.1:5000")
            };
            _client = new UnicepseApiPrepHttpClient(_httpClient, new UnicepsePrepAPIKey(""));
            subscriptionFactory = new();
            subscriptionDataService = new(_client);
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
        // it should create Subscription and return 201(created) status code 
        public async Task CreateSubscription()
        {
            Sport sport = new() { Name = "body building"};
            Employee trainer = new() { FullName = "yazan ash"};
            Subscription expected_subscription = subscriptionFactory!.FakeSubscriptionWithRandom(sport, trainer);
            //Act
            bool actual_subscription = await subscriptionDataService!.Create(expected_subscription);
            //Assert
            Assert.IsTrue(actual_subscription);
            Subscription created_subscription = await subscriptionDataService!.Get(expected_subscription);
            Assert.AreEqual(expected_subscription.Sport!.Name, created_subscription.Sport!.Name);
        }

        [Test]
        // it should update Subscription and return 200(Ok) status code
        public async Task UpdateSubscription()
        {
            //Arrange
            Sport sport = new() { Name = "body building" };
            Employee trainer = new() { FullName = "yazan ash" };
            Subscription expected_subscription = subscriptionFactory!.FakeSubscriptionWithRandom(sport, trainer);
            //Act
            bool actual_player = await subscriptionDataService!.Create(expected_subscription);
            //Assert
            Assert.IsTrue(actual_player);
            expected_subscription.Trainer!.FullName = "";
            bool updated_subscription = await subscriptionDataService!.Update(expected_subscription);
            //Assert
            Assert.IsTrue(updated_subscription);

            Subscription updated_subscription_dto = await subscriptionDataService!.Get(expected_subscription);
            Assert.AreEqual(expected_subscription.Trainer!.FullName, updated_subscription_dto.Trainer!.FullName);
        }

    }
}
