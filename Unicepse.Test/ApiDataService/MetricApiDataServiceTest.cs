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
            int actual_metric = await metricDataService!.Create(expected_metric);
            //Assert
            Assert.IsTrue(actual_metric==201);
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
            int actual_player = await metricDataService!.Create(expected_metric);
            //Assert
            Assert.IsTrue(actual_player==201);
            expected_metric.Hieght = 180.0;
            int updated_metric = await metricDataService!.Update(expected_metric);
            //Assert
            Assert.IsTrue(updated_metric==200);

            Metric updated_metric_dto = await metricDataService!.Get(expected_metric);
            Assert.AreEqual(expected_metric.Hieght, updated_metric_dto.Hieght);
        }





    }
}
