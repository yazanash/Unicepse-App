using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;

namespace Uniceps.Stores.ApiDataStores
{
    public class SyncStore
    {
        private readonly IDataService<SyncObject> _syncDataService;

        public SyncStore(IDataService<SyncObject> syncDataService)
        {
            _syncDataService = syncDataService;
        }

        public async Task Add(SyncObject entity)
        {
            await _syncDataService.Create(entity);
        }

        public async Task Delete(int entity_id)
        {
            await _syncDataService.Delete(entity_id);
        }

        public async Task<IEnumerable<SyncObject>> GetAll()
        {
            return await _syncDataService.GetAll();
        }

    }
}
