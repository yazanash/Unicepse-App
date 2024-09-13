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
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Test.Fakes;

namespace Unicepse.Test.ApiDataService
{
    [TestFixture]
    public class PaymentApiDataServiceTest
    {
        PaymentFactory? paymentFactory;
        SubscriptionFactory? subscriptionFactory;
        API.Services.PaymentApiDataService? paymentDataService;
        UnicepseApiPrepHttpClient? _client;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/")
            };
            _client = new UnicepseApiPrepHttpClient(_httpClient, new UnicepsePrepAPIKey(""));
            paymentFactory = new();
            paymentDataService = new(_client);
            subscriptionFactory = new();
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
        // it should create Payment and return 201(created) status code 
        public async Task CreatePayment()
        {
            Sport sport = new() { Name = "body building" };
            Employee trainer = new() { FullName = "yazan ash" };
            Subscription subscription = subscriptionFactory!.FakeSubscriptionWithRandom(sport,trainer);
            PlayerPayment expected_payment= paymentFactory!.FakePayments(subscription);
            //Act
            int actual_payment = await paymentDataService!.Create(expected_payment);
            //Assert
            Assert.IsTrue(actual_payment==201);
            PlayerPayment created_payment = await paymentDataService!.Get(expected_payment);
            Assert.AreEqual(expected_payment.PaymentValue, created_payment.PaymentValue);
        }

        [Test]
        // it should update Payment and return 200(Ok) status code
        public async Task UpdatePayment()
        {
            //Arrange
            Sport sport = new() { Name = "body building" };
            Employee trainer = new() { FullName = "yazan ash" };
            Subscription subscription = subscriptionFactory!.FakeSubscriptionWithRandom(sport, trainer);
            PlayerPayment expected_payment = paymentFactory!.FakePayments(subscription);
            //Act
            int actual_player = await paymentDataService!.Create(expected_payment);
            //Assert
            Assert.IsTrue(actual_player==201);
            expected_payment.PaymentValue=180000;
            int updated_subscription = await paymentDataService!.Update(expected_payment);
            //Assert
            Assert.IsTrue(updated_subscription==200);

            PlayerPayment updated_payment_dto = await paymentDataService!.Get(expected_payment);
            Assert.AreEqual(expected_payment.PaymentValue, updated_payment_dto.PaymentValue);
        }
    }
}
