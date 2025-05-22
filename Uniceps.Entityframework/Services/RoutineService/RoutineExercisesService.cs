using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.RoutineService
{
    public class RoutineExercisesService : IGetExercisesService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public RoutineExercisesService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Exercises>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Exercises>? entities = await context.Set<Exercises>().AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<MuscleGroup>> GetAllMuscleGroups()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<MuscleGroup>? entities = await context.Set<MuscleGroup>().AsNoTracking().ToListAsync();
                return entities;
            }
        }
    }
}
