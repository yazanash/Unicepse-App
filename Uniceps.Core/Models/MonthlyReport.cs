using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models
{
    public class MonthlyReport
    {
        public double TotalIncome { get; set; }
        public double IncomeForNextMonth { get; set; }
        public double IncomeFromLastMonth { get; set; }
        public double TrainerDauses { get; set; }
        public double Salaries { get; set; }
        public double Expenses { get; set; }
        public double NetIncome { get; set; }
    }
}
