using System;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Exceptions;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
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
                    BirthDate = _addPlayerViewModel.Year,
                    GenderMale = _addPlayerViewModel.GenderMale,
                    MediclStatus = _addPlayerViewModel.MediclStatus,
                    Phone = _addPlayerViewModel.Phone,
                    SubscribeDate = _addPlayerViewModel.SubscribeDate,
                    SubscribeEndDate = _addPlayerViewModel.SubscribeDate.AddDays(30),
                    IsSubscribed = true,
                    FingerprintData = _addPlayerViewModel.UID
                };
                if (_addPlayerViewModel.IsEditMode)
                {
                    player.Id = _addPlayerViewModel.PlayerId;
                    await _playerStore.UpdatePlayer(player);
                    MessageBox.Show("تم تعديل اللاعب بنجاح", "تم بنجاح");
                    _addPlayerViewModel.OnPlayerCreated();
                }
                else
                {
                    await _playerStore.AddPlayer(player);
                    if (MessageBox.Show("تم اضافة اللاعب بنجاح ... هل تريد اضافة لاعب اخر؟", "تم بنجاح", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        _addPlayerViewModel.ClearForm();
                    }
                    else
                        _addPlayerViewModel.OnPlayerCreated();
                }
                _addPlayerViewModel.Submited = true;
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
