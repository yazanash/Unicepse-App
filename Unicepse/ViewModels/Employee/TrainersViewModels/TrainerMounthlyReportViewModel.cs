﻿using Unicepse.Core.Models.Employee;
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

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class TrainerMounthlyReportViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly EmployeeAccountantPageViewModel? _employeeAccountantPageViewModel;
        public TrainerDueses trainerDueses;
        public int Id => trainerDueses.Id;
        public double TotalSubscriptions => trainerDueses.TotalSubscriptions;
        public int CountSubscription => trainerDueses.CountSubscription;
        public DateTime IssueDate => trainerDueses.IssueDate;
        public string IssueDateText => trainerDueses.IssueDate.ToShortDateString();
        public double Parcent => trainerDueses.Parcent;
        public double DausesFromParcent => trainerDueses.TotalSubscriptions * trainerDueses.Parcent;
        public double TotalDause => trainerDueses.TotalSubscriptions * trainerDueses.Parcent + trainerDueses.Salary;
        public double Credits => trainerDueses.Credits;
        public double CreditsCount => trainerDueses.CreditsCount;
        public double FinalAmount => TotalDause - trainerDueses.Credits;
        public double Salary => trainerDueses.Salary;
        public TrainerMounthlyReportViewModel(TrainerDueses trainerDueses, EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel? employeeAccountantPageViewModel)
        {
            this.trainerDueses = trainerDueses;
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _employeeAccountantPageViewModel = employeeAccountantPageViewModel;


            //PrintCommand = new PrintCommand(new PrintWindowViewModel(CreateTrainerReport(_employeeStore, _dausesDataStore, employeeAccountantPageViewModel), new NavigationStore()));
        }

        internal void Update(TrainerDueses obj)
        {
            trainerDueses = obj;
        }
        public ICommand? PrintCommand { get; }

        private static TrainerDetiledReportViewModel CreateTrainerReport(EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel employeeAccountantPageViewModel)
        {
            return TrainerDetiledReportViewModel.LoadViewModel(employeeStore, dausesDataStore, employeeAccountantPageViewModel);
            //return new TrainerDetiledReportViewModel(employeeStore, dausesDataStore, trainerMounthlyReportViewModel);
        }
    }
}
