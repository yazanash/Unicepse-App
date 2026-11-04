using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Sport;
using Uniceps.Core.Models.Sport;
using Uniceps.navigation.DialogService;
using Uniceps.Stores;
using Uniceps.Views.SportViews;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class SportListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SportListItemViewModel> sportListItemViewModels;
        private SportDataStore _sportStore;
        private readonly IDialogService _dialogService;
        public IEnumerable<SportListItemViewModel> SportList => sportListItemViewModels;
        public SearchBoxViewModel SearchBox { get; set; }
        public bool HasData => sportListItemViewModels.Count > 0;
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
        public ICommand OpenSubscriptionReport => new RelayCommand(()=> _dialogService.ShowDialog<SportSubscriptionsWindowView, SportSubscriptionsViewModel>());
        public SportListViewModel(SportDataStore sportStore, IDialogService dialogService)
        {
            _sportStore = sportStore;
            _dialogService = dialogService;

            LoadSportsCommand = new LoadSportsCommand(this, _sportStore);
            AddSportCommand = new RelayCommand(() => _dialogService.ShowDialog<SportDetailWindowView, AddSportViewModel>());
            sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _sportStore.Loaded += _sportStore_SportLoaded;
            _sportStore.Created += _sportStore_SportAdded;
            _sportStore.Updated += _sportStore_SportUpdated;
            _sportStore.Deleted += _sportStore_SportDeleted;
            LoadSportsCommand.Execute(null);
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
            OnPropertyChanged(nameof(HasData));
        }
        private void _sportStore_SportUpdated(Sport sport)
        {
            SportListItemViewModel? sportViewModel =
                  sportListItemViewModels.FirstOrDefault(y => y.Sport.Id == sport.Id);

            if (sportViewModel != null)
            {
                sportViewModel.Update(sport);
            }
            OnPropertyChanged(nameof(HasData));
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
                new SportListItemViewModel(sport, _sportStore,_dialogService);
            sportListItemViewModels.Add(itemViewModel);
            OnPropertyChanged(nameof(HasData));
        }
    }
}
