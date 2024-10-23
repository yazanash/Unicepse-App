using Unicepse.Core.Models.Employee;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.ViewModels.PrintViewModels;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.Commands.Player;
using Unicepse.ViewModels.Employee.CreditViewModels;
using Unicepse.navigation;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
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
        public string Parcent => trainerDueses.Parcent *100 + "%";
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
            AddCreditCommand = new NavaigateCommand<CreditDetailsViewModel>(new NavigationService<CreditDetailsViewModel>(_navigatorStore, () => new CreditDetailsViewModel(_employeeStore, _creditsDataStore, _navigatorStore, _creditListViewModel,this.FinalAmount)));
           
        }
        public ICommand AddCreditCommand { get; }
        internal void Update(TrainerDueses obj)
        {
            trainerDueses = obj;
        }


    }
}
