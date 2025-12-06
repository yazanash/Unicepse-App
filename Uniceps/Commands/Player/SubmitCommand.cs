//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views;

namespace Uniceps.Commands.Player
{
    public class SubmitCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly AddPlayerViewModel _addPlayerViewModel;
        public SubmitCommand(AddPlayerViewModel addPlayerViewModel, PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
            _addPlayerViewModel = addPlayerViewModel;
            _addPlayerViewModel.PropertyChanged += AddPlayerViewModel_PropertyChanged;
        }

        private void AddPlayerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addPlayerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {

            return _addPlayerViewModel.CanSubmit && !string.IsNullOrEmpty(_addPlayerViewModel.FullName) && _addPlayerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                _addPlayerViewModel.Submited = false;
                Core.Models.Player.Player player = new()
                {
                    FullName = _addPlayerViewModel.FullName,
                    BirthDate = _addPlayerViewModel.Year?.year??DateTime.Now.Year,
                    GenderMale = _addPlayerViewModel.GenderMale,
                    Hieght = _addPlayerViewModel.Hieght,
                    Phone = _addPlayerViewModel.Phone,
                    SubscribeDate = _addPlayerViewModel.SubscribeDate,
                    SubscribeEndDate = _addPlayerViewModel.SubscribeDate.AddDays(30),
                    Weight = _addPlayerViewModel.Weight,
                    IsSubscribed = true,
                };
                await _playerStore.AddPlayer(player);
                _playerStore.SelectedPlayer = player;
             
                _addPlayerViewModel.Submited = true;
                if (MessageBox.Show("تم اضافة اللاعب بنجاح ... هل تريد اضافة لاعب اخر؟", "تم بنجاح", MessageBoxButton.YesNo, MessageBoxImage.Information)
             == MessageBoxResult.Yes)
                {
                    _addPlayerViewModel.ClearForm();
                }
                else
                    _addPlayerViewModel.OnPlayerCreated();
            }
            catch (PlayerConflictException ex)
            {
                _addPlayerViewModel.ClearError(nameof(_addPlayerViewModel.FullName));
                _addPlayerViewModel.AddError(ex.Message, nameof(_addPlayerViewModel.FullName));
                _addPlayerViewModel.OnErrorChanged(nameof(_addPlayerViewModel.FullName));
            }
            catch(FreeLimitException)
            {
                PremiumViewDialog premiumViewDialog = new PremiumViewDialog();
                premiumViewDialog.ShowDialog();
            }
        }


    }
}
