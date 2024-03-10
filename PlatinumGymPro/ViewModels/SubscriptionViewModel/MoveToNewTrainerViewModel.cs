using PlatinumGym.Core.Models.Employee;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class MoveToNewTrainerViewModel : ViewModelBase
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
            SubmitCommand = new MoveToNewTrainerCommand(_subscriptionStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView), this);
        }

        private void ListTrainers()
        {
            foreach(var trainer in _subscriptionStore.SelectedSubscription!.Sport!.Trainers!)
            {
                AddTrainer(trainer);
            }
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

            }
        }
        #endregion
        private void AddTrainer(Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
    }
}
