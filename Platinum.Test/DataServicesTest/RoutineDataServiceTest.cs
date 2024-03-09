using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.TrainingProgram;
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

    public class RoutineDataServiceTest
    {

        PlatinumGymDbContextFactory? db;
        RoutineFactory? routineFactory;
        RoutineItemFactory? routineItemFactory;
        PlayerFactory? playerFactory;
        PlayerDataService? playerDataService;
        PlayerRoutineDataService? routineDataService;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDBD; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            routineItemFactory = new();
            routineFactory = new(routineItemFactory);
            playerFactory = new();
            playerDataService = new(db!);
            routineDataService = new(db!);
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
            using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var routineitems = platinumGymDbContext.RoutineItems!.ToList();
                platinumGymDbContext.RoutineItems!.RemoveRange(routineitems);
                var routines = platinumGymDbContext.PlayerRoutine!.ToList();
                platinumGymDbContext.PlayerRoutine!.RemoveRange(routines);
                platinumGymDbContext.SaveChanges();
            }
        }
        ////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        /// 
        //////////////////////////

        private async Task create_routine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
                expected_routine.Player = await create_player();
                PlayerRoutine actual_routine = await routineDataService!.Create(expected_routine);
            }
        }


        public async Task<Player> create_player()
        {
            Player sport = playerFactory!.FakePlayer();
            return await playerDataService!.Create(sport);
        }
        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////

        [Test]
        //it sholud create an routine and assert that it created
        public async Task CreateRoutine()
        {
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            expected_routine.Player =await create_player();
            PlayerRoutine actual_routine = await routineDataService!.Create(expected_routine);
            Assert.AreEqual(expected_routine.RoutineNo, actual_routine.RoutineNo);
        }
        [Test]
        //it sholud try to create an existed routine and throw confilct exception
        public async Task CreateExistingRoutine()
        {
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            PlayerRoutine actual_routine = await routineDataService!.Create(expected_routine);
            Assert.ThrowsAsync<ConflictException>(
                () => routineDataService.Create(actual_routine));
        }

        [Test]
        /// it should get routine info and assert it informations
        public async Task GetRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act
            PlayerRoutine test_routine = await routineDataService!.Create(expected_routine);
            PlayerRoutine actual_routine = await routineDataService.Get(test_routine.Id);
            //Assert
            Assert.AreEqual(expected_routine.RoutineNo, actual_routine.RoutineNo);
            //Assert.AreEqual(expected_routine.RoutineSchedule.Count, actual_routine.RoutineSchedule.Count);
        }

        [Test]
        /// it should try get not exist routine and throw not exist exception 
        public void GetNotExistRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act

            //Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await routineDataService!.Get(expected_routine.Id));
        }

        [Test]
        /// it should update routine and assert it information updated 
        public async Task UpdateRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act
            PlayerRoutine test_routine = await routineDataService!.Create(expected_routine);
            PlayerRoutine actual_routine = await routineDataService.Get(test_routine.Id);
            actual_routine.RoutineNo = 1;
            PlayerRoutine updated_routine = await routineDataService.Update(actual_routine);
            //Assert
            Assert.AreEqual(actual_routine.RoutineNo, updated_routine.RoutineNo);
        }

        [Test]
        /// it should try update not exist routine and throw not exist exception
        public void UpdateNotExistRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act
            expected_routine.RoutineNo = 4;
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await routineDataService!.Update(expected_routine));
        }
        [Test]
        /// it should delete routine and assert it deleted
        public async Task DeleteRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act
            PlayerRoutine test_routine = await routineDataService!.Create(expected_routine);
            await routineDataService.Delete(test_routine.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await routineDataService!.Get(test_routine.Id));
        }

        [Test]
        /// it should try delete not exist routine and throw not exist exception
        public void DeleteNotExistRoutine()
        {
            //Arrange
            PlayerRoutine expected_routine = routineFactory!.FakeRoutine();
            //Act
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await routineDataService!.Delete(expected_routine.Id));
        }
        [Test]
        /// it should List All Routines
        public async Task ListAllRoutines()
        {
            //Arrange
            int count = 5;
            //Act
            await create_routine(count);
            var routines = await routineDataService!.GetAll();
            //Assert
            Assert.AreEqual(routines.Count(), count);
        }

    }
}
