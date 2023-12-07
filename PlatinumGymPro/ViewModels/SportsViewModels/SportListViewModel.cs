using PlatinumGym.Core.Models.Sport;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.SportsCommands;
//using PlatinumGymPro.Models;
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
        private SportDataStore _sportStore;
        //private TrainerStore _trinerStore;
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
        public SportListViewModel(NavigationStore navigatorStore, SportDataStore sportStore)
        {
            _navigatorStore = navigatorStore;
            _sportStore = sportStore;
            //_trinerStore = trinerStore;
            LoadSportsCommand = new LoadSportsCommand( this,_sportStore);
            AddSportCommand = new NavaigateCommand<AddSportViewModel>(new NavigationService<AddSportViewModel>(_navigatorStore, () => CreateAddSportViewModel(navigatorStore, this)));
            sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();



            _sportStore.Loaded += _sportStore_SportLoaded;
            _sportStore.Created += _sportStore_SportAdded;
            _sportStore.Updated += _sportStore_SportUpdated;
            _sportStore.Deleted += _sportStore_SportDeleted;

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
            _sportStore.Loaded -= _sportStore_SportLoaded;
            _sportStore.Created -= _sportStore_SportAdded;
            _sportStore.Updated -= _sportStore_SportUpdated;
            _sportStore.Deleted -= _sportStore_SportDeleted;
            base.Dispose();
        }
     
      

      
      
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport, _sportStore, _navigatorStore);
            sportListItemViewModels.Add(itemViewModel);
        }
        public static SportListViewModel LoadViewModel(NavigationStore navigatorStore, SportDataStore sportStore)
        {
            SportListViewModel viewModel = new SportListViewModel(navigatorStore , sportStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }


        private AddSportViewModel CreateAddSportViewModel(NavigationStore navigatorStore,SportListViewModel sportListViewModel)
        {
            return AddSportViewModel.LoadViewModel(navigatorStore, sportListViewModel);
        }
    }
}
