using PlatinumGym.Core.Services;
using PlatinumGym.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Entityframework.DbContexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PlatinumGym.Entityframework.Services
{
    public class PaymentDataService : IDataService<PlayerPayment>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PaymentDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlayerPayment> Create(PlayerPayment entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            context.Attach(entity.Subscription!);
            context.Attach(entity.Player!);
            EntityEntry<PlayerPayment> CreatedResult = await context.Set<PlayerPayment>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PlayerPayment> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PlayerPayment>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PlayerPayment> Update(PlayerPayment entity)
        {
            throw new NotImplementedException();
        }
    }
}
