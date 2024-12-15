using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.RoutineService
{
    public class RoutineItemsService : IRoutineItemsDataService
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public RoutineItemsService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> DeleteRoutineItems(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<RoutineItems>? entity = await context.Set<RoutineItems>().Where(x => x.PlayerRoutine!.Id == id).ToListAsync();

            context.Set<RoutineItems>().RemoveRange(entity);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
