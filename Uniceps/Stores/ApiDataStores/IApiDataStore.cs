using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SyncModel;

namespace Uniceps.Stores.ApiDataStores
{
    public interface IApiDataStore<T>
    {
        public Task Create(T entity);
        public Task Update(T entity);
        public Task Sync(SyncObject syncObject);
    }
}
