using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.API.Responses
{
    public class EmployeeResponse
    {
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
        public long ServerVersion { get; set; }
        public Guid SyncId { get; set; }
        public Employee ToModel()
        {
            return new Employee
            {
                FullName = FullName,
                Phone = Phone,
                IsActive = IsActive,
                BirthDate = BirthDate,
                GenderMale = GenderMale,
                Salary = Salary,
                Parcent = Parcent,
                SalaryValue = SalaryValue,
                ParcentValue = ParcentValue,
                IsSecrtaria = IsSecrtaria,
                Position = Position,
                StartDate = StartDate,
                Balance = Balance,
                IsTrainer = IsTrainer,
                LastClosingBalance = LastClosingBalance,
                LastClosingDate = LastClosingDate,
                SyncId = SyncId,
                ServerVersion = ServerVersion
            };
        }
    }
}
