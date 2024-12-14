using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.DataSyncServices
{
    public class PaymentDataStatusSyncService : IDataStatusService<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public PaymentDataStatusSyncService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerPayment>> GetByDataStatus(DataStatus status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerPayment>? entities = await context.Set<PlayerPayment>().Where(x => x.DataStatus == status).Include(x => x.Player).AsNoTracking()
                    .Include(x => x.Subscription).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<PlayerPayment> UpdateDataStatus(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            PlayerPayment? dataToSync = await context.PlayerPayments!.FindAsync(entity.Id);
            if (dataToSync == null)
                throw new NotExistException("هذا السجل غير موجود");

            dataToSync.DataStatus = entity.DataStatus;
            context.Entry(dataToSync).Property(e => e.DataStatus).IsModified = true;
            await context.SaveChangesAsync();
            return entity;

        }
    }
}
