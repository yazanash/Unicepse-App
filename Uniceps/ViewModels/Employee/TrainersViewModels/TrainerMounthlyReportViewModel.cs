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
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.Views.EmployeeViews;
using Uniceps.Views.EmployeeViews.CreditViews;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerMounthlyReportViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly DausesDataStore _dausesDataStore;
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
        public TrainerMounthlyReportViewModel(TrainerDueses trainerDueses, EmployeeStore employeeStore, CreditsDataStore creditsDataStore, DausesDataStore dausesDataStore)
        {
            this.trainerDueses = trainerDueses;
            _employeeStore = employeeStore;
            _creditsDataStore = creditsDataStore;
           
            _dausesDataStore = dausesDataStore;
        }
        public ICommand AddCreditCommand => new RelayCommand(ExecuteCreateCreditsCommand);

        private void ExecuteCreateCreditsCommand()
        {
            CreateCreditViewModelWindow createCreditViewModelWindow = new CreateCreditViewModelWindow(_creditsDataStore,FinalAmount,trainerDueses.Trainer!);
            CreateCreditWindowView createCreditWindowView = new CreateCreditWindowView();
            createCreditWindowView.DataContext = createCreditViewModelWindow;
            createCreditWindowView.ShowDialog();
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
