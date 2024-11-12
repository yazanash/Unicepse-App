using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;

namespace Unicepse.Core.Services
{
    public interface IPaymentDataService : IDataService<PlayerPayment>
    {
        Task<IEnumerable<PlayerPayment>> GetByDataStatus(DataStatus status);
        Task<PlayerPayment> UpdateDataStatus(PlayerPayment entity);
        Task<IEnumerable<PlayerPayment>> GetPlayerPayments(Player player);
        Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<PlayerPayment>> GetAll(DateTime dateFrom);
        Task<IEnumerable<PlayerPayment>> GetTrainerPayments(Employee trainer, DateTime date);
    }
}
