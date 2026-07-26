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
        private readonly List<ExerciseV2> _exercises;
        public IEnumerable<ExerciseV2> Exercises => _exercises;

        private readonly List<MuscleGroupV2> _muscleGroups;
        public IEnumerable<MuscleGroupV2> MuscleGroups => _muscleGroups;

        private readonly List<Equipment> _equipments;
        public IEnumerable<Equipment> Equipments => _equipments;
        public event Action? ExercisesLoaded;
        public event Action? MuscleGroupsLoaded;
        public event Action? SelectedMuscleChanged;


        public event Action<double>? MuscleGroupDownloaded;
        public event Action<double>? GotExercises;

        private MuscleGroupV2? _selectedMuscle;
        public MuscleGroupV2? SelectedMuscle
        {
            get { return _selectedMuscle; }
            set { _selectedMuscle = value; SelectedMuscleChanged?.Invoke(); }
        }
        public ExercisesDataStore(IGetExercisesService getExercisesService, ILogger<ExercisesDataStore> logger, GetExercisesService getExercisesApiService)
        {
            _getExercisesService = getExercisesService;
            _logger = logger;
            _exercises = new List<ExerciseV2>();
            _muscleGroups = new List<MuscleGroupV2>();
            _equipments = new List<Equipment>();
            _getExercisesApiService = getExercisesApiService;
        }


        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all exercises");
            IEnumerable<ExerciseV2> routines = await _getExercisesService.GetAll();
            _exercises.Clear();
            _exercises.AddRange(routines);
            ExercisesLoaded?.Invoke();
        }

        public async Task GetAllMuscleGroups()
        {
            _logger.LogInformation(LogFlag + "get all muscel groups");
            IEnumerable<MuscleGroupV2> routines = await _getExercisesService.GetAllMuscleGroups();
            IEnumerable<Equipment> equipment = await _getExercisesService.GetAllEquipments();
            _muscleGroups.Clear();
            _muscleGroups.AddRange(routines);
            _equipments.Clear();
            _equipments.AddRange(equipment);
            MuscleGroupsLoaded?.Invoke();
        }
        public async Task GetExcersisesWithMuscleGroups()
        {
            int count = 0;
            ApiResponse<List<ExerciseDtoModel>> exerciseDtoResponse = await _getExercisesApiService.FetchExercises();
            MuscleGroupDownloaded?.Invoke(exerciseDtoResponse.Data!.Count());
            string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Uniceps");
            string imagesFolder = Path.Combine(appDataFolder, "ImagesV2");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);
            foreach (var exerciseDto in exerciseDtoResponse.Data!)
            {
                try
                {
                    string muscleFolder = Path.Combine(imagesFolder, exerciseDto.MuscleHeadCode.ToString());
                    if (!Directory.Exists(muscleFolder))
                        Directory.CreateDirectory(muscleFolder);

                    Directory.CreateDirectory(muscleFolder);
                    string originalExtension = Path.GetExtension(exerciseDto.ImageUrl)!;
                    string localPath = Path.Combine(muscleFolder, $"{exerciseDto.ExerciseId}.png");
                    ExerciseV2 exercises = new()
                    {
                        ImagePath = localPath,
                        MuscleGroupCode = exerciseDto.MuscleGroupCode,
                        Name = exerciseDto.Name,
                        ExerciseId = exerciseDto.ExerciseId,
                        MuscleHeadCode = exerciseDto.MuscleHeadCode,
                        MuscleAux1 = exerciseDto.MuscleAux1,
                        MuscleAux2 = exerciseDto.MuscleAux2,
                        MuscleAux3 = exerciseDto.MuscleAux3,
                        Description = exerciseDto.Implementation ?? "N/A",
                        EquipmentCode = exerciseDto.EquipmentCode,
                        IsActive = true,
                        IsLegacy = false,
                        Mechanism = GetExerciseMechanisim(exerciseDto.Mechanism ?? ""),
                        LastUpdated = exerciseDto.LastUpdated,
                        Version = exerciseDto.Version,
                    };
                    int oldVersion = await _getExercisesService.GetExerciseVersion(exercises.ExerciseId);

                    await _getExercisesService.GetOrCreate(exercises);

                    if (oldVersion < exerciseDto.Version || !File.Exists(localPath))
                    {
                        if (File.Exists(localPath)) File.Delete(localPath);
                        await _getExercisesApiService.DownloadImage(exerciseDto.ExerciseId!, localPath);
                    }
                    GotExercises?.Invoke(++count);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message} | {ex.InnerException?.Message} | {exerciseDto.ExerciseId}");
                }
            }
        }
        private ExerciseMechanism GetExerciseMechanisim(string uniBi)
        {
            foreach (var item in Enum.GetValues(typeof(ExerciseMechanism)))
            {
                if (uniBi.Trim().ToLower().Equals(item.ToString()?.Trim().ToLower()))
                {
                    return (ExerciseMechanism)item;
                }
            }
            return ExerciseMechanism.Bi;
        }
        public async Task GetAndVerifyMuscleGroups()
        {
            ApiResponse<EssentialsReponse> apiMuscleGroups = await _getExercisesApiService.FetchEssentials();
            foreach (var musGroup in apiMuscleGroups.Data!.MuscleGroups)
            {
                MuscleGroupV2 muscleGroup = new()
                {
                    Name = musGroup.Name,
                    Code = musGroup.Code,
                };
                muscleGroup.Heads = musGroup.MuscleHeads.Select(x => new MuscleHead { Code = x.Code, MuscleGroupCode = x.MuscleGroupCode, Name = x.Name }).ToList();
                try
                {
                    await _getExercisesService.GetOrCreateMuscleGroup(muscleGroup);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message} | {ex.InnerException?.Message} | {muscleGroup.Code}");
                }
            }
            foreach (var equip in apiMuscleGroups.Data!.Equipments)
            {
                Equipment equipment = new()
                {
                    Name = equip.Name,
                    Code = equip.Code,
                };
                try
                {
                    await _getExercisesService.GetOrCreateEquipments(equipment);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message} | {ex.InnerException?.Message} | {equipment.Code}");
                }
            }
        }

    }
}
