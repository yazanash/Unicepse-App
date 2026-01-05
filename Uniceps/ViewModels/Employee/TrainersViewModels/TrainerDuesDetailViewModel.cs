using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDuesDetailViewModel:ViewModelBase
    {
        public TrainerDuesDetail TrainerDuesDetail;

        public TrainerDuesDetailViewModel(TrainerDuesDetail trainerDuesDetail)
        {
            TrainerDuesDetail = trainerDuesDetail;
        }

        public int SubscriptionId => TrainerDuesDetail.SubscriptionId;
        public string? PlayerName => TrainerDuesDetail.PlayerName;
        public string? SportName => TrainerDuesDetail.SportName;
        public double PaymentValue => TrainerDuesDetail.PaymentValue;
        public string CoveredFrom => TrainerDuesDetail.CoveredFrom.ToShortDateString();
        public string CoveredTo => TrainerDuesDetail.CoveredTo.ToShortDateString();
        public int Days =>Convert.ToInt32(TrainerDuesDetail.CoveredTo.Subtract(TrainerDuesDetail.CoveredFrom).TotalDays);
        public double AmountForMonth => Math.Round(TrainerDuesDetail.AmountForMonth, 1);
        public bool IsLatePayment => TrainerDuesDetail.IsLatePayment;
    }
}
