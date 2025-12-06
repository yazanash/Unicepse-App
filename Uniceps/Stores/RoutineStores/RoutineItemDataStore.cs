using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Stores.RoutineStores
{
    public class RoutineItemDataStore : IDataStore<RoutineItemModel>, IGetAllByIdDataStore<RoutineItemModel>
    {
        private readonly IDataService<RoutineItemModel> _dataService;
        private readonly IGetAllById<RoutineItemModel> _getAllDataService;
        private readonly IUpdateRangeDataService<RoutineItemModel> _updateRangeDataService;
        private readonly List<RoutineItemModel> _routineItemModels;
        public IEnumerable<RoutineItemModel> RoutineItems => _routineItemModels;
        public RoutineItemDataStore(IDataService<RoutineItemModel> dataService, IGetAllById<RoutineItemModel> getAllDataService, IUpdateRangeDataService<RoutineItemModel> updateRangeDataService)
        {
            _dataService = dataService;
            _routineItemModels = new List<RoutineItemModel>();
            _getAllDataService = getAllDataService;
            _updateRangeDataService = updateRangeDataService;
        }

        public event Action<RoutineItemModel>? Created;
        public event Action? Loaded;
        public event Action<RoutineItemModel>? Updated;
        public event Action<int>? Deleted;
        public event Action<RoutineItemModel?>? RoutineItemChanged;
        private RoutineItemModel? _selectedRoutineItem;
        public RoutineItemModel? SelectedRoutineItem
        {
            get
            {
                return _selectedRoutineItem;
            }
            set
            {
                _selectedRoutineItem = value;
                RoutineItemChanged?.Invoke(SelectedRoutineItem);
            }
        }
        public async Task Add(RoutineItemModel entity)
        {
            await _dataService.Create(entity);
            _routineItemModels.Add(entity);
            Created?.Invoke(entity);
        }
        public async Task Delete(int entity_id)
        {
            await _dataService.Delete(entity_id);
            int currentIndex = _routineItemModels.FindIndex(y => y.Id == entity_id);
            _routineItemModels.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<RoutineItemModel> routineItems = await _dataService.GetAll();
            _routineItemModels.Clear();
            _routineItemModels.AddRange(routineItems);
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(RoutineItemModel entity)
        {
            await _dataService.Update(entity);
            int currentIndex = _routineItemModels.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _routineItemModels[currentIndex] = entity;
            }
            else
            {
                _routineItemModels.Add(entity);
            }
            Updated?.Invoke(entity);
        }

        public async Task GetAllById(int id)
        {
            IEnumerable<RoutineItemModel> routineItems = await _getAllDataService.GetAllById(id);
            _routineItemModels.Clear();
            _routineItemModels.AddRange(routineItems);
            Loaded?.Invoke();
        }
        public async Task UpdateRange(List<RoutineItemModel> entities)
        {
            await _updateRangeDataService.UpdateRange(entities);
            foreach (RoutineItemModel entity in entities)
            {
                int currentIndex = _routineItemModels.FindIndex(y => y.Id == entity.Id);

                if (currentIndex != -1)
                {
                    _routineItemModels[currentIndex] = entity;
                }
                else
                {
                    _routineItemModels.Add(entity);
                }
            }

        }
    }
}
