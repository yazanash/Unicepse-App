using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.Stores.SystemAuthStores
{
    public interface IProfileDataStore
    {
        public event Action<SystemProfile>? Created;
        public event Action<SystemProfile>? Updated;
       
        public Task CreateOrUpdate(SystemProfile entity);
        Task<bool> CheckAndSyncProfileAsync(string businessId);
        Task UploadProfilePicture(string filePath);

    }
}
