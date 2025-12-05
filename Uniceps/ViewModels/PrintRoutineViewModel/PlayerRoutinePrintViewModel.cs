using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.ViewModels.PrintRoutineViewModel
{
    public class PlayerRoutinePrintViewModel:ViewModelBase
    {
        public string GymName { get; set; } = "Uniceps";
        public string FullName { get; set; } = "";
        public string RoutineName { get; set; } = "";
        public string Date { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");
        public List<DayPrintVm> Days { get; set; } = new();
    }
}
