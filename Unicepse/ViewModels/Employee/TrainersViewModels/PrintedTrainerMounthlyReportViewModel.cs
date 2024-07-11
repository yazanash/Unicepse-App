﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class PrintedTrainerMounthlyReportViewModel : ViewModelBase
    {

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
        public PrintedTrainerMounthlyReportViewModel(TrainerDueses trainerDueses)
        {
            this.trainerDueses = trainerDueses;
        }


    }
}
