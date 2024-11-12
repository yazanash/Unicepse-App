using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;

namespace Unicepse.Core.Services
{
    public interface ISubscriptionDataService: IDataService<Subscription>
    {
        Task<IEnumerable<Subscription>> GetByDataStatus(DataStatus status);
        Task<Subscription> UpdateDataStatus(Subscription entity);
        Task<IEnumerable<Subscription>> GetAll(Player player);
        Task<IEnumerable<Subscription>> GetAll(Employee trainer);
        Task<IEnumerable<Subscription>> GetAll(Sport sport, DateTime date);
        Task<IEnumerable<Subscription>> GetAll(DateTime date);
        Task<IEnumerable<Subscription>> GetAll(Employee trainer, DateTime date);
        Task<IEnumerable<Subscription>> GetAllActive();
        Task<Subscription> Stop(Subscription entity, DateTime stop_date);
        Task<Subscription> MoveToNewTrainer(Subscription entity, Employee trainer, DateTime movedate);
    }
}
