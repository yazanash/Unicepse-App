using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class SalaryDetailsViewModel:ViewModelBase
    {
        public SalaryDetail SalaryDetail;

        public SalaryDetailsViewModel(SalaryDetail salaryDetail)
        {
            SalaryDetail = salaryDetail;
        }
        public string MonthName => SalaryDetail.MonthName;
        public double BaseSalary => SalaryDetail.BaseSalary;
        public double EarnedAmount => SalaryDetail.EarnedAmount;
        public string Note => SalaryDetail.Note;
        public double ActualDue => SalaryDetail.ActualDue;
    }
}
