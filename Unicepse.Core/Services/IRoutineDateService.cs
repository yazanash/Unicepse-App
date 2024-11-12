using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.TrainingProgram;

namespace Unicepse.Core.Services
{
    public interface IRoutineDateService :IDataService<PlayerRoutine>
    {
        Task<IEnumerable<PlayerRoutine>> GetByDataStatus(DataStatus status);
        Task<PlayerRoutine> UpdateDataStatus(PlayerRoutine entity);
        Task<bool> DeleteRoutineItems(int id);
        Task<IEnumerable<PlayerRoutine>> GetAllTemp();
        Task<IEnumerable<Exercises>> GetAllExercises();
        Task<IEnumerable<PlayerRoutine>> GetAll(Player player);
    }
}
