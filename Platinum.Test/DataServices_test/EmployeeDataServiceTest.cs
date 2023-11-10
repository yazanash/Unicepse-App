using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Platinum.Test.Fakes;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.DataServices_test
{
    [TestFixture]
    public class EmployeeDataServiceTest
    {

        PlatinumGymDbContextFactory db;
        EmployeeFactory employeeFactory;
        EmployeeDataService employeeDataService;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            employeeFactory = new();
            employeeDataService = new(db!);

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
            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                var employee = platinumGymDbContext.Employees!.ToList();
                platinumGymDbContext.Employees!.RemoveRange(employee);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Players!.Count();
            }
        }

        [Test]
        public async Task CreateEmployee()
        {
            Employee expected_employee = employeeFactory.FakeEmployee();
            Employee actual_employee = await employeeDataService.Create(expected_employee);
            Assert.AreEqual(expected_employee.FullName, actual_employee.FullName);
        }
        [Test]
        public async Task CreateExistingEmployee()
        {
            Employee expected_employee = employeeFactory.FakeEmployee();
            Employee actual_employee = await employeeDataService.Create(expected_employee);
            Assert.ThrowsAsync<ConflictException>(
                () => employeeDataService.Create(actual_employee));
        }
    }
}
