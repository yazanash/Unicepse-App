using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class DaysListItemViewModel : ViewModelBase
    {
        public DaysListItemViewModel(int dayNum, string? dayName)
        {
            DayNum = dayNum;
            DayName = dayName;
        }

        public int DayNum { get;set; }
        public string? DayName { get; set; }

    }
}