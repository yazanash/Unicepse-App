using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Uniceps.ViewModels.PrintRoutineViewModel
{
    public class PlayerRoutinePrintViewModel:ViewModelBase
    {
        public string GymName { get; set; } = "Uniceps";
        private string? _gymLogo;

        public string? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        public string FullName { get; set; } = "";
        public string RoutineName { get; set; } = "";
        public string Date { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");
        public List<DayPrintVm> Days { get; set; } = new();
       
    }
}
