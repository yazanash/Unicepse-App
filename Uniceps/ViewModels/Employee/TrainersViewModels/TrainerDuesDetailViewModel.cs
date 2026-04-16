using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDuesDetailViewModel : ViewModelBase
    {
        public TrainerDuesDetail TrainerDuesDetail;

        public TrainerDuesDetailViewModel(TrainerDuesDetail trainerDuesDetail)
        {
            TrainerDuesDetail = trainerDuesDetail;
        }
        private int _order;
        public int Order { get => _order; set { _order = value; OnPropertyChanged(nameof(Order)); } }
        public int SubscriptionId => TrainerDuesDetail.SubscriptionId;
        public string? PlayerName => TrainerDuesDetail.PlayerName;
        public string? SportName => TrainerDuesDetail.SportName;
        public double PaymentValue => TrainerDuesDetail.PaymentValue;
        public string CoveredFrom => TrainerDuesDetail.CoveredFrom.ToShortDateString();
        public string CoveredTo => TrainerDuesDetail.CoveredTo.ToShortDateString();
        public int Days => Convert.ToInt32(TrainerDuesDetail.CoveredTo.Subtract(TrainerDuesDetail.CoveredFrom).TotalDays);
        public double AmountForMonth => Math.Round(TrainerDuesDetail.AmountForMonth, 1);
        public bool IsLatePayment => TrainerDuesDetail.IsLatePayment;
        public double TotalAmount => Math.Round(TrainerDuesDetail.AmountForMonth, 0);

        public double EarnedUntilNow => Math.Round(TrainerDuesDetail.EarnedUntilNow, 0);

        public double RemainingToEarn => TotalAmount - EarnedUntilNow;

        public double ProgressPercentage => (TotalAmount > 0) ? (EarnedUntilNow / TotalAmount) * 100 : 0;
    }
}
