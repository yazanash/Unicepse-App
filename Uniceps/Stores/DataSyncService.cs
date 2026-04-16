using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;

namespace Uniceps.Stores
{
    public class DataSyncService
    {
        public event Action<Player>? PlayerUpdated;
        public event Action<Sport>? SportUpdated;
        public event Action<Employee>? TrainerUpdated;

        public void NotifyPlayerUpdated(Player player) => PlayerUpdated?.Invoke(player);
        public void NotifySportUpdated(Sport sport) => SportUpdated?.Invoke(sport);
        public void NotifyTrainerUpdated(Employee trainer) => TrainerUpdated?.Invoke(trainer);
    }
}
