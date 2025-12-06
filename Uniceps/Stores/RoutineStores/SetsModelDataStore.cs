using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Services;

namespace Uniceps.Stores.RoutineStores
{
    public class SetsModelDataStore : IDataStore<SetModel>, IGetAllByIdDataStore<SetModel>
    {
        private readonly IDataService<SetModel> _dataService;
        private readonly IApplySetsToAll _applySetsToAll;
        private readonly IGetAllById<SetModel> _getAllDataService;
        private readonly IUpdateRangeDataService<SetModel> _updateRangeDataService;
        private readonly List<SetModel> _setModels;

        public SetsModelDataStore(IDataService<SetModel> dataService, IGetAllById<SetModel> getAllDataService, IUpdateRangeDataService<SetModel> updateRangeDataService, IApplySetsToAll applySetsToAll)
        {
            _dataService = dataService;
            _getAllDataService = getAllDataService;
            _setModels = new List<SetModel>();
            _updateRangeDataService = updateRangeDataService;
            _applySetsToAll = applySetsToAll;
        }

        public IEnumerable<SetModel> SetModels => _setModels;
        public event Action<SetModel>? Created;
        public event Action? Loaded;
        public event Action<SetModel>? Updated;
        public event Action<int>? Deleted;
        public event Action<int,int>? DeletedSet;
        public event Action<List<SetModel>,int>? AppliedToAll;

        public async Task Add(SetModel entity)
        {
            await _dataService.Create(entity);
            _setModels.Add(entity);
            Created?.Invoke(entity);
        }
       

        public async Task Delete(int entity_id)
        {
            int ItemId = _setModels.FirstOrDefault(y => y.Id == entity_id)?.RoutineItemId ?? 0;
            await _dataService.Delete(entity_id);
            int currentIndex = _setModels.FindIndex(y => y.Id == entity_id);
            _setModels.RemoveAt(currentIndex);
            DeletedSet?.Invoke(entity_id,ItemId);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<SetModel> setModels = await _dataService.GetAll();
            _setModels.Clear();
            _setModels.AddRange(setModels);
            Loaded?.Invoke();
        }

        public async Task GetAllById(int id)
        {
            IEnumerable<SetModel> setModels = await _getAllDataService.GetAllById(id);
            _setModels.Clear();
            _setModels.AddRange(setModels);
            Loaded?.Invoke();
        }
        public async Task ApplySetsToAll(List<SetModel> entities,int id)
        {
            List<SetModel> setModels = await _applySetsToAll.ApplySetsToEntity(entities, id);
            AppliedToAll?.Invoke(setModels,id);
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(SetModel entity)
        {
            await _dataService.Update(entity);
            int currentIndex = _setModels.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _setModels[currentIndex] = entity;
            }
            else
            {
                _setModels.Add(entity);
            }
            Updated?.Invoke(entity);
        }
        public async Task UpdateRange(List<SetModel> entities)
        {
            await _updateRangeDataService.UpdateRange(entities);
            foreach (SetModel entity in entities)
            {
                int currentIndex = _setModels.FindIndex(y => y.Id == entity.Id);

                if (currentIndex != -1)
                {
                    _setModels[currentIndex] = entity;
                }
                else
                {
                    _setModels.Add(entity);
                }
            }

        }
    }
}
