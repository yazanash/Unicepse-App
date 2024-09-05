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

namespace Unicepse.Stores
{
    public class RoutineDataStore : IDataStore<PlayerRoutine>
    {
        public event Action<PlayerRoutine>? Created;
        public event Action? Loaded;
        public event Action? ExercisesLoaded;
        public event Action<PlayerRoutine>? Updated;
        public event Action<int>? Deleted;
        private readonly PlayerRoutineDataService _playerRoutineDataService;
        private readonly RoutineApiDataService _routineApiDataService;

        private readonly List<PlayerRoutine> _playerRoutines;
        private readonly List<PlayerRoutine> _tempRoutines;
        private readonly List<Exercises> _exercises;

        private readonly List<RoutineItems> _routineItems;
        private readonly List<DayGroupListItemViewModel> _daysItems;
        public event Action<RoutineItems>? RoutineItemCreated;
        public event Action<RoutineItems>? RoutineItemDeleted;
        public event Action<DayGroupListItemViewModel>? daysItemCreated;
        public event Action<DayGroupListItemViewModel>? daysItemDeleted;
        public IEnumerable<PlayerRoutine> Routines => _playerRoutines;
        public IEnumerable<PlayerRoutine> TempRoutines => _tempRoutines;
        public IEnumerable<Exercises> Exercises => _exercises;

        public IEnumerable<RoutineItems> RoutineItems => _routineItems;
        public IEnumerable<DayGroupListItemViewModel> DaysItems => _daysItems;
        public RoutineDataStore(PlayerRoutineDataService playerRoutineDataService, RoutineApiDataService routineApiDataService)
        {
            _playerRoutineDataService = playerRoutineDataService;
            _playerRoutines = new List<PlayerRoutine>();
            _exercises = new List<Exercises>();
            _routineItems = new List<RoutineItems>();
            _daysItems = new List<DayGroupListItemViewModel>();
            _tempRoutines = new List<PlayerRoutine>();
            _routineApiDataService = routineApiDataService;
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
                _routineItems.Clear();
                StateChanged?.Invoke(SelectedRoutine);
            }
        }
        public event Action<PlayerRoutine?>? StateChanged;

        public async Task Add(PlayerRoutine entity)
        {
            entity.DataStatus = DataStatus.ToCreate;
            await _playerRoutineDataService.Create(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                bool status = await _routineApiDataService.Create(entity);
                if (status)
                {
                    entity.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.Update(entity);
                }

            }
            _playerRoutines.Add(entity);
            Created?.Invoke(entity);
            _routineItems.Clear();
        }
        public void AddRoutineItem(RoutineItems entity)
        {
            //await _playerRoutineDataService.Create(entity);
            _routineItems.Add(entity);
            RoutineItemCreated?.Invoke(entity);
        }
        public void DeleteRoutineItem(RoutineItems entity)
        {
            //await _playerRoutineDataService.Create(entity);
            _routineItems.Remove(entity);
            RoutineItemDeleted?.Invoke(entity);
        }
        public void AddDaysItem(DayGroupListItemViewModel entity)
        {
            //await _playerRoutineDataService.Create(entity);
            _daysItems.Add(entity);
            daysItemCreated?.Invoke(entity);
        }
        public void DeleteDaysItem(DayGroupListItemViewModel entity)
        {
            //await _playerRoutineDataService.Create(entity);
            _daysItems.Remove(entity);
            daysItemDeleted?.Invoke(entity);
        }
        public async Task Delete(int entity_id)
        {
            bool deleted = await _playerRoutineDataService.Delete(entity_id);
            int currentIndex = _playerRoutines.FindIndex(y => y.Id == entity_id);
            _playerRoutines.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {

            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAll();
            _playerRoutines.Clear();
            _playerRoutines.AddRange(routines);
            Loaded?.Invoke();
        }

        public async Task GetAllExercises()
        {

            IEnumerable<Exercises> routines = await _playerRoutineDataService.GetAllExercises();
            _exercises.Clear();
            _exercises.AddRange(routines);
            ExercisesLoaded?.Invoke();
        }

        public async Task GetAll(Player player)
        {

            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAll(player);
            _playerRoutines.Clear();
            _playerRoutines.AddRange(routines);
            Loaded?.Invoke();
        }
        public async Task GetAllTemp()
        {

            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetAllTemp();
            _tempRoutines.Clear();
            _tempRoutines.AddRange(routines);
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(PlayerRoutine entity)
        {
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            await _playerRoutineDataService.Update(entity);

            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                bool status = await _routineApiDataService.Update(entity);
                if (status)
                {
                    entity.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.Update(entity);
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
                bool status = await _routineApiDataService.Create(routine);
                if (status)
                {
                    routine.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.Update(routine);
                }


            }
        }

        public async Task SyncRoutineToUpdate()
        {
            IEnumerable<PlayerRoutine> routines = await _playerRoutineDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (PlayerRoutine routine in routines)
            {
                bool status = await _routineApiDataService.Update(routine);
                if (status)
                {
                    routine.DataStatus = DataStatus.Synced;
                    await _playerRoutineDataService.Update(routine);
                }


            }
        }
    }
}
