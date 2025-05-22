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
    public class DayGroupDataStore : IDataStore<DayGroup>, IGetAllByIdDataStore<DayGroup>
    {
        private readonly IDataService<DayGroup> _dataService;
        private readonly IGetAllById<DayGroup> _getAllDataService;
        private readonly List<DayGroup> _dayGroups;
        public IEnumerable<DayGroup> DayGroups => _dayGroups;
        public DayGroupDataStore(IDataService<DayGroup> dataService, IGetAllById<DayGroup> getAllDataService)
        {
            _dataService = dataService;
            _dayGroups = new List<DayGroup>();
            _getAllDataService = getAllDataService;
        }

        public event Action<DayGroup>? Created;
        public event Action? Loaded;
        public event Action<DayGroup>? Updated;
        public event Action<int>? Deleted;
        public event Action<DayGroup?>? DayGroupChanged;
        private DayGroup? _selectedDayGroup;
        public DayGroup? SelectedDayGroup
        {
            get
            {
                return _selectedDayGroup;
            }
            set
            {
                _selectedDayGroup = value;
                DayGroupChanged?.Invoke(SelectedDayGroup);
            }
        }

        public async Task Add(DayGroup entity)
        {
            await _dataService.Create(entity);
            _dayGroups.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            await _dataService.Delete(entity_id);
            int currentIndex = _dayGroups.FindIndex(y => y.Id == entity_id);
            _dayGroups.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<DayGroup> dayGroups = await _dataService.GetAll();
            _dayGroups.Clear();
            _dayGroups.AddRange(dayGroups);
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(DayGroup entity)
        {
            await _dataService.Update(entity);
            int currentIndex = _dayGroups.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _dayGroups[currentIndex] = entity;
            }
            else
            {
                _dayGroups.Add(entity);
            }
            Updated?.Invoke(entity);
        }

        public async Task GetAllById(int id)
        {
            IEnumerable<DayGroup> dayGroups = await _getAllDataService.GetAllById(id);
            _dayGroups.Clear();
            _dayGroups.AddRange(dayGroups);
            Loaded?.Invoke();
        }
    }
}
