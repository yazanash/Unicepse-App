using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Core.Common;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class SystemProfileCreationViewModel : ViewModelBase
    {
        private readonly IProfileDataStore _profileStore;
        public event Action? ProfileCreatedAction;
        private readonly HomeViewModel _homeViewModel;
        private readonly NavigationStore _navigationStore;
        public SystemProfileCreationViewModel(IProfileDataStore profileStore, HomeViewModel homeViewModel, NavigationStore navigationStore)
        {
            _profileStore = profileStore;
            _navigationStore = navigationStore;
            _homeViewModel = homeViewModel;

            NavigationService<HomeViewModel> navigationService = new NavigationService<HomeViewModel>(_navigationStore, () => _homeViewModel);
            CreateProfileCommand = new CreateProfileCommand(_profileStore, this, navigationService);
        }

        public void OnProfileCreatedAction()
        {
            ProfileCreatedAction?.Invoke();
        }
        public ICommand CreateProfileCommand { get; set; }
        private string? _displayName;
        public string? DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged(nameof(DisplayName)); }
        }
        private DateTime _birthDate =DateTime.Now;
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); }
        }
        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }
        public GenderType Gender { get; set; }

    }

}
