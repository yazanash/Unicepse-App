using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API;
using Unicepse.API.Models;
using Unicepse.Core.Models.Metric;
using Unicepse.Test.Fakes;

namespace Unicepse.Test.ApiDataService
{
    [TestFixture]
    internal class MetricApiDataServiceTest
    {
        MetricFactory? metricFactory;
        API.Services.MetricApiDataService? metricDataService;
        UnicepseApiPrepHttpClient? _client;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/")
    
            };
            _client = new UnicepseApiPrepHttpClient(_httpClient, new UnicepsePrepAPIKey(""));
            metricFactory = new();
            metricDataService = new(_client);
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
        // it should create metric and return 201(created) status code 
        public async Task CreateMetric()
        {
            Metric expected_metric = metricFactory!.FakeMetricWithId();
            //expected_metric.Player = new Core.Models.Player.Player() {Id=1852369 };
            //expected_metric.Id = 123456789;
            //Act
            bool actual_metric = await metricDataService!.Create(expected_metric);
            //Assert
            Assert.IsTrue(actual_metric);
            Metric created_metric = await metricDataService!.Get(expected_metric);
            Assert.AreEqual(expected_metric.Hieght, created_metric.Hieght);
        }

        [Test]
        // it should update metric and return 200(Ok) status code
        public async Task UpdateMetric()
        {
            //Arrange
            Metric expected_metric = metricFactory!.FakeMetricWithId();
            //Act
            bool actual_player = await metricDataService!.Create(expected_metric);
            //Assert
            Assert.IsTrue(actual_player);
            expected_metric.Hieght = 180.0;
            bool updated_metric = await metricDataService!.Update(expected_metric);
            //Assert
            Assert.IsTrue(updated_metric);

            Metric updated_metric_dto = await metricDataService!.Get(expected_metric);
            Assert.AreEqual(expected_metric.Hieght, updated_metric_dto.Hieght);
        }





    }
}
