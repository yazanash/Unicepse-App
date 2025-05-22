using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;
using Uniceps.Stores;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Stores.ApiDataStores;

namespace Uniceps.Stores.RoutineStores
{
    public class RoutineTempDataStore : IDataStore<RoutineModel>
    {
        private readonly IDataService<RoutineModel> _dataService;
        private readonly List<RoutineModel> _routineModels;
        public IEnumerable<RoutineModel> Routines => _routineModels;
        public RoutineTempDataStore(IDataService<RoutineModel> dataService)
        {
            _dataService = dataService;
            _routineModels = new List<RoutineModel>();
        }

        public event Action<RoutineModel>? Created;
        public event Action? Loaded;
        public event Action<RoutineModel>? Updated;
        public event Action<int>? Deleted;
        public event Action<RoutineModel?>? RoutineChanged;

        private RoutineModel? _selectedRoutine;
        public RoutineModel? SelectedRoutine
        {
            get
            {
                return _selectedRoutine;
            }
            set
            {
                _selectedRoutine = value;
                RoutineChanged?.Invoke(SelectedRoutine);
            }
        }
        public async Task Add(RoutineModel entity)
        {
            await _dataService.Create(entity);
            _routineModels.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            await _dataService.Delete(entity_id);
            int currentIndex = _routineModels.FindIndex(y => y.Id == entity_id);
            _routineModels.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<RoutineModel> routines = await _dataService.GetAll();
            _routineModels.Clear();
            _routineModels.AddRange(routines);
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(RoutineModel entity)
        {

            await _dataService.Update(entity);
            int currentIndex = _routineModels.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _routineModels[currentIndex] = entity;
            }
            else
            {
                _routineModels.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
