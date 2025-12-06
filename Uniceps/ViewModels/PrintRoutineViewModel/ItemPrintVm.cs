using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Models.RoutineExportModels;

namespace Uniceps.ViewModels.PrintRoutineViewModel
{
    public class ItemPrintVm:ViewModelBase
    {
        public int Order { get; set; }
        public string ExerciseName { get; set; } = "";
        public int ExerciseId { get; set; }
        public string ExerciseUrl { get; set; } = "";
        public string Muscle { get; set; } = "";
        public List<SetPrintVm> Sets { get; set; } = new();
        public string SetsText => string.Join(" × ", Sets.Select(s => s.Repetition));
    }
}
