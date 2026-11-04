using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Services.DataSyncServices
{
    public interface IPushPullDataService<T>
    {
        Task<IEnumerable<T>> GetDataToPush();
        Task SaveDataFromPullAsync(List<T> data);
        Task MarkAsSynced(List<T> data);
        Task<long> GetLatestVersion();
    }
}
