using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.API.Requests
{
    public class EmployeeRequest
    {
        public EmployeeRequest(Employee employee)
        {
            FullName = employee.FullName;
            Phone = employee.Phone;
            IsActive = employee.IsActive;
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
            IsTrainer = employee.IsTrainer;
            LastClosingBalance = employee.LastClosingBalance;
            LastClosingDate = employee.LastClosingDate;
        }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
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

        public double LastClosingBalance { get; set; }
        public DateTime? LastClosingDate { get; set; }
        public Guid SyncId { get; set; }
    }
}
