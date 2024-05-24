using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using Unicepse.WPF.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.SubscriptionCommand
{
    public class MoveToNewTrainerCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly NavigationService<PlayerMainPageViewModel> _navigationService;
        private readonly MoveToNewTrainerViewModel _moveToNewTrainerViewModel;

        public MoveToNewTrainerCommand(SubscriptionDataStore subscriptionDataStore, NavigationService<PlayerMainPageViewModel> navigationService, MoveToNewTrainerViewModel moveToNewTrainerViewModel)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _navigationService = navigationService;
            _moveToNewTrainerViewModel = moveToNewTrainerViewModel;
            _moveToNewTrainerViewModel.PropertyChanged += _moveToNewTrainerViewModel_PropertyChanged;
        }

        private void _moveToNewTrainerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_moveToNewTrainerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _moveToNewTrainerViewModel.CanSubmit && _moveToNewTrainerViewModel.SelectedTrainer != null && base.CanExecute(null);
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _subscriptionDataStore.MoveToNewTrainer(_subscriptionDataStore.SelectedSubscription!, _subscriptionDataStore.SelectedTrainer!, _moveToNewTrainerViewModel.MoveDate);

                _navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
