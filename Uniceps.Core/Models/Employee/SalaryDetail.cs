using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.Employee
{
    public class SalaryDetail
    {
        public string MonthName { get; set; } = string.Empty;
        public double BaseSalary { get; set; }
        public double EarnedAmount { get; set; }
        public string Note { get; set; } = string.Empty;
        public double ActualDue { get; set; }
    }
}
