using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Exercises;
using Uniceps.API.Models;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Core.Services;

namespace Uniceps.Stores.RoutineStores
{
    public class ExercisesDataStore
    {
        string LogFlag = "[Routine] ";

        private readonly IGetExercisesService _getExercisesService;
        private readonly GetExercisesService _getExercisesApiService;
        private readonly ILogger<ExercisesDataStore> _logger;
        private readonly List<Exercises> _exercises;
        public IEnumerable<Exercises> Exercises => _exercises;

        private readonly List<MuscleGroup> _muscleGroups;
        public IEnumerable<MuscleGroup> MuscleGroups => _muscleGroups;
        public event Action? ExercisesLoaded;
        public event Action? MuscleGroupsLoaded;
        public event Action? SelectedMuscleChanged;


        public event Action<double>? MuscleGroupDownloaded;
        public event Action<double>? GotExercises;

        private MuscleGroup? _selectedMuscle;
        public MuscleGroup? SelectedMuscle
        {
            get { return _selectedMuscle; }
            set { _selectedMuscle = value; SelectedMuscleChanged?.Invoke(); }
        }
        public ExercisesDataStore(IGetExercisesService getExercisesService, ILogger<ExercisesDataStore> logger, GetExercisesService getExercisesApiService)
        {
            _getExercisesService = getExercisesService;
            _logger = logger;
            _exercises = new List<Exercises>();
            _muscleGroups = new List<MuscleGroup>();
            _getExercisesApiService = getExercisesApiService;
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
        public async Task GetExcersisesWithMuscleGroups()
        {
            List<MuscleGroup> muscles = await GetAndVerifyMuscleGroups();
            foreach (var mg in muscles)
            {
                int count = 0;
                ApiResponse<List<ExerciseDtoModel>> exerciseDtoResponse= await _getExercisesApiService.FetchExercises(mg.PublicId);
                MuscleGroupDownloaded?.Invoke(exerciseDtoResponse.Data!.Count());
                string appFolder = AppDomain.CurrentDomain.BaseDirectory + "Images\\";
                Directory.CreateDirectory(appFolder); // Ensure the folder exists
                foreach (var exerciseDto in exerciseDtoResponse.Data!)
                {
                    string folder_path = Path.Combine(appFolder, exerciseDto.muscleGroupId.ToString());
                    Directory.CreateDirectory(folder_path);
                    string originalExtension = Path.GetExtension(exerciseDto.imageUrl)!;
                    string localPath = Path.Combine(folder_path, $"exercise_{exerciseDto.name}{originalExtension}");
                    Exercises exercises = new()
                    {
                        ImagePath = localPath,
                        MuscleGroupId = exerciseDto.muscleGroupId,
                        Name = exerciseDto.name,
                        Tid = exerciseDto.id,
                        MuscelAr = mg.Name,
                        MuscelEng = mg.EngName,
                        Version = 0,
                         ImageUrl= exerciseDto.imageUrl
                    };

                    await _getExercisesService.GetOrCreate(exercises);
                    if (!File.Exists(localPath))
                    {
                        await _getExercisesApiService.DownloadImage(exerciseDto.imageUrl!, localPath);
                    }

                    GotExercises?.Invoke(++count);
                }

            }
        }
        public async Task<List<MuscleGroup>> GetAndVerifyMuscleGroups()
        {
            List<MuscleGroup> muscleGroups = new List<MuscleGroup>();
            ApiResponse<List<MuscleGroupDto>> apiMuscleGroups = await _getExercisesApiService.FetchMuscleGroup();
            foreach(var musGroup in apiMuscleGroups.Data!)
            {
                MuscleGroup muscleGroup = new()
                {
                    Name = musGroup.Name,
                    EngName = musGroup.EngName,
                    PublicId = musGroup.Id
                };
                await _getExercisesService.GetOrCreateMuscleGroup(muscleGroup);
                muscleGroups.Add(muscleGroup);
            }
            return muscleGroups;
        }
    }
}
