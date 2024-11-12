using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Player;

namespace Unicepse.Core.Services
{
    public interface IMetricDataService: IDataService<Metric>
    {
        Task<IEnumerable<Metric>> GetByDataStatus(DataStatus status);
        Task<Metric> UpdateDataStatus(Metric entity);
        Task<IEnumerable<Metric>> GetAll(Player player);
    }
}
