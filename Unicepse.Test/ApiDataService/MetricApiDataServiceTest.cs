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
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(expected_metric);
            int actual_metric = await metricDataService!.Create(metricDto);
            //Assert
            Assert.IsTrue(actual_metric==201);
            MetricDto created_metric = await metricDataService!.Get(metricDto);
            Metric metric = created_metric.ToMetric();
            Assert.AreEqual(expected_metric.Hieght, metric.Hieght);
        }

        [Test]
        // it should update metric and return 200(Ok) status code
        public async Task UpdateMetric()
        {
            //Arrange
            Metric expected_metric = metricFactory!.FakeMetricWithId();
            //Act
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(expected_metric);
            int actual_player = await metricDataService!.Create(metricDto);
            //Assert
            Assert.IsTrue(actual_player==201);
            expected_metric.Hieght = 180.0;
            metricDto.FromMetric(expected_metric);
            int updated_metric = await metricDataService!.Update(metricDto);
            //Assert
            Assert.IsTrue(updated_metric==200);

            MetricDto updated_metric_dto = await metricDataService!.Get(metricDto);
            Metric metric = updated_metric_dto.ToMetric();
            Assert.AreEqual(expected_metric.Hieght, metric.Hieght);
        }





    }
}
