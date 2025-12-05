using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Core.Services
{
    public interface IPeriodReportService
    {
        Task<IEnumerable<Expenses>> GetExpenses(DateTime from, DateTime to);
        Task<IEnumerable<PlayerPayment>> GetPayments(DateTime from, DateTime to);
    }
}
