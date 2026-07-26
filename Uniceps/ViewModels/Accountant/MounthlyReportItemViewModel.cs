using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.ViewModels.Accountant
{
    public class MounthlyReportItemViewModel : ViewModelBase
    {
        public MonthlyReport monthlyReport;
        public double TotalIncome => monthlyReport.TotalIncome;
        public double IncomeForNextMonth => monthlyReport.IncomeForNextMonth;
        public double IncomeForThisMonth => monthlyReport.TotalIncome - monthlyReport.IncomeForNextMonth;
        public double IncomeFromLastMonth => monthlyReport.IncomeFromLastMonth;
        public double EarnNet => monthlyReport.TotalIncome - monthlyReport.IncomeForNextMonth + monthlyReport.IncomeFromLastMonth + monthlyReport.OtherRevenue;
        public double TrainerDauses => monthlyReport.TrainerDauses;
        public double Salaries => monthlyReport.Salaries;

        public double OtherRevenue => monthlyReport.OtherRevenue;

        public double Expenses => monthlyReport.Expenses;
        public double NetIncome => monthlyReport.NetIncome;
        public double PaidSalaries => monthlyReport.PaidSalaries;
        public double ActualNetIncome => monthlyReport.ActualNetIncome;
        public MounthlyReportItemViewModel(MonthlyReport monthlyReport)
        {
            this.monthlyReport = monthlyReport;
        }

        public void Update(MonthlyReport monthlyReport)
        {
            this.monthlyReport = monthlyReport;
        }
    }
}
