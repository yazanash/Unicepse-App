using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Core.Services;

namespace Uniceps.Stores.RoutineStores
{
    public class ExercisesDataStore
    {
        string LogFlag = "[Routine] ";

        private readonly IGetExercisesService _getExercisesService;
        private readonly ILogger<ExercisesDataStore> _logger;
        private readonly List<Exercises> _exercises;
        public IEnumerable<Exercises> Exercises => _exercises;

        private readonly List<MuscleGroup> _muscleGroups;
        public IEnumerable<MuscleGroup> MuscleGroups => _muscleGroups;
        public event Action? ExercisesLoaded;
        public event Action? MuscleGroupsLoaded;
        public event Action? SelectedMuscleChanged;

        private MuscleGroup? _selectedMuscle;
        public MuscleGroup? SelectedMuscle
        {
            get { return _selectedMuscle; }
            set { _selectedMuscle = value; SelectedMuscleChanged?.Invoke(); }
        }
        public ExercisesDataStore(IGetExercisesService getExercisesService, ILogger<ExercisesDataStore> logger)
        {
            _getExercisesService = getExercisesService;
            _logger = logger;
            _exercises = new List<Exercises>();
            _muscleGroups = new List<MuscleGroup>();
        }


        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all exercises");
            IEnumerable<Exercises> routines = await _getExercisesService.GetAll();
            _exercises.Clear();
            _exercises.AddRange(routines);
            ExercisesLoaded?.Invoke();
        }

        public async Task GetAllMuscleGroups()
        {
            _logger.LogInformation(LogFlag + "get all muscel groups");
            IEnumerable<MuscleGroup> routines = await _getExercisesService.GetAllMuscleGroups();
            _muscleGroups.Clear();
            _muscleGroups.AddRange(routines);
            MuscleGroupsLoaded?.Invoke();
        }

    }
}
