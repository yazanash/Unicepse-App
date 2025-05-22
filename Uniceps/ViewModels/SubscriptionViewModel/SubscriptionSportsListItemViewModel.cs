using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.utlis.common;
using Uniceps.Core.Models.Sport;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionSportsListItemViewModel : ViewModelBase
    {
        public Sport Sport;

        public int Id => Sport.Id;
        public string? SportName => Sport.Name;
        public double Price => Sport.Price;
        public bool IsActive => Sport.IsActive;
        public int DaysInWeek => Sport.DaysInWeek;
        public double DailyPrice => Sport.DailyPrice;

        public int DaysCount => Sport.DaysCount;

        public SubscriptionSportsListItemViewModel(Sport sport)
        {
            Sport = sport;
        }

    }
}
