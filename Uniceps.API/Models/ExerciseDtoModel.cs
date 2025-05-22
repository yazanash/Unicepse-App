using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.Models
{
    public class ExerciseDtoModel
    {
        public int id { get; set; }
        public string? imageUrl { get; set; }
        public string? name { get; set; }
        public int muscleGroupId { get; set; }
    }
}
