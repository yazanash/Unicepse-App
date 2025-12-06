using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.FileSystem;
using Uniceps.FileSystem.Helpers;
using Uniceps.FileSystem.Models;
using Uniceps.Helpers.Mappers;
using Uniceps.Models.RoutineExportModels;
using Uniceps.Stores;
using Uniceps.Stores.ApiDataStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PrintRoutineViewModel;

namespace Uniceps.Stores.RoutineStores
{
    public class RoutineTempDataStore : IDataStore<RoutineModel>
    {
        private readonly IDataService<RoutineModel> _dataService;
        private readonly List<RoutineModel> _routineModels;
        private readonly AccountStore _accountStore;
        public IEnumerable<RoutineModel> Routines => _routineModels;
        public RoutineTempDataStore(IDataService<RoutineModel> dataService, AccountStore accountStore)
        {
            _dataService = dataService;
            _routineModels = new List<RoutineModel>();
            _accountStore = accountStore;
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

        public async Task ExportRoutine(int entity_id, FileFormatType fileFormatType, string filePath)
        {
            RoutineModel routineModel = await _dataService.Get(entity_id);
            switch (fileFormatType)
            {
                case FileFormatType.UniFile:
                    RoutineExportDto routineExportDto = RoutineMapper.Map(routineModel);
                    var uniFile = new UniFile
                    {
                        Meta = new MetaData
                        {
                            Source = "MyApp",
                            SchemaVersion = 1,
                            CreatedAt = DateTime.UtcNow,
                            FileType = GetEnumString(FileTypes.Routine)
                        },
                        Data = routineExportDto
                    };
                    string uniFilePath = UniFileHelper.EnsureUniExtension(filePath);
                    FileManager.Write(uniFile, uniFilePath);
                    break;
                case FileFormatType.PDF:
                    PlayerRoutinePrintViewModel vm = RoutineMapper.MapToPdf(routineModel, routineModel.Name ?? "");
                    if (_accountStore.SystemProfile != null)
                    {
                        vm.GymName = _accountStore.SystemProfile.DisplayName;
                        if (!string.IsNullOrEmpty(_accountStore.SystemProfile.LocalProfileImagePath))
                            vm.GymLogo=_accountStore.SystemProfile.LocalProfileImagePath;
                    }
                    var doc = BuildRoutineDocument.BuildDocument(vm);
                    var pd = new PrintDialog();
                    if (pd.ShowDialog() == true)
                    {
                        pd.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
                        pd.PrintDocument(doc.DocumentPaginator, "Routine PDF");
                    }

                    break;
            }
        }
        public string GetEnumString(FileTypes value)
        {

            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();

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
