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

namespace Unicepse.Stores
{
    public class RoutineDataStore : IDataStore<PlayerRoutine>
    {
        public event Action<PlayerRoutine>? Created;
        public event Action? Loaded;
        public event Action? TempLoaded;
        public event Action? ExercisesLoaded;
        public event Action<PlayerRoutine>? Updated;
        public event Action<int>? Deleted;
        public event Action<RoutineItems>? OrdersApplied;
        private readonly PlayerRoutineDataService _playerRoutineDataService;
        private readonly RoutineApiDataService _routineApiDataService;

        string LogFlag = "[Routine] ";
        private readonly ILogger<RoutineDataStore> _logger;

        public event Action? Reorderd;
        private readonly List<PlayerRoutine> _playerRoutines;
        private readonly List<PlayerRoutine> _tempRoutines;
        private readonly List<Exercises> _exercises;

        private readonly List<RoutineItems> _routineItems;

        private readonly List<DayGroupListItemViewModel> _daysItems;
        public event Action<RoutineItems>? RoutineItemCreated;
        public event Action<RoutineItems>? RoutineItemDeleted;
        public event Action<DayGroupListItemViewModel>? daysItemCreated;
        public event Action<DayGroupListItemViewModel>? daysItemDeleted;

        public event Action<List<Exercises>>? RoutineItemsCleared;

        public IEnumerable<PlayerRoutine> Routines => _playerRoutines;
        public IEnumerable<PlayerRoutine> TempRoutines => _tempRoutines;
        public IEnumerable<Exercises> Exercises => _exercises;

        public IEnumerable<RoutineItems> RoutineItems => _routineItems;
        public IEnumerable<DayGroupListItemViewModel> DaysItems => _daysItems;
        public RoutineDataStore(PlayerRoutineDataService playerRoutineDataService, RoutineApiDataService routineApiDataService, ILogger<RoutineDataStore> logger)
        {
            _playerRoutineDataService = playerRoutineDataService;
            _playerRoutines = new List<PlayerRoutine>();
            _exercises = new List<Exercises>();
            _routineItems = new List<RoutineItems>();
            _daysItems = new List<DayGroupListItemViewModel>();
            _tempRoutines = new List<PlayerRoutine>();
            _routineApiDataService = routineApiDataService;
            _logger = logger;
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
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add routine to api");
                    int status = await _routineApiDataService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _playerRoutineDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "routine synced failed with error {0}", ex.Message);
                }
            }
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
            await _playerRoutineDataService.DeleteRoutineItems(id);
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

        public async Task GetAllExercises()
        {
            _logger.LogInformation(LogFlag + "get all exercises");
            IEnumerable<Exercises> routines = await _playerRoutineDataService.GetAllExercises();
            _exercises.Clear();
            _exercises.AddRange(routines);
            ExercisesLoaded?.Invoke();
        }

        public async Task GetAll(Player player)
        {
            _logger.LogInformation(LogFlag + "get all player routines");
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAll(player);
            _playerRoutines.Clear();
            _playerRoutines.AddRange(routines);
            Loaded?.Invoke();
        }
        public async Task GetAllTemp()
        {
            _logger.LogInformation(LogFlag + "get all templates");
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAllTemp();
            _tempRoutines.Clear();
            _tempRoutines.AddRange(routines);
            TempLoaded?.Invoke();
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

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "update routine to api");
                    int status = await _routineApiDataService.Update(entity);
                    if (status == 200)
                    {
                        _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                        entity.DataStatus = DataStatus.Synced;
                        await _playerRoutineDataService.UpdateDataStatus(entity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "routine synced failed with error {0}", ex.Message);
                }

            }


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

        public async Task SyncRoutineToCreate()
        {
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetByDataStatus(DataStatus.ToCreate);
            foreach (PlayerRoutine routine in routines)
            {
                _logger.LogInformation(LogFlag + "create routine to api");
                int status = await _routineApiDataService.Create(routine);
                if (status == 201 || status == 409)
                {
                    _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                    routine.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.UpdateDataStatus(routine);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "routine synced failed with code {0}", status.ToString());
                }

            }
        }

        public async Task SyncRoutineToUpdate()
        {
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (PlayerRoutine routine in routines)
            {
                _logger.LogInformation(LogFlag + "update routine to api");
                int status = await _routineApiDataService.Update(routine);
                if (status == 200)
                {
                    _logger.LogInformation(LogFlag + "routine synced successfully with code {0}", status.ToString());
                    routine.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.UpdateDataStatus(routine);
                }
                else
                {
                    _logger.LogWarning(LogFlag + "routine synced failed with code {0}", status.ToString());
                }

            }
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
