using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;

namespace Unicepse.Stores
{
    public class RoutineTemplatesDataStore
    {
        string LogFlag = "[Routine] ";
        private readonly ILogger<RoutineDataStore> _logger;
        private readonly IGetRoutineTemplatesService _getRoutineTemplatesService;
        private readonly List<PlayerRoutine> _tempRoutines;
        public IEnumerable<PlayerRoutine> TempRoutines => _tempRoutines;
        public event Action? TempLoaded;
        public RoutineTemplatesDataStore(ILogger<RoutineDataStore> logger, IGetRoutineTemplatesService getRoutineTemplatesService)
        {
            _logger = logger;
            _getRoutineTemplatesService = getRoutineTemplatesService;
            _tempRoutines = new List<PlayerRoutine>();
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all templates");
            IEnumerable<PlayerRoutine> routines = await _getRoutineTemplatesService.GetAll();
            _tempRoutines.Clear();
            _tempRoutines.AddRange(routines);
            TempLoaded?.Invoke();
        }
    }
}
