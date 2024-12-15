using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Entityframework.Services;
using Unicepse.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.Stores
{
    public class RoutineDataStore : IDataStore<PlayerRoutine>
    {
        public event Action<PlayerRoutine>? Created;
        public event Action? Loaded;
        //public event Action? ExercisesLoaded;
        public event Action<PlayerRoutine>? Updated;
        public event Action<int>? Deleted;
        public event Action<RoutineItems>? OrdersApplied;
        private readonly IDataService<PlayerRoutine> _playerRoutineDataService;
        private readonly IGetPlayerTransactionService<PlayerRoutine> _getPlayerTransactionService;
        private readonly IRoutineItemsDataService _routineItemsDataService;
        private readonly IApiDataStore<PlayerRoutine> _apiDataStore;


        string LogFlag = "[Routine] ";
        private readonly ILogger<RoutineDataStore> _logger;

        public event Action? Reorderd;
        private readonly List<PlayerRoutine> _playerRoutines;

        private readonly List<RoutineItems> _routineItems;

        private readonly List<DayGroupListItemViewModel> _daysItems;
        public event Action<RoutineItems>? RoutineItemCreated;
        public event Action<RoutineItems>? RoutineItemDeleted;
        public event Action<DayGroupListItemViewModel>? daysItemCreated;
        public event Action<DayGroupListItemViewModel>? daysItemDeleted;

        public event Action<List<Exercises>>? RoutineItemsCleared;

        public IEnumerable<PlayerRoutine> Routines => _playerRoutines;

        public IEnumerable<RoutineItems> RoutineItems => _routineItems;
        public IEnumerable<DayGroupListItemViewModel> DaysItems => _daysItems;
        public RoutineDataStore(IDataService<PlayerRoutine> playerRoutineDataService, ILogger<RoutineDataStore> logger, IGetPlayerTransactionService<PlayerRoutine> getPlayerTransactionService, IRoutineItemsDataService routineItemsDataService, IApiDataStore<PlayerRoutine> apiDataStore)
        {
            _playerRoutineDataService = playerRoutineDataService;
            _playerRoutines = new List<PlayerRoutine>();
            _routineItems = new List<RoutineItems>();
            _daysItems = new List<DayGroupListItemViewModel>();
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
            _routineItemsDataService = routineItemsDataService;
            _apiDataStore = apiDataStore;
        }

        internal void ReloadDaysItems()
        {
            _logger.LogInformation(LogFlag + "reload days item");
            List<DayGroupListItemViewModel> daysListItemViewModels = _daysItems.ToList();
            _daysItems.Clear();
            foreach (var item in daysListItemViewModels)
            {
                AddDaysItem(item);
            }
        }
        internal void ApplyToAll(RoutineItems routineItems)
        {
            OrdersApplied?.Invoke(routineItems);
        }
        private MuscleGroup? _selectedMuscleGroup;
        public MuscleGroup? SelectedMuscle
        {
            get
            {
                return _selectedMuscleGroup;
            }
            set
            {
                _selectedMuscleGroup = value;
                _logger.LogInformation(LogFlag + "selected muscle changed");
                MuscleChanged?.Invoke(SelectedMuscle);
                
            }
        }
        public event Action<MuscleGroup?>? MuscleChanged;
        private PlayerRoutine? _selectedRoutine;
        public PlayerRoutine? SelectedRoutine
        {
            get
            {
                return _selectedRoutine;
            }
            set
            {
                _selectedRoutine = value;
                _logger.LogInformation(LogFlag + "selected routine changed");
                _routineItems.Clear();
                StateChanged?.Invoke(SelectedRoutine);
            }
        }
        public event Action<PlayerRoutine?>? StateChanged;

        public async Task Add(PlayerRoutine entity)
        {
            _logger.LogInformation(LogFlag + "add routine");
            entity.DataStatus = DataStatus.ToCreate;
            await _playerRoutineDataService.Create(entity);
           await _apiDataStore.Create(entity);
            _playerRoutines.Add(entity);
            Created?.Invoke(entity);
            _routineItems.Clear();
        }
        public void AddRoutineItem(RoutineItems entity)
        {
            _logger.LogInformation(LogFlag + "add routine item");
            _routineItems.Add(entity);
            RoutineItemCreated?.Invoke(entity);
            Reorderd?.Invoke();
        }

        public void Reorder()
        {
            Reorderd?.Invoke();
        }

        public void DeleteRoutineItem(RoutineItems entity)
        {
            _logger.LogInformation(LogFlag + "delete routine item");
            _routineItems.Remove(entity);
            RoutineItemDeleted?.Invoke(entity);
        }
        public async Task DeleteRoutineItems(int id)
        {
            _logger.LogInformation(LogFlag + "add routine items");
            await _routineItemsDataService.DeleteRoutineItems(id);
        }
        public void AddDaysItem(DayGroupListItemViewModel entity)
        {
            _logger.LogInformation(LogFlag + "add days item");
            _daysItems.Add(entity);
            daysItemCreated?.Invoke(entity);
        }
        public void DeleteDaysItem(DayGroupListItemViewModel entity)
        {
            _logger.LogInformation(LogFlag + "delete days item");
            _daysItems.Remove(entity);
            daysItemDeleted?.Invoke(entity);
        }
        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete routine");
            bool deleted = await _playerRoutineDataService.Delete(entity_id);
            int currentIndex = _playerRoutines.FindIndex(y => y.Id == entity_id);

            _playerRoutines.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all routines");
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAll();
            _playerRoutines.Clear();
            _playerRoutines.AddRange(routines);
            Loaded?.Invoke();
        }

        public async Task GetAll(Player player)
        {
            _logger.LogInformation(LogFlag + "get all player routines");
            IEnumerable<PlayerRoutine> routines = await _getPlayerTransactionService.GetAll(player);
            _playerRoutines.Clear();
            _playerRoutines.AddRange(routines);
            Loaded?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(PlayerRoutine entity)
        {
            _logger.LogInformation(LogFlag + "update routine");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            await _playerRoutineDataService.Update(entity);
            await _apiDataStore.Update(entity);
            int currentIndex = _playerRoutines.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _playerRoutines[currentIndex] = entity;
            }
            else
            {
                _playerRoutines.Add(entity);
            }
            _routineItems.Clear();
            Updated?.Invoke(entity);
        }

        internal void ClearRoutineItems()
        {
            _logger.LogInformation(LogFlag + "clear routine items");
            List<Exercises> exercises = new List<Exercises>();
            foreach (var item in _routineItems)
            {
                if (!exercises.Where(x => x.Id == item.Exercises!.Id).Any())
                    exercises.Add(item.Exercises!);
            }
            _routineItems.Clear();
            RoutineItemsCleared?.Invoke(exercises);
        }

        internal void ClearDaysItems()
        {
            _logger.LogInformation(LogFlag + "clear days items");
            _daysItems.Clear();
        }
    }
}
