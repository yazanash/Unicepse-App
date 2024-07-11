﻿using Unicepse.Core.Models.Employee;
using Unicepse.Commands.SubscriptionCommand;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.SubscriptionViewModel
{
    public class MoveToNewTrainerViewModel : ErrorNotifyViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayerMainPageViewModel _playerMainPageView;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        public SubscriptionCardViewModel? Subscription { get; set; }
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public SubscriptionTrainerListItem? SelectedTrainer
        {
            get
            {
                return TrainerList
                    .FirstOrDefault(y => y?.trainer == _subscriptionStore.SelectedTrainer);
            }
            set
            {
                _subscriptionStore.SelectedTrainer = value?.trainer;
                OnPropertyChanged(nameof(SelectedTrainer));
                ClearError(nameof(SelectedTrainer));
                if (SelectedTrainer == null)
                {
                    AddError("يجب اختيار مدرب", nameof(SelectedTrainer));
                    OnErrorChanged(nameof(SelectedTrainer));
                }
                OnPropertyChanged(nameof(SelectedTrainer));
            }
        }
        public MoveToNewTrainerViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayerMainPageViewModel playerMainPageView)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playerMainPageView = playerMainPageView;
            _trainerListItemViewModels = new ObservableCollection<SubscriptionTrainerListItem>();
            Subscription = new SubscriptionCardViewModel(_subscriptionStore.SelectedSubscription!);
            ListTrainers();
            CancelCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView));

            SubmitCommand = new MoveToNewTrainerCommand(_subscriptionStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView), this);
        }

        private void ListTrainers()
        {
            foreach (var trainer in _subscriptionStore.SelectedSubscription!.Sport!.Trainers!)
            {
                AddTrainer(trainer);
            }
            if(_subscriptionStore.SelectedSubscription.Trainer!!=null)
            SelectedTrainer = TrainerList.FirstOrDefault(x => x.Id == _subscriptionStore.SelectedSubscription!.Trainer!.Id);
        }
        #region Properties
        private DateTime _moveDate = DateTime.Now;
        public DateTime MoveDate
        {
            get { return _moveDate; }
            set
            {
                _moveDate = value;
                OnPropertyChanged(nameof(MoveDate));
                ClearError(nameof(MoveDate));
                if (MoveDate < _subscriptionStore.SelectedSubscription!.RollDate)
                {
                    AddError("لا يمكن ان يكون تاريخ نقل الاشتراك اصغر من تاريخ الاشتراك", nameof(MoveDate));
                    OnErrorChanged(nameof(MoveDate));
                }
                else if (MoveDate >= _subscriptionStore.SelectedSubscription!.EndDate)
                {
                    AddError("لا يمكن ان يكون تاريخ نقل الاشتراك اكبر من تاريخ نهاية الاشتراك", nameof(MoveDate));
                    OnErrorChanged(nameof(MoveDate));
                }
                OnPropertyChanged(nameof(MoveDate));
            }
        }
        #endregion
        private void AddTrainer(emp.Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
    }
}
