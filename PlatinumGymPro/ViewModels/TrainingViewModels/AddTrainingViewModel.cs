using PlatinumGymPro.Commands.TrainingCommands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.TrainingViewModels
{
    public class AddTrainingViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly SportStore _sportStore;
        private readonly ObservableCollection<SportSelectListItemViewModel> _sportListItemViewModels;
       
        public IEnumerable<SportSelectListItemViewModel> SportList => _sportListItemViewModels;

        public SportSelectListItemViewModel? SelectedSportListingItemViewModel
        {
            get
            {
                return _sportListItemViewModels
                    .FirstOrDefault(y => y.Sport?.Id == _sportStore.SelectedSport?.Id);
            }
            set
            {
                _sportStore.SelectedSport = value?.Sport;
              
            }
        }
        public AddTrainingViewModel(NavigationStore navigationStore, SportStore sportStore, TrainerStore trainerStore)
        {
            _navigationStore = navigationStore;
            _sportStore = sportStore;
            _sportListItemViewModels = new ObservableCollection<SportSelectListItemViewModel>();
            _sportStore.SportLoaded += _sportStore_SportLoaded;
            _sportStore.SelectedSportChanged += _sportStore_SelectedSportChanged;
            
            LoadSportsCommand = new LoadSportsForTrainingCommand(_sportStore,this);
           
        }

        private void _sportStore_SelectedSportChanged()
        {
            OnPropertyChanged(nameof(SelectedSportListingItemViewModel));
        }

        

        private void _sportStore_SportLoaded()
        {
            _sportListItemViewModels.Clear();

            foreach (Sport sport in _sportStore.Sports)
            {
                AddSport(sport);
            }
        }
       
        private void AddSport(Sport sport)
        {
            SportSelectListItemViewModel itemViewModel =
                new SportSelectListItemViewModel(sport);
            _sportListItemViewModels.Add(itemViewModel);
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        public ICommand? LoadSportsCommand { get; }

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


        public static AddTrainingViewModel LoadViewModel(SportStore sportStore, NavigationStore navigatorStore, PlayerListViewModel playerListingViewModel, TrainerStore trainerStore)
        {
            AddTrainingViewModel viewModel = new AddTrainingViewModel(navigatorStore, sportStore, trainerStore);

            viewModel.LoadSportsCommand!.Execute(null);

            return viewModel;
        }


    }
}
