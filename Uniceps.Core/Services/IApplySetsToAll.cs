using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.Core.Services
{
    public interface IApplySetsToAll
    {
        Task<List<SetModel>> ApplySetsToEntity(List<SetModel> entities, int itemId);
    }
}
