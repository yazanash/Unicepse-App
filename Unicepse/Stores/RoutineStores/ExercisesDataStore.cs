using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;

namespace Unicepse.Stores.RoutineStores
{
    public class ExercisesDataStore
    {
        string LogFlag = "[Routine] ";

        private readonly IGetExercisesService _getExercisesService;
        private readonly ILogger<RoutineDataStore> _logger;
        private readonly List<Exercises> _exercises;
        public IEnumerable<Exercises> Exercises => _exercises;
        public event Action? ExercisesLoaded;
        public ExercisesDataStore(IGetExercisesService getExercisesService, ILogger<RoutineDataStore> logger)
        {
            _getExercisesService = getExercisesService;
            _logger = logger;
            _exercises = new List<Exercises>();
        }


        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all exercises");
            IEnumerable<Exercises> routines = await _getExercisesService.GetAll();
            _exercises.Clear();
            _exercises.AddRange(routines);
            ExercisesLoaded?.Invoke();
        }

    }
}
