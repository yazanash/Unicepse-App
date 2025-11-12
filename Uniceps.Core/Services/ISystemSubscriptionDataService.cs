using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.Core.Services
{
    public interface ISystemSubscriptionDataService
    {
        Task<SystemSubscription?> GetActiveSubscription();
        Task<SystemSubscription> Create(SystemSubscription entity);
        Task<SystemSubscription> Update(SystemSubscription entity);
       
    }
}
