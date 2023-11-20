using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Expenses;
using PlatinumGym.Entityframework.Services;
using Platinum.Test.Fakes;
using PlatinumGym.Entityframework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Platinum.Test.DataServicesTest
{
    [TestFixture]
    public class ExpensesDataServiceTest
    {
        PlatinumGymDbContextFactory? db;
        ExpensesFactory? expensesFactory;
        ExpensesDataService? expensesDataService;

        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            string CONNECTION_STRING = @"data source =.\sqlexpress; initial catalog = PlatinumDB_test; integrated security = SSPI; TrustServerCertificate = True; ";
            db = new PlatinumGymDbContextFactory(CONNECTION_STRING);

            using (PlatinumGymDbContext platinumGymDbContext = db.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }
            expensesFactory = new();
            expensesDataService = new(db!);

        }

        [TearDown]
        public void TearDown()
        {
            using (PlatinumGymDbContext platinumGymDbContext = db!.CreateDbContext())
            {
                var expenses = platinumGymDbContext.Expenses!.ToList();
                platinumGymDbContext.Expenses!.RemoveRange(expenses);
                platinumGymDbContext.SaveChanges();
                var x = platinumGymDbContext.Expenses!.Count();
            }
        }

        [Test]
        //it sholud create an Expenses and assert that it created
        public async Task CreateExpenses()
        {
            Expenses expected_expenses = expensesFactory!.FakeExpenses();
            Expenses actual_expenses = await expensesDataService!.Create(expected_expenses);
            Assert.AreEqual(expected_expenses.Value, actual_expenses.Value);
        }
        

    }
}
