using Unicepse.Core.Models.Employee;
using sp = Unicepse.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.ViewModels.SportsViewModels;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.Sport
{
    public class SubmitSportCommand : AsyncCommandBase
    {
        private readonly NavigationService<SportListViewModel> navigationService;
        private readonly SportDataStore _sportStore;
        private AddSportViewModel _addSportViewModel;
        public SubmitSportCommand(NavigationService<SportListViewModel> navigationService, AddSportViewModel addSportViewModel, SportDataStore sportStore)
        {

            this.navigationService = navigationService;
            _sportStore = sportStore;
            _addSportViewModel = addSportViewModel;
            _addSportViewModel.PropertyChanged += _addSportViewModel_PropertyChanged;
        }

        private void _addSportViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addSportViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _addSportViewModel.CanSubmit && !string.IsNullOrEmpty(_addSportViewModel.SportName?.Trim()) && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                sp.Sport sport = new sp.Sport()
                {
                    Name = _addSportViewModel.SportName,
                    DailyPrice = _addSportViewModel.DailyPrice,
                    DaysCount = _addSportViewModel.SubscribeLength,
                    DaysInWeek = _addSportViewModel.WeeklyTrainingDays,
                    Price = _addSportViewModel.MonthlyPrice,
                    IsActive = true

                };
                foreach (var TrainerListItem in _addSportViewModel.TrainerList)
                {
                    if (TrainerListItem.IsSelected)
                        sport.Trainers!.Add(TrainerListItem.trainer);
                }
                await _sportStore.Add(sport);

                MessageBox.Show("Sport add successfully");
                navigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
