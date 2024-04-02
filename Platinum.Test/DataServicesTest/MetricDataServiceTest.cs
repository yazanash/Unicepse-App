using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Metric;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Entityframework.Services;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class MetricDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
        MetricFactory? metricsFactory;
        MetricDataService? metricDataService;
        PlayerDataService? playerDataService;
        PlayerFactory? playerFactory;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            metricsFactory = new();
            metricDataService = new(db!);
            playerDataService = new(db!);
            playerFactory = new();

        }

        [TearDown]
        public void TearDown()
        {
            using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var metric = platinumGymDbContext.Metrics!.ToList();
                platinumGymDbContext.Metrics!.RemoveRange(metric);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Metrics!.Count();
            }
        }
        ////////////////////////////////////////
        /// 
        ///   H E L P E R  F U N C T I O N S
        /// 
        ///////////////////////////////

        private async Task create_metric(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Metric metric = metricsFactory!.FakeMetric();
                    metric.Player = await create_player();
                Metric actual_metric = await metricDataService!
                    .Create(metric);
            }
        }

        public async Task<Player> create_player()
        {
            Player player = playerFactory!.FakePlayer();
            return await playerDataService!.Create(player);
        }
        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////
        [Test]
        //it sholud create an Metrics record and assert that it created
        public async Task CreateMetric()
        {
            Metric expected_metrics = metricsFactory!.FakeMetric();
            expected_metrics.Player = await create_player();
            Metric actual_metrics = await metricDataService!.Create(expected_metrics);
            Assert.AreEqual(expected_metrics.Wieght, actual_metrics.Wieght);
        }

        [Test]
        // it should get a Metrics record 
        public async Task GetMetric()
        {
            // Arrange
            Metric metric = metricsFactory!.FakeMetric();
            metric.Player = await create_player();
            // Act
            Metric created_metric = await metricDataService!.Create(metric);
            Metric get_metric = await metricDataService!.Get(metric.Id);
            // Assert
            Assert.AreEqual(created_metric.Wieght, get_metric.Wieght);
            Assert.AreEqual(created_metric.Hieght, get_metric.Hieght);

        }
        [Test]
        // it should to try get a not exist Metrics record and throw not exist exception
        public void GetNotExistMetric()
        {
            // Arrange
            Metric metrics = metricsFactory!.FakeMetric();
            // Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await metricDataService!.Get(metrics.Id));

        }
        [Test]
        // it should update a Metrics record and assert that it updated
        public async Task UpdateMetric()
        {
            // Arrange
            Metric metric = metricsFactory!.FakeMetric();
            metric.Player = await create_player();
            // Act
            Metric created_metric = await metricDataService!.Create(metric);
            Metric get_metric = await metricDataService!.Get(metric.Id);
            get_metric.Chest = 30000;
            Metric updated_expenses = await metricDataService.Update(get_metric);
            // Assert
            Assert.AreEqual(updated_expenses.Chest, 30000);

        }

        [Test]
        // it should to try update not existed Metrics record and throw not exist exception
        public void UpdateNotExistMetric()
        {
            // Arrange

            Metric metric = metricsFactory!.FakeMetric();
            // Act
            metric.Chest = 30000;
            // Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await metricDataService!.Update(metric));

        }

        [Test]
        // it should delete a Metrics record and assert that it deleted
        public async Task DeleteMetric()
        {
            // Arrange
            Metric metric = metricsFactory!.FakeMetric();
            metric.Player = await create_player();
            Metric created_metric = await metricDataService!.Create(metric);
            // Act
            await metricDataService.Delete(created_metric.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await metricDataService!.Get(created_metric.Id));

        }
        [Test]
        // it should to try delete not existed Metrics record and throw not exist exception
        public void DeleteNotExistMetric()
        {
            // Arrange
            Metric metric = metricsFactory!.FakeMetric();
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await metricDataService!.Delete(metric.Id));

        }
        [Test]
        // it should List all Metrics records
        public async Task GetAllMetrics()
        {
            // Arrange
            int count = 5;
            //Act
            await create_metric(count);
            var expenses = await metricDataService!.GetAll();
            //Assert
            Assert.AreEqual(expenses.Count(), count);

        }
      
    }
}
