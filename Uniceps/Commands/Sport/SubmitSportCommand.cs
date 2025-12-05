using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views;
using Sp = Uniceps.Core.Models.Sport;

namespace Uniceps.Commands.Sport
{
    public class SubmitSportCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;
        private AddSportViewModel _addSportViewModel;
        public SubmitSportCommand( AddSportViewModel addSportViewModel, SportDataStore sportStore)
        {
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
                Core.Models.Sport.Sport sport = new Sp.Sport()
                {
                    Name = _addSportViewModel.SportName,
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

                if (MessageBox.Show("تم اضافة الرياضة بنجاح ... هل تريد اضافة رياضة اخرى؟", "تم بنجاح", MessageBoxButton.YesNo, MessageBoxImage.Information)
               == MessageBoxResult.Yes)
                {
                    _addSportViewModel.ClearForm();
                }
                else
                _addSportViewModel.OnSportCreated();
            }
            catch (FreeLimitException)
            {
                PremiumViewDialog premiumViewDialog = new PremiumViewDialog();
                premiumViewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

    }
}
