using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.Models
{
    public class EssentialsReponse
    {
        public List<MuscleGroupDto> MuscleGroups { get; set; } = new List<MuscleGroupDto>();
        public List<EquipmentsResponse> Equipments { get; set; } = new List<EquipmentsResponse>();
    }
}
