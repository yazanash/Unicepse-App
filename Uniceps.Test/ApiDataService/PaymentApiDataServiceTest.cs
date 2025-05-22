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
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Test.Fakes;
using Uniceps.Core.Models.Player;

namespace Uniceps.Test.ApiDataService
{
    [TestFixture]
    public class PaymentApiDataServiceTest
    {
        PaymentFactory? paymentFactory;
        SubscriptionFactory? subscriptionFactory;
        PaymentApiDataService? paymentDataService;
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
            Subscription subscription = subscriptionFactory!.FakeSubscriptionWithRandom(sport, trainer);
            PlayerPayment expected_payment = paymentFactory!.FakePayments(subscription);
            //Act
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(expected_payment);
            int actual_payment = await paymentDataService!.Create(paymentDto);
            //Assert
            Assert.IsTrue(actual_payment == 201);
            PaymentDto created_payment = await paymentDataService!.Get(paymentDto);
            PlayerPayment pay = created_payment.ToPayment();
            Assert.AreEqual(expected_payment.PaymentValue, pay.PaymentValue);
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
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(expected_payment);
            int actual_player = await paymentDataService!.Create(paymentDto);
            //Assert
            Assert.IsTrue(actual_player == 201);
            expected_payment.PaymentValue = 180000;
            paymentDto.FromPayment(expected_payment);
            int updated_subscription = await paymentDataService!.Update(paymentDto);
            //Assert
            Assert.IsTrue(updated_subscription == 200);

            PaymentDto updated_payment_dto = await paymentDataService!.Get(paymentDto);
            PlayerPayment pay = updated_payment_dto.ToPayment();
            Assert.AreEqual(expected_payment.PaymentValue, pay.PaymentValue);
        }
    }
}
