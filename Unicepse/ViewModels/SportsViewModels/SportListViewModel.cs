﻿using Unicepse.Core.Models.Sport;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Sport;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.Stores.SportStores;

namespace Unicepse.ViewModels.SportsViewModels
{
    public class SportListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SportListItemViewModel> sportListItemViewModels;

        private NavigationStore _navigatorStore;
        private SportDataStore _sportStore;
        private EmployeeStore _trainerStore;
        private SportSubscriptionDataStore _subscriptionDataStore;
        public IEnumerable<SportListItemViewModel> SportList => sportListItemViewModels;
        public SearchBoxViewModel SearchBox { get; set; }

        public ICommand AddSportCommand { get; }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand LoadSportsCommand { get; }
        public SportListViewModel(NavigationStore navigatorStore, SportDataStore sportStore, EmployeeStore trainerStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            _navigatorStore = navigatorStore;
            _sportStore = sportStore;
            _trainerStore = trainerStore;
            LoadSportsCommand = new LoadSportsCommand(this, _sportStore);
            AddSportCommand = new NavaigateCommand<AddSportViewModel>(new NavigationService<AddSportViewModel>(_navigatorStore, () => CreateAddSportViewModel(navigatorStore, this, _sportStore, _trainerStore)));
            sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _sportStore.Loaded += _sportStore_SportLoaded;
            _sportStore.Created += _sportStore_SportAdded;
            _sportStore.Updated += _sportStore_SportUpdated;
            _sportStore.Deleted += _sportStore_SportDeleted;
            _subscriptionDataStore = subscriptionDataStore;
        }
        public SportListItemViewModel? SelectedSport
        {
            get
            {
                return SportList
                    .FirstOrDefault(y => y?.Sport == _sportStore.SelectedSport);
            }
            set
            {
                _sportStore.SelectedSport = value?.Sport;

            }
        }
        private void SearchBox_SearchedText(string? obj)
        {
            sportListItemViewModels.Clear();

            foreach (Sport sport in _sportStore.Sports.Where(x => x.Name!.ToLower().Contains(obj!.ToLower())))
            {
                AddSport(sport);
            }
        }

        private void _sportStore_SportDeleted(int id)
        {
            SportListItemViewModel? itemViewModel = sportListItemViewModels.FirstOrDefault(y => y.Sport?.Id == id);

            if (itemViewModel != null)
            {
                sportListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _sportStore_SportUpdated(Sport sport)
        {
            SportListItemViewModel? sportViewModel =
                  sportListItemViewModels.FirstOrDefault(y => y.Sport.Id == sport.Id);

            if (sportViewModel != null)
            {
                sportViewModel.Update(sport);
            }
        }

        private void _sportStore_SportAdded(Sport sport)
        {
            AddSport(sport);
        }

        private void _sportStore_SportLoaded()
        {
            sportListItemViewModels.Clear();

            foreach (Sport sport in _sportStore.Sports)
            {
                AddSport(sport);
            }
        }

        public override void Dispose()
        {
            _sportStore.Loaded -= _sportStore_SportLoaded;
            _sportStore.Created -= _sportStore_SportAdded;
            _sportStore.Updated -= _sportStore_SportUpdated;
            _sportStore.Deleted -= _sportStore_SportDeleted;
            base.Dispose();
        }





        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport, _sportStore, _navigatorStore, _trainerStore, this,_subscriptionDataStore);
            sportListItemViewModels.Add(itemViewModel);
        }
        public static SportListViewModel LoadViewModel(NavigationStore navigatorStore, SportDataStore sportStore, EmployeeStore employeeStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            SportListViewModel viewModel = new SportListViewModel(navigatorStore, sportStore, employeeStore,subscriptionDataStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }


        private AddSportViewModel CreateAddSportViewModel(NavigationStore navigatorStore, SportListViewModel sportListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            return AddSportViewModel.LoadViewModel(navigatorStore, sportListViewModel, sportDataStore, employeeStore);
        }
    }
}
