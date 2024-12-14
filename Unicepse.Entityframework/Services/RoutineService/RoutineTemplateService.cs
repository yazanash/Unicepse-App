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
    public class RoutineTemplateService : IGetRoutineTemplatesService
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public RoutineTemplateService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<PlayerRoutine>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<PlayerRoutine>? entities = await context.Set<PlayerRoutine>().Include(x => x.RoutineSchedule).ThenInclude(x => x.Exercises).AsNoTracking().Where(x => x.IsTemplate == true).ToListAsync();
                return entities;
            }
        }
    }
}
