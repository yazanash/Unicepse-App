using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Core.Services
{
    public interface IGetExercisesService
    {
        Task<IEnumerable<ExerciseV2>> GetAll();
        Task<IEnumerable<MuscleGroupV2>> GetAllMuscleGroups();
        Task<IEnumerable<Equipment>> GetAllEquipments();
        Task<ExerciseV2> GetOrCreate(ExerciseV2 exercises);
        Task<MuscleGroupV2> GetOrCreateMuscleGroup(MuscleGroupV2 muscleGroup);
        Task<Equipment> GetOrCreateEquipments(Equipment equipment);
        Task<ExerciseV2> Update(ExerciseV2 exercises);
        Task<int> GetExerciseVersion(string exerciseId);
    }
}
