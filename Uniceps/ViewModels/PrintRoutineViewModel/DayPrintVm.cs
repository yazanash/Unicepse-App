using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.ViewModels.PrintRoutineViewModel
{
    public class DayPrintVm:ViewModelBase
    {
        public int Order { get; set; }
        public string Name { get; set; } = "";
        public List<ItemPrintVm> Items { get; set; } = new();
    }
}
