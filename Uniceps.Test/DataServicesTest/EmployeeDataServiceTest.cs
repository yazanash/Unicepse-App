using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.DataServicesTest
{
    [TestFixture]
    public class EmployeeDataServiceTest
    {

        UnicepsDbContextFactory? db;
        EmployeeFactory? employeeFactory;
        EmployeeDataService? employeeDataService;
        SportFactory? sportFactory;
        SportServices? sportDataService;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new UnicepsDbContextFactory(CONNECTION_STRING, false);

            using (UnicepsDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            employeeFactory = new();
            employeeDataService = new(db!);

            sportFactory = new();
            sportDataService = new(db!);
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
            using (UnicepsDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var payments = platinumGymDbContext.PlayerPayments!.ToList();
                platinumGymDbContext.PlayerPayments!.RemoveRange(payments);
                var subscriptions = platinumGymDbContext.Subscriptions!.ToList();
                platinumGymDbContext.Subscriptions!.RemoveRange(subscriptions);
                var employee = platinumGymDbContext.Employees!.ToList();
                platinumGymDbContext.Employees!.RemoveRange(employee);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Employees!.Count();
            }
        }
        ////////////////////////////////
        ///
        /// H E L P E R  F U N C T I O N S
        /// 
        //////////////////////////

        private async Task create_employee(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Employee actual_employee = await employeeDataService!.Create(employeeFactory!.FakeEmployee());
            }
        }


        public async Task<Sport> create_sport()
        {
            Sport sport = sportFactory!.FakeSport();
            return await sportDataService!.Create(sport);
        }
        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////

        [Test]
        //it sholud create an employee and assert that it created
        public async Task CreateEmployee()
        {
            Employee expected_employee = employeeFactory!.FakeEmployee();
            Employee actual_employee = await employeeDataService!.Create(expected_employee);
            Assert.Equals(expected_employee.FullName!, actual_employee.FullName!);
        }
        [Test]
        //it sholud create an employee and assert that it created
        public async Task CreateEmployeeWithSport()
        {
            Employee expected_employee = employeeFactory!.FakeEmployee();
            Sport sport = await create_sport();
            expected_employee.Sports!.Add(sport);
            Employee actual_employee = await employeeDataService!.Create(expected_employee);
            Assert.Equals(expected_employee.Sports.Count, actual_employee.Sports!.Count);
        }
        [Test]
        //it sholud try to create ana existed employee and throw confilct exception
        public async Task CreateExistingEmployee()
        {
            Employee expected_employee = employeeFactory!.FakeEmployee();
            Employee actual_employee = await employeeDataService!.Create(expected_employee);
            Assert.ThrowsAsync<ConflictException>(
                () => employeeDataService.Create(actual_employee));
        }

        [Test]
        /// it should get Employee info and assert it informations
        public async Task GetEmployee()
        {
            //Arrange
            Employee expected_employee = employeeFactory!.FakeEmployee();
            //Act
            Employee test_employee = await employeeDataService!.Create(expected_employee);
            Employee actual_employee = await employeeDataService.Get(test_employee.Id);
            //Assert
            Assert.Equals(expected_employee.FullName!, actual_employee.FullName!);
        }

        [Test]
        /// it should try get not exist Employee and throw not exist exception 
        public void GetNotExistEmployee()
        {
            //Arrange
            Employee expected_employee = employeeFactory!.FakeEmployee();
            //Act

            //Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await employeeDataService!.Get(expected_employee.Id));
        }

        [Test]
        /// it should update Employee and assert it information updated 
        public async Task UpdateEmployee()
        {
            //Arrange
            Employee expected_player = employeeFactory!.FakeEmployee();
            //Act
            Employee test_player = await employeeDataService!.Create(expected_player);
            Employee actual_employee = await employeeDataService.Get(test_player.Id);
            actual_employee.FullName = "updated Name";
            Employee updated_player = await employeeDataService.Update(actual_employee);
            //Assert
            Assert.Equals(actual_employee.FullName, updated_player.FullName!);
        }

        [Test]
        /// it should try update not exist Employee and throw not exist exception
        public void UpdateNotExistPlayer()
        {
            //Arrange
            Employee expected_player = employeeFactory!.FakeEmployee();
            //Act
            expected_player.FullName = "updated Name";
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await employeeDataService!.Update(expected_player));
        }
        [Test]
        /// it should delete Employee and assert it deleted
        public async Task DeleteEmployee()
        {
            //Arrange
            Employee expected_emp = employeeFactory!.FakeEmployee();
            //Act
            Employee test_emp = await employeeDataService!.Create(expected_emp);
            await employeeDataService.Delete(test_emp.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await employeeDataService!.Get(test_emp.Id));
        }

        [Test]
        /// it should try delete not exist Employee and throw not exist exception
        public void DeleteNotExistEmployee()
        {
            //Arrange
            Employee expected_player = employeeFactory!.FakeEmployee();
            //Act
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await employeeDataService!.Delete(expected_player.Id));
        }
        [Test]
        /// it should List All Employees
        public async Task ListAllEmployees()
        {
            //Arrange
            int count = 5;
            //Act
            await create_employee(count);
            var employees = await employeeDataService!.GetAll();
            //Assert
            Assert.Equals(employees.Count(), count);
        }

    }
}
