using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;

namespace Uniceps.Stores
{
    public class OtherRevenuesDataStore : IDataStore<OtherRevenue>
    {
        private readonly IDataService<OtherRevenue> _dataService;
        private readonly List<OtherRevenue> _otherRevenues;
        public IEnumerable<OtherRevenue> Revenues => _otherRevenues;
        public OtherRevenuesDataStore(IDataService<OtherRevenue> dataService)
        {
            _dataService = dataService;
            _otherRevenues=new List<OtherRevenue>();
        }

        public event Action<OtherRevenue>? Created;
        public event Action? Loaded;
        public event Action<OtherRevenue>? Updated;
        public event Action<int>? Deleted;

        public async Task Add(OtherRevenue entity)
        {
            await _dataService.Create(entity);
            _otherRevenues.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _dataService.Delete(entity_id);
            int currentIndex = _otherRevenues.FindIndex(y => y.Id == entity_id);
            _otherRevenues.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<OtherRevenue> otherRevenue = await _dataService.GetAll();
            _otherRevenues.Clear();
            _otherRevenues.AddRange(otherRevenue);
            Loaded?.Invoke();
        }

        public async Task Initialize()
        {
            IEnumerable<OtherRevenue> otherRevenue = await _dataService.GetAll();
            _otherRevenues.Clear();
            _otherRevenues.AddRange(otherRevenue);
        }

        public async Task Update(OtherRevenue entity)
        {
            await _dataService.Update(entity);
            int currentIndex = _otherRevenues.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _otherRevenues[currentIndex] = entity;
            }
            else
            {
                _otherRevenues.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
