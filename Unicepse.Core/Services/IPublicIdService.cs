using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Services
{
    public interface IPublicIdService<T>
    {
        Task<T?> GetByUID(string uid);
    }
}
