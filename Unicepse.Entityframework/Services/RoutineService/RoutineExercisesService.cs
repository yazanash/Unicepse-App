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
    public class RoutineExercisesService : IGetExercisesService
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public RoutineExercisesService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Exercises>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Exercises>? entities = await context.Set<Exercises>().AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
