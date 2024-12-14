using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.GetPlayerTransaction
{
    public class GetRoutineService : IGetPlayerTransactionService<PlayerRoutine>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public GetRoutineService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerRoutine>> GetAll(Player player)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).Include(x => x.Player).Where(x => x.Player!.Id == player.Id).ToListAsync();
                return entities;
            }
        }
    }
}
