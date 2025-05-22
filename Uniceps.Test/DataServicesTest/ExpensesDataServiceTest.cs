using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Expenses;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Test.Fakes;

namespace Uniceps.Test.DataServicesTest
{
    [TestFixture]
    public class ExpensesDataServiceTest
    {
        UnicepsDbContextFactory? db;
        ExpensesFactory? expensesFactory;
        ExpensesDataService? expensesDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new UnicepsDbContextFactory(CONNECTION_STRING, false);

            using (UnicepsDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            expensesFactory = new();
            expensesDataService = new(db!);

        }

        [TearDown]
        public void TearDown()
        {
            using (UnicepsDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var expenses = platinumGymDbContext.Expenses!.ToList();
                platinumGymDbContext.Expenses!.RemoveRange(expenses);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Expenses!.Count();
            }
        }
        ////////////////////////////////////////
        /// 
        ///   H E L P E R  F U N C T I O N S
        /// 
        ///////////////////////////////

        private async Task create_expenses(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Expenses actual_expenses = await expensesDataService!
                    .Create(expensesFactory!.FakeExpenses());
            }
        }


        ////////////////////////////////
        ///
        /// T E S T  C A S E S
        /// 
        //////////////////////////
        [Test]
        //it sholud create an Expenses and assert that it created
        public async Task CreateExpenses()
        {
            Expenses expected_expenses = expensesFactory!.FakeExpenses();
            Expenses actual_expenses = await expensesDataService!.Create(expected_expenses);
            Assert.AreEqual(expected_expenses.Value, actual_expenses.Value);
        }

        [Test]
        // it should get a expenses 
        public async Task GetExpenses()
        {
            // Arrange
            Expenses expenses = expensesFactory!.FakeExpenses();
            // Act
            Expenses created_expenses = await expensesDataService!.Create(expenses);
            Expenses get_expenses = await expensesDataService!.Get(expenses.Id);
            // Assert
            Assert.AreEqual(created_expenses.Value, get_expenses.Value);
            Assert.AreEqual(created_expenses.Description, get_expenses.Description);

        }
        [Test]
        // it should to try get a not exist expenses and throw not exist exception
        public void GetNotExistExpenses()
        {
            // Arrange
            Expenses expenses = expensesFactory!.FakeExpenses();
            // Assert
            Assert.ThrowsAsync<NotExistException>(
                async () => await expensesDataService!.Get(expenses.Id));

        }
        [Test]
        // it should update a expenses and assert that it updated
        public async Task UpdatePayment()
        {
            // Arrange
            Expenses expenses = expensesFactory!.FakeExpenses();
            // Act
            Expenses created_expenses = await expensesDataService!.Create(expenses);
            Expenses get_expenses = await expensesDataService!.Get(expenses.Id);
            get_expenses.Value = 30000;
            Expenses updated_expenses = await expensesDataService.Update(get_expenses);
            // Assert
            Assert.AreEqual(updated_expenses.Value, 30000);

        }

        [Test]
        // it should to try update not existed expenses and throw not exist exception
        public void UpdateNotExistExpenses()
        {
            // Arrange

            Expenses expenses = expensesFactory!.FakeExpenses();
            // Act
            expenses.Value = 30000;
            // Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await expensesDataService!.Update(expenses));

        }

        [Test]
        // it should delete a expenses and assert that it deleted
        public async Task DeleteExpenses()
        {
            // Arrange
            Expenses expenses = expensesFactory!.FakeExpenses();
            Expenses created_expenses = await expensesDataService!.Create(expenses);
            // Act
            await expensesDataService.Delete(created_expenses.Id);
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await expensesDataService!.Get(created_expenses.Id));

        }
        [Test]
        // it should to try delete not existed expenses and throw not exist exception
        public void DeleteNotExistExpenses()
        {
            // Arrange
            Expenses expenses = expensesFactory!.FakeExpenses();
            //Assert
            Assert.ThrowsAsync<NotExistException>(
               async () => await expensesDataService!.Delete(expenses.Id));

        }
        [Test]
        // it should List all expenses
        public async Task GetAllExpenses()
        {
            // Arrange
            int count = 5;
            //Act
            await create_expenses(count);
            var expenses = await expensesDataService!.GetAll();
            //Assert
            Assert.AreEqual(expenses.Count(), count);

        }
        [Test]
        // it should List all expenses in specifice period
        public async Task GetPeriodExpenses()
        {
            // Arrange
            int count = 5;
            DateTime pstart = DateTime.Now;
            DateTime pend = DateTime.Now.AddMonths(1);
            //Act
            await create_expenses(count);
            //var expenses = await expensesDataService!.GetPeriodExpenses(pstart, pend);
            //Assert
            //foreach(var exp in expenses)
            //{
            //    Assert.GreaterOrEqual(exp.date, pstart);
            //    Assert.LessOrEqual(exp.date, pend);
            //}


        }

    }
}
