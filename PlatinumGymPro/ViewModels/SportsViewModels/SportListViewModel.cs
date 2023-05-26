using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.SportsCommands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SportsViewModels
{
    public class SportListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SportListItemViewModel> sportListItemViewModels;
       
        private NavigationStore _navigatorStore;
        private SportStore _sportStore;
        private TrainerStore _trinerStore;
        public IEnumerable<SportListItemViewModel> SportList => sportListItemViewModels;
       
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
        public SportListViewModel(NavigationStore navigatorStore, SportStore playerStore,TrainerStore trinerStore)
        {
            _navigatorStore = navigatorStore;
            _sportStore = playerStore;
            _trinerStore = trinerStore;
            LoadSportsCommand = new LoadSportsCommand(_sportStore, this);
            AddSportCommand = new NavaigateCommand<AddSportViewModel>(new NavigationService<AddSportViewModel>(_navigatorStore, () => CreateAddSportViewModel(navigatorStore, _sportStore, this, _trinerStore)));
            sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();

          

            _sportStore.SportLoaded += _sportStore_SportLoaded;
            _sportStore.SportAdded += _sportStore_SportAdded;
            _sportStore.SportUpdated += _sportStore_SportUpdated;
            _sportStore.SportDeleted += _sportStore_SportDeleted;

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

        protected override void Dispose()
        {
            _sportStore.SportLoaded -= _sportStore_SportLoaded;
            _sportStore.SportAdded -= _sportStore_SportAdded;
            _sportStore.SportUpdated -= _sportStore_SportUpdated;
            _sportStore.SportDeleted -= _sportStore_SportDeleted;
            base.Dispose();
        }
     
      

      
      
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport, _sportStore, _navigatorStore);
            sportListItemViewModels.Add(itemViewModel);
        }
        public static SportListViewModel LoadViewModel(SportStore sportStore, NavigationStore navigatorStore,TrainerStore trainerStore)
        {
            SportListViewModel viewModel = new SportListViewModel(navigatorStore, sportStore, trainerStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }


        private AddSportViewModel CreateAddSportViewModel(NavigationStore navigatorStore, SportStore _sportStore,SportListViewModel sportListViewModel, TrainerStore trainerStore)
        {
            return AddSportViewModel.LoadViewModel(_sportStore, navigatorStore, sportListViewModel, trainerStore);
        }
    }
}
