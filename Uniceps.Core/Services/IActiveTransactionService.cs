using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Services
{
    public interface IActiveTransactionService
    {
        Task<IEnumerable<Subscription>> GetAllByDate(DateTime date, int? sportId, int? trainerId);
    }
}
