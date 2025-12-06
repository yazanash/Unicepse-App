using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Services
{
    public interface IDailyReportService
    {
        Task<IEnumerable<Credit>> GetCredits(DateTime date);
        Task<IEnumerable<Expenses>> GetExpenses(DateTime date);
        Task<IEnumerable<PlayerPayment>> GetPayments(DateTime date);
        Task<IEnumerable<Subscription>> GetSubscriptions(DateTime date);
    }
}
