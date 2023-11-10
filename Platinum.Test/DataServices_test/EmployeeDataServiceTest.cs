﻿using Microsoft.EntityFrameworkCore;
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
                var x = platinumGymDbContext.Employees!.Count();
            }
        }

        private async Task create_employee(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Employee actual_employee = await employeeDataService!.Create(employeeFactory!.FakeEmployee());
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

        [Test]
        /// it should get player info and assert it informations
        public async Task GetEmployee()
        {
            //Arrange
            Employee expected_employee = employeeFactory!.FakeEmployee();
            //Act
            Employee test_employee = await employeeDataService!.Create(expected_employee);
            Employee actual_employee = await employeeDataService.Get(test_employee.Id);
            //Assert
            Assert.AreEqual(expected_employee.FullName, actual_employee.FullName);
        }

        [Test]
        /// it should try get not exist player and throw exception 
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
        /// it should update player and assert it information updated 
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
            Assert.AreEqual(actual_employee.FullName, updated_player.FullName);
        }

        [Test]
        /// it should try update not exist player and throw exception
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
        /// it should delete player and assert it deleted
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
        /// it should try delete not exist player and throw exception
        public void DeleteNotExistEmployee()
        {
            //Arrange
            Employee expected_player = employeeFactory!.FakeEmployee();
            //Act
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await employeeDataService.Delete(expected_player.Id));
        }
        [Test]
        public async Task ListAllEmployees()
        {
            //Arrange
            int count = 5;
            //Act
            await create_employee(count);
            var employees = await employeeDataService.GetAll();
            //Assert
            Assert.AreEqual(employees.Count(), count);
        }

    }
}
