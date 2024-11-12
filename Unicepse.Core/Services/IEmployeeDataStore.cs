using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;

namespace Unicepse.Core.Services
{
    public interface IEmployeeDataStore : IDataService<Employee>
    {
        Task<bool> DeleteConnectedSports(int id);
        Task<IEnumerable<Employee>> GetAllParcentTrainers();
    }
}
