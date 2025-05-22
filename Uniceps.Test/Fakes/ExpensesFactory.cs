using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Uniceps.Core.Models.Expenses;

namespace Uniceps.Test.Fakes
{
    public class ExpensesFactory
    {
        public Expenses FakeExpenses()
        {
            var expensesFaker = new Faker<Expenses>()
              .StrictMode(false)
              .Rules((fake, expenses) =>
              {
                  expenses.Value = Convert.ToDouble(fake.Commerce.Price());
                  expenses.Description = fake.Lorem.Paragraph();
                  expenses.isManager = fake.Random.Bool();
                  expenses.date = fake.Date.Recent();
              });
            return expensesFaker;
        }
    }
}
