using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.RelationService
{
    public class SportTrainersDeleteConnection : IDeleteConnectionService<Sport>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public SportTrainersDeleteConnection(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> DeleteConnection(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().Include(x => x.Trainers).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            foreach (var trainer in entity.Trainers!)
            {
                context.Attach(trainer);
                entity.Trainers!.Remove(trainer);
            }

            context.Set<Sport>().Update(entity!);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
