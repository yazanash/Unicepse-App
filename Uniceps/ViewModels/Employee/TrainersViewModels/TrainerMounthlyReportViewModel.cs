using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Employee;
using Uniceps.navigation;
using Uniceps.navigation.DialogService;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.Views.EmployeeViews;
using Uniceps.Views.EmployeeViews.CreditViews;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerMounthlyReportViewModel : ViewModelBase
    {
        private readonly DausesDataStore _dausesDataStore;
        private readonly IDialogService _dialogService;
        public TrainerDueses trainerDueses;
        public int Id => trainerDueses.Id;
        public double TotalSubscriptions => trainerDueses.TotalSubscriptions;
        public int CountSubscription => trainerDueses.CountSubscription;
        public DateTime IssueDate => trainerDueses.IssueDate;
        public string IssueDateText => trainerDueses.IssueDate.ToShortDateString();
        public string Parcent => trainerDueses.Parcent * 100 + "%";
        public double DausesFromParcent => trainerDueses.TotalSubscriptions;
        public double TotalDause => trainerDueses.TotalSubscriptions + trainerDueses.Salary;
        public double Credits => trainerDueses.Credits;
        public double CreditsCount => trainerDueses.CreditsCount;
        public double FinalAmount => TotalDause - trainerDueses.Credits;
        public double Salary => trainerDueses.Salary;
        public double BalanceForward => trainerDueses.BalanceForward;
        public double TotalEarnedUntilNow => trainerDueses.Details.Sum(d => d.EarnedUntilNow);
        public double ActualFairBalance => BalanceForward + TotalSalaryDebt + TotalEarnedUntilNow - Credits;
        public double FinalBalance => trainerDueses.FinalBalance;

        public double Salaries => trainerDueses.Salaries;
        public double TotalSalaryDebt => trainerDueses.TotalSalaryDebt;
        public DateTime LastClosingDate => trainerDueses.Trainer?.LastClosingDate ?? trainerDueses.Trainer?.StartDate??DateTime.Now;
        public TrainerMounthlyReportViewModel(TrainerDueses trainerDueses, DausesDataStore dausesDataStore, IDialogService dialogService)
        {
            this.trainerDueses = trainerDueses;
            _dausesDataStore = dausesDataStore;
            _dialogService = dialogService;
        }
        public ICommand AddCreditCommand => new RelayCommand(ExecuteCreateCreditsCommand);

        private void ExecuteCreateCreditsCommand()
        {
            if (trainerDueses.Trainer != null)
            {
                var navParams = new Dictionary<string, object>
            {
                {"Amount",FinalAmount},
                {"Employee",trainerDueses.Trainer},

            };
                _dialogService.ShowDialog<CreateCreditWindowView, CreateCreditViewModelWindow>(navParams);
            }
        }

        internal void Update(TrainerDueses obj)
        {
            trainerDueses = obj;
        }
        public ICommand ViewDetailCommand => new RelayCommand(ExecuteViewDetailCommand);

        private void ExecuteViewDetailCommand()
        {
            TrainerDauseDetailsViewModel trainerDauseDetailsViewModel = new TrainerDauseDetailsViewModel(trainerDueses);
            TrainerDauseDetails trainerDauseDetails = new TrainerDauseDetails();
            trainerDauseDetails.DataContext = trainerDauseDetailsViewModel;
            trainerDauseDetails.ShowDialog();
        }
        public ICommand CloseTrainerAccountCommand => new AsyncRelayCommand(ExecuteCloseTrainerAccountCommand);

        private async Task ExecuteCloseTrainerAccountCommand()
        {
          await _dausesDataStore.CloseTrainerAccountAsync();
        }
    }

}
