using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;

namespace Unicepse.Core.Services
{
    public interface ICreditDataService : IDataService<Credit>
    {
        Task<IEnumerable<Credit>> GetAll(DateTime date);
        Task<IEnumerable<Credit>> GetAll(Employee trainer, DateTime date);
        Task<IEnumerable<Credit>> GetAll(Employee trainer);
    }
}
