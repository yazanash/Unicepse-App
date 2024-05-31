using sp = Unicepse.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.ViewModels.SportsViewModels;
using Unicepse.Stores;
using Unicepse.Commands;
using Unicepse.navigation;

namespace Unicepse.Commands.Sport
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
            _editSportViewModel.PropertyChanged += _addSportViewModel_PropertyChanged;
        }
        private void _addSportViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editSportViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _editSportViewModel.CanSubmit && !string.IsNullOrEmpty(_editSportViewModel.SportName?.Trim()) && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
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

                //MessageBox.Show("Sport edited successfully");
                navigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }

}
