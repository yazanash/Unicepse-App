using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.ViewModels.Employee.CreditViewModels;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDauseDetailsViewModel :ViewModelBase
    {
        public TrainerDueses trainerDueses;
        public TrainerDauseDetailsViewModel(TrainerDueses trainerDueses)
        {
            this.trainerDueses = trainerDueses;
            foreach (var item in this.trainerDueses.Details)
            {
                Details.Add(new TrainerDuesDetailViewModel(item));
            }
            foreach (var item in this.trainerDueses.SalaryDetails)
            {
                SalaryDetails.Add(new SalaryDetailsViewModel(item));
            }
            foreach (var item in this.trainerDueses.CreditDetails)
            {
                CreditsDetails.Add(new CreditListItemViewModel(item));
            }
            this.trainerDueses = trainerDueses;
        }
        public ObservableCollection<TrainerDuesDetailViewModel> Details { get; set; } = new ObservableCollection<TrainerDuesDetailViewModel>();

        public ObservableCollection<SalaryDetailsViewModel> SalaryDetails { get; set; } = new ObservableCollection<SalaryDetailsViewModel>();

        public ObservableCollection<CreditListItemViewModel> CreditsDetails { get; set; } = new ObservableCollection<CreditListItemViewModel>();
    }
}
