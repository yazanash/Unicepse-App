
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.Core.Models.RoutineModels
{
    public class SetModel
    {
        public int Id { get; set; }
        public int RoundIndex { get; set; }
        public int Repetition { get; set; }
        public int RoutineItemId { get; set; }
        public RoutineItemModel? RoutineItem { get; set; }
    }
}
