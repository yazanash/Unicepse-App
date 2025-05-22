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
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.Player;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.ApiDataService
{
    [TestFixture]
    public class SubscriptionApiDataServiceTest
    {
        SubscriptionFactory? subscriptionFactory;
        SubscriptionApiDataService? subscriptionDataService;
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
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(expected_subscription);
            int actual_subscription = await subscriptionDataService!.Create(subscriptionDto);
            //Assert
            Assert.IsTrue(actual_subscription==201);
            SubscriptionDto created_subscription = await subscriptionDataService!.Get(subscriptionDto);
            Subscription sub = created_subscription.ToSubscription();
            Assert.AreEqual(expected_subscription.Sport!.Name, sub.Sport!.Name);
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
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(expected_subscription);
            int actual_player = await subscriptionDataService!.Create(subscriptionDto);
            //Assert
            Assert.IsTrue(actual_player==201);
            expected_subscription.Trainer!.FullName = "";
            subscriptionDto.FromSubscription(expected_subscription);
            int updated_subscription = await subscriptionDataService!.Update(subscriptionDto);
            //Assert
            Assert.IsTrue(updated_subscription==200);

            SubscriptionDto updated_subscription_dto = await subscriptionDataService!.Get(subscriptionDto);
            Subscription sub = updated_subscription_dto.ToSubscription();
            Assert.AreEqual(expected_subscription.Trainer!.FullName, sub.Trainer!.FullName);
        }

    }
}
