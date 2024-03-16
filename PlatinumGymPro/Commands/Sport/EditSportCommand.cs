using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SportsViewModels;
using sp = PlatinumGym.Core.Models.Sport ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Sport
{
    internal class EditSportCommand : AsyncCommandBase
    {
        private readonly NavigationService<SportListViewModel> navigationService;
        private readonly SportDataStore _sportStore;
        private EditSportViewModel _editSportViewModel;
        public EditSportCommand(NavigationService<SportListViewModel> navigationService, EditSportViewModel editSportViewModel, SportDataStore sportStore)
        {

            this.navigationService = navigationService;
            _sportStore = sportStore;
            _editSportViewModel = editSportViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {

            sp.Sport sport = new sp.Sport()
            {
                Id = _sportStore.SelectedSport!.Id,
                Name = _editSportViewModel.SportName,
                DailyPrice = _editSportViewModel.DailyPrice,
                DaysCount = _editSportViewModel.SubscribeLength,
                DaysInWeek = _editSportViewModel.WeeklyTrainingDays,
                Price = _editSportViewModel.MonthlyPrice,
                IsActive = true

            };
            await _sportStore.DeleteConnectedTrainers(sport.Id);
            foreach (var TrainerListItem in _editSportViewModel.TrainerList)
            {
                if (TrainerListItem.IsSelected)
                    sport.Trainers!.Add(TrainerListItem.trainer);
            }
            await _sportStore.Update(sport);

            MessageBox.Show("Sport added successfully");
            navigationService.Navigate();
        }

    }

}
