using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Core.Services
{
    public interface ISubscriptionRenewService
    {
        Task<bool> MarkAsRenewed(int entityId);
        Task<IEnumerable<Subscription>> GetAll(int daysPastEnd = 0);
    }
}
