using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public async Task<Exercises> GetOrCreate(Exercises exercises)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            Exercises? exExercise = await _dbContext.Set<Exercises>().FirstOrDefaultAsync(x => x.Tid == exercises.Tid || x.Name == exercises.Name);
            if (exExercise != null)
            {
                exExercise.Version = exercises.Version;
                exExercise.MuscelEng = exercises.MuscelEng;
                exExercise.MuscelAr= exercises.MuscelAr;
                exExercise.ImageUrl = exercises.ImageUrl;
                exercises.Name = exercises.Name;
                _dbContext.Set<Exercises>().Update(exercises);
                return exExercise;
            }
            EntityEntry<Exercises> CreatedResult = await _dbContext.Set<Exercises>().AddAsync(exercises);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<MuscleGroup> GetOrCreateMuscleGroup(MuscleGroup muscleGroup)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            MuscleGroup? exMuscleGroup = await _dbContext.Set<MuscleGroup>().FirstOrDefaultAsync(x => x.PublicId == muscleGroup.PublicId);
            if (exMuscleGroup != null)
            {
                return exMuscleGroup;
            }
            EntityEntry<MuscleGroup> CreatedResult = await _dbContext.Set<MuscleGroup>().AddAsync(muscleGroup);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public Task<Exercises> Update(Exercises exercises)
        {
            throw new NotImplementedException();
        }
    }
}
