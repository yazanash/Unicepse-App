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
        public async Task<IEnumerable<ExerciseV2>> GetAll()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<ExerciseV2>? entities = await context.Set<ExerciseV2>().Where(x => !x.IsLegacy).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipments()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Equipment>? entities = await context.Set<Equipment>().AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<MuscleGroupV2>> GetAllMuscleGroups()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<MuscleGroupV2>? entities = await context.Set<MuscleGroupV2>().Include(x => x.Heads).AsNoTracking().ToListAsync();
                return entities;
            }
        }

        public async Task<int> GetExerciseVersion(string exerciseId)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            ExerciseV2? exExercise = await _dbContext.Set<ExerciseV2>().FirstOrDefaultAsync(x => x.ExerciseId == exerciseId);
            return exExercise?.Version ?? -1;
        }

        public async Task<ExerciseV2> GetOrCreate(ExerciseV2 exercises)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            ExerciseV2? exExercise = await _dbContext.Set<ExerciseV2>().FirstOrDefaultAsync(x => x.ExerciseId == exercises.ExerciseId);
            if (exExercise != null)
            {
                exExercise.Version = exercises.Version;
                exExercise.MuscleGroupCode = exercises.MuscleGroupCode;
                exExercise.MuscleHeadCode = exercises.MuscleHeadCode;
                exExercise.ImagePath = exercises.ImagePath;
                exExercise.Name = exercises.Name;
                exExercise.MuscleAux1 = exercises.MuscleAux1;
                exExercise.MuscleAux2 = exercises.MuscleAux2;
                exExercise.MuscleAux3 = exercises.MuscleAux3;
                exExercise.Description = exercises.Description;
                exExercise.EquipmentCode = exercises.EquipmentCode;
                exExercise.IsActive = true;
                exExercise.IsLegacy = false;
                exExercise.Mechanism = exercises.Mechanism;
                _dbContext.Set<ExerciseV2>().Update(exercises);
                return exExercise;
            }
            EntityEntry<ExerciseV2> CreatedResult = await _dbContext.Set<ExerciseV2>().AddAsync(exercises);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<Equipment> GetOrCreateEquipments(Equipment equipment)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            Equipment? exEquipment = await _dbContext.Set<Equipment>().FirstOrDefaultAsync(x => x.Code == equipment.Code);
            if (exEquipment != null)
            {
                exEquipment.Name = equipment.Name;
                _dbContext.Set<Equipment>().Update(exEquipment);
                return exEquipment;
            }
            EntityEntry<Equipment> CreatedResult = await _dbContext.Set<Equipment>().AddAsync(equipment);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<MuscleGroupV2> GetOrCreateMuscleGroup(MuscleGroupV2 muscleGroup)
        {
            using UnicepsDbContext _dbContext = _contextFactory.CreateDbContext();
            MuscleGroupV2? exMuscleGroup = await _dbContext.Set<MuscleGroupV2>().FirstOrDefaultAsync(x => x.Code == muscleGroup.Code);
            if (exMuscleGroup != null)
            {
                exMuscleGroup.Name = muscleGroup.Name;
                _dbContext.Set<MuscleGroupV2>().Update(exMuscleGroup);
                return exMuscleGroup;
            }
            EntityEntry<MuscleGroupV2> CreatedResult = await _dbContext.Set<MuscleGroupV2>().AddAsync(muscleGroup);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public Task<ExerciseV2> Update(ExerciseV2 exercises)
        {
            throw new NotImplementedException();
        }
    }
}
