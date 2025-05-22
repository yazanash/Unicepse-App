using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.Test.Fakes
{
    public class EmployeeFactory
    {
        public Employee FakeEmployee()
        {
            var employee_faker = new Faker<Employee>()
               .StrictMode(false)
               .Rules((fake, employee) =>
               {
                   employee.FullName = fake.Person.FullName;
                   employee.Phone = fake.Person.Phone;
                   employee.BirthDate = fake.Random.Number(1970, 2023);
                   employee.GenderMale = fake.Random.Bool();
                   employee.Balance = Convert.ToDouble(fake.Commerce.Price(10, 100));
                   employee.Parcent = fake.Random.Bool();
                   employee.Salary = fake.Random.Bool();
                   employee.SalaryValue = Convert.ToDouble(fake.Commerce.Price(100000, 1000000));
                   employee.ParcentValue = 25;
                   employee.IsTrainer = fake.Random.Bool();
                   employee.IsSecrtaria = fake.Random.Bool();
                   if (employee.IsTrainer)
                       employee.Position = "مدرب";
                   else if (employee.IsSecrtaria)
                       employee.Position = "سكرتارية";
                   else
                       employee.Position = "موظف";

                   employee.StartDate = fake.Date.Past(60);
               });
            return employee_faker;
        }
    }
}
