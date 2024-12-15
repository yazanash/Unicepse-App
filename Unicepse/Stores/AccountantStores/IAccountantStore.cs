using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores.AccountantStores
{
    public interface IAccountantStore<T>
    {
        Task GetAll(DateTime dateFrom, DateTime dateTo);
    }
}
