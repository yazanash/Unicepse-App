using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.Employee
{
    public class EmployeeAccountClosing : DomainObject
    {
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime ReportDate { get; set; }  
        public double BalanceForwarded { get; set; }
        public double TotalSalaries { get; set; }
        public double TotalCommissions { get; set; }
        public double TotalCredits { get; set; }
        public string? Note { get; set; } 
    }
}
