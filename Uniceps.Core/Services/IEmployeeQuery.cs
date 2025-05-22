using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.Core.Services
{
    public interface IEmployeeQuery
    {
        Task<IEnumerable<Employee>> GetAll();
    }
}
