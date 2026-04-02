using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models
{
    public class DashboardAnalyticsModel
    {
        public double TotalPaymentsToday { get; set; }
        public double TotalExpensesToday { get; set; }
        public int ActivePlayersCount { get; set; }

        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }

        public Dictionary<string, int> SportPopularity { get; set; } = new Dictionary<string, int>();

        public List<HourlyAttendanceDto> PeakHours { get; set; } = new List<HourlyAttendanceDto>();

        public List<FinancialHistoryDto> FinancialHistory { get; set; } = new List<FinancialHistoryDto>();
        public int StaffCount { get; set; }
        public int TrainersCount { get; set; }
        public int CurrentPresentPlayers { get; set; }
        public List<DayAttendanceDto> WeeklyAttendance { get; set; } = new List<DayAttendanceDto>();
        public double TotalCredits { get; set; }
    }
    public class HourlyAttendanceDto
    {
        public string Hour => $"{HourInt:D2}:00";
        public int Count { get; set; }
        public int HourInt { get; set; }
        public bool IsMale { get; set; }
    }
    public class FinancialHistoryDto
    {
        public string Month { get; set; } = string.Empty;
        public double Revenue { get; set; }
        public double Expenses { get; set; }
    }
    public class DayAttendanceDto
    {
        public DateTime Date { get; set; } 
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
    }
    
}
