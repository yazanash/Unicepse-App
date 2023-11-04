using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.SportsCommands
{
    public class SubmitSportCommand : AsyncCommandBase
    {
        private readonly NavigationService<SportListViewModel> navigationService;
        private readonly SportStore _sportStore;
        private AddSportViewModel _addSportViewModel;
        public SubmitSportCommand(NavigationService<SportListViewModel> navigationService, SportStore sportStore, AddSportViewModel addSportViewModel)
        {

            this.navigationService = navigationService;
            _sportStore = sportStore;
            _addSportViewModel = addSportViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
           foreach (var item in _addSportViewModel.TrainerList)
            {
                MessageBox.Show(item.TrainerName + " " + item.IsSelected.ToString());
            }
            Sport sport = new Sport()
            {
                Name = _addSportViewModel.SportName,
                DailyPrice = _addSportViewModel.DailyPrice,
                DaysCount = _addSportViewModel.SubscribeLength,
                DaysInWeek = _addSportViewModel.WeeklyTrainingDays,
                Price = _addSportViewModel.MonthlyPrice,
                IsActive = true

            };
            List<Employee> trainers = new();
            foreach(var TrainerListItem in _addSportViewModel.TrainerList)
            {
                trainers.Add(TrainerListItem.trainer);
            }
           
            await _sportStore.Add(sport,trainers);
          
          

            //await _sportStore.Update(sport);
            MessageBox.Show("Sport added successfully");
            navigationService.Navigate();
        }
       
    }
}
