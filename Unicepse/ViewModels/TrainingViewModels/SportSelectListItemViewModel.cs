﻿using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Sport;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.TrainingViewModels
{
    public class SportSelectListItemViewModel : ViewModelBase
    {
        public Sport Sport;

        public int Id => Sport.Id;
        public string? SportName => Sport.Name + Sport.Trainers!.Count();
        public double Price => Sport.Price;
        public bool IsActive => Sport.IsActive;
        public int DaysInWeek => Sport.DaysInWeek;
        public double DailyPrice => Sport.DailyPrice;

        public int DaysCount => Sport.DaysCount;

        public List<emp.Employee> TrainerList => Sport.Trainers?.ToList() ?? new List<emp.Employee>();
        public SportSelectListItemViewModel(Sport sport)
        {
            Sport = sport;
        }


    }
}
