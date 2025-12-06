using Uniceps.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerMounthlyReportViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigatorStore;
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly CreditListViewModel _creditListViewModel;

        public TrainerDueses trainerDueses;
        public int Id => trainerDueses.Id;
        public double TotalSubscriptions => trainerDueses.TotalSubscriptions;
        public int CountSubscription => trainerDueses.CountSubscription;
        public DateTime IssueDate => trainerDueses.IssueDate;
        public string IssueDateText => trainerDueses.IssueDate.ToShortDateString();
        public string Parcent => trainerDueses.Parcent * 100 + "%";
        public double DausesFromParcent => trainerDueses.TotalSubscriptions * trainerDueses.Parcent;
        public double TotalDause => trainerDueses.TotalSubscriptions * trainerDueses.Parcent + trainerDueses.Salary;
        public double Credits => trainerDueses.Credits;
        public double CreditsCount => trainerDueses.CreditsCount;
        public double FinalAmount => TotalDause - trainerDueses.Credits;
        public double Salary => trainerDueses.Salary;
        public TrainerMounthlyReportViewModel(TrainerDueses trainerDueses, NavigationStore navigatorStore, EmployeeStore employeeStore, CreditsDataStore creditsDataStore, CreditListViewModel creditListViewModel)
        {
            this.trainerDueses = trainerDueses;
            _employeeStore = employeeStore;
            _navigatorStore = navigatorStore;
            _creditsDataStore = creditsDataStore;
            _creditListViewModel = creditListViewModel;
            AddCreditCommand = new NavaigateCommand<CreditDetailsViewModel>(new NavigationService<CreditDetailsViewModel>(_navigatorStore, () => new CreditDetailsViewModel(_employeeStore, _creditsDataStore, _navigatorStore, _creditListViewModel, FinalAmount)));

        }
        public ICommand AddCreditCommand { get; }
        internal void Update(TrainerDueses obj)
        {
            trainerDueses = obj;
        }


    }
}
