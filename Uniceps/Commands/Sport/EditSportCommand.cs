using Sp = Uniceps.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.SportsViewModels;

namespace Uniceps.Commands.Sport
{
    internal class EditSportCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;
        private EditSportViewModel _editSportViewModel;
        public EditSportCommand( EditSportViewModel editSportViewModel, SportDataStore sportStore)
        {
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
            //try
            //{
            Core.Models.Sport.Sport sport = new Sp.Sport()
            {
                Id = _sportStore.SelectedSport!.Id,
                Name = _editSportViewModel.SportName,
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

            MessageBox.Show("تم التعديل بنجاح");
            _editSportViewModel.OnSportUpdated();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

    }

}
