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
        Task<IEnumerable<Exercises>> GetAll();
        Task<IEnumerable<MuscleGroup>> GetAllMuscleGroups();
        Task<Exercises> GetOrCreate(Exercises exercises);
        Task<MuscleGroup> GetOrCreateMuscleGroup(MuscleGroup muscleGroup);
        Task<Exercises> Update(Exercises exercises);
    }
}
