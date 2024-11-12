using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Sport;

namespace Unicepse.Core.Services
{
    public interface ISportDataService : IDataService<Sport>
    {
        Task<bool> DeleteConnectedTrainers(int id);
    }
}
