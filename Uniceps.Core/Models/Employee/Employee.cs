using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Expenses;

namespace Uniceps.Core.Models.Employee
{
    public class Employee : Person
    {

        public bool Salary { get; set; }
        public bool Parcent { get; set; }
        public double SalaryValue { get; set; }
        public int ParcentValue { get; set; }
        public bool IsSecrtaria { get; set; }
        public string? Position { get; set; }
        public DateTime StartDate { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrainer { get; set; }
        public ICollection<Sport.Sport>? Sports { get; set; }
        public ICollection<Subscription.Subscription>? PlayerTrainings { get; set; }

        public Employee()
        {
            Sports = new HashSet<Sport.Sport>();
            PlayerTrainings = new HashSet<Subscription.Subscription>();
        }

        public void MergeWith(Employee employee)
        {
            FullName = employee.FullName;
            Phone = employee.Phone;
            BirthDate = employee.BirthDate;
            GenderMale = employee.GenderMale;
            Salary = employee.Salary;
            Parcent = employee.Parcent;
            SalaryValue = employee.SalaryValue;
            ParcentValue = employee.ParcentValue;
            IsSecrtaria = employee.IsSecrtaria;
            Position = employee.Position;
            StartDate = employee.StartDate;
            Balance = employee.Balance;
            IsActive = employee.IsActive;
            IsTrainer = employee.IsTrainer;
            SyncId = employee.SyncId;
        }
    }
}
