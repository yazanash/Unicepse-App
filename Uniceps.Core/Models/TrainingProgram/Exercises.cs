using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.TrainingProgram
{
    public class Exercises
    {
        public int Id { get; set; }
        public int Tid { get; set; }
        public string? Name { get; set; }
        public int MuscleGroupId { get; set; }
        public string? Muscel { get; set; }
        public string? ImagePath { get; set; }
        public int Version { get; set; } = 0;
    }
}
