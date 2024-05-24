using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGym.Entityframework.Services;
using PlatinumGymPro.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class RoutineDataStore : IDataStore<PlayerRoutine>
    {
        public event Action<PlayerRoutine>? Created;
        public event Action? Loaded;
        public event Action? ExercisesLoaded;
        public event Action<PlayerRoutine>? Updated;
        public event Action<int>? Deleted;
        private readonly PlayerRoutineDataService _playerRoutineDataService;
        private readonly List<PlayerRoutine> _playerRoutines;
        private readonly List<Exercises> _exercises;

        private readonly List<RoutineItems> _routineItems;
        public event Action<RoutineItems>? RoutineItemCreated;
        public event Action<RoutineItems>? RoutineItemDeleted;

        public IEnumerable<PlayerRoutine> Routines => _playerRoutines;
        public IEnumerable<Exercises> Exercises => _exercises;

        public IEnumerable<RoutineItems> RoutineItems => _routineItems;
        public RoutineDataStore(PlayerRoutineDataService playerRoutineDataService)
        {
            _playerRoutineDataService = playerRoutineDataService;
            _playerRoutines = new List<PlayerRoutine>();
            _exercises = new List<Exercises>();
            _routineItems = new List<RoutineItems>();
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
            await _playerRoutineDataService.Create(entity);
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
            //await _playerRoutineDataService.DeleteRoutineItems(entity.Id);
            await _playerRoutineDataService.Update(entity);
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
    }
}
