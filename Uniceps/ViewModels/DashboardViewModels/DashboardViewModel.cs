using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Models;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.ViewModels.DashboardViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly GymStore _gymStore;

        public DashboardViewModel(GymStore gymStore)
        {
            _gymStore = gymStore;
            _gymStore.AnalyticsLoaded += _gymStore_AnalyticsLoaded;
        }

        private void _gymStore_AnalyticsLoaded()
        {
            LoadData(_gymStore.DashboardAnalyticsModel);
        }

        public ISeries[]? GenderSeries { get; set; }
        public ISeries[]? PeakHoursSeries { get; set; }
        public Axis[]? XAxesPeak { get; set; }
        public ISeries[]? FinancialSeries { get; set; }
        public Axis[]? XAxesFinancial { get; set; }

        public double TodayRevenue { get; set; }
        public double TodayExpenses { get; set; }
        public double TodayCredits{ get; set; }
        public ISeries[]? SportPopularitySeries { get; set; }
        public Axis[]? YAxesInteger { get; set; }
        public int StaffCount { get; set; }
        public int TrainersCount { get; set; }
      
        public int CurrentPresentPlayers { get; set; }
        public ISeries[]? WeeklyDaysSeries { get; set; }
        public Axis[]? XAxesDays { get; set; }
        private void LoadData(DashboardAnalyticsModel data)
        {

            GenderSeries = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new[] { data.MaleCount },
                    Name = "ذكور",
                    Fill = new SolidColorPaint(SKColors.DeepSkyBlue)
                },
                new PieSeries<int>
                {
                    Values = new[] { data.FemaleCount },
                    Name = "إناث",
                    Fill = new SolidColorPaint(SKColors.HotPink)
                }
            };
            SportPopularitySeries = data.SportPopularity.Select(sport => new PieSeries<int>
            {
                Values = new[] { sport.Value },
                Name = sport.Key,
                //DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                //DataLabelsPaint = new SolidColorPaint(SKColors.White),
            }).ToArray();
            var hoursRange = Enumerable.Range(7, 17).ToList();
            PeakHoursSeries = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = hoursRange.Select(h => data.PeakHours.FirstOrDefault(x => x.HourInt == h && x.IsMale)?.Count ?? 0).ToArray(),
                    Name = "ذكور",
                    Fill = new SolidColorPaint(SKColors.DeepSkyBlue.WithAlpha(200)),
                    Padding = 2
                },
                new ColumnSeries<int>
                {
                    Values = hoursRange.Select(h => data.PeakHours.FirstOrDefault(x => x.HourInt == h && !x.IsMale)?.Count ?? 0).ToArray(),
                    Name = "إناث",
                    Fill = new SolidColorPaint(SKColors.HotPink.WithAlpha(200)),
                    Padding = 2
                }
            };

            XAxesPeak = new Axis[] 
            {
                new Axis {
                    Labels = hoursRange.Select(h => $"{h:D2}:00").ToArray(),
                }
            };
            YAxesInteger = new Axis[] {
                new Axis {
                    MinStep = 1,
                    Labeler = val => val.ToString("N0")
                }
            };
            FinancialSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = data.FinancialHistory.Select(x => x.Revenue).ToArray(),
                    Name = "إيرادات",
                    Fill = new SolidColorPaint(SKColors.LightGreen.WithAlpha(100)),
                    GeometrySize = 10
                },
                new LineSeries<double>
                {
                    Values = data.FinancialHistory.Select(x => x.Expenses).ToArray(),
                    Name = "مصاريف",
                    Fill = new SolidColorPaint(SKColors.Tomato.WithAlpha(100)),
                    GeometrySize = 10
                }
            };

            XAxesFinancial = new Axis[]
            {
                new Axis { Labels = data.FinancialHistory.Select(x => x.Month).ToArray() }
            };
            WeeklyDaysSeries = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = data.WeeklyAttendance.Select(x => x.MaleCount).ToArray(),
                    Name = "ذكور",
                    Fill = new SolidColorPaint(SKColors.DeepSkyBlue.WithAlpha(200)),
                    Padding = 2
                },
                new ColumnSeries<int>
                {
                    Values = data.WeeklyAttendance.Select(x => x.FemaleCount).ToArray(),
                    Name = "إناث",
                    Fill = new SolidColorPaint(SKColors.HotPink.WithAlpha(200)),
                    Padding = 2
                }
            };

            XAxesDays = new Axis[]
            {
                new Axis
                {
                    Labels = data.WeeklyAttendance.Select(x => x.Date.ToString("dddd")).ToArray()
                }
            };
            TodayRevenue = data.TotalPaymentsToday;
            TodayExpenses = data.TotalExpensesToday;
            StaffCount = data.StaffCount;
            TrainersCount = data.TrainersCount;
            CurrentPresentPlayers = data.CurrentPresentPlayers;
            TodayCredits = data.TotalCredits;
            OnPropertyChanged(string.Empty);
        }

        public ICommand LoadAnalyticsCommand => new AsyncRelayCommand(ExecuteLoadAnalyticsCommand);

        public async Task ExecuteLoadAnalyticsCommand()
        {
            await _gymStore.GetAnalytics();
        }

        public static DashboardViewModel LoadViewModel(GymStore gymStore)
        {
            DashboardViewModel viewModel = new DashboardViewModel(gymStore);
            viewModel.LoadAnalyticsCommand.Execute(null);
            return viewModel;
        }
    }
}
