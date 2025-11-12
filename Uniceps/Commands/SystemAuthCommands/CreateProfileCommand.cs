using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.navigation;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Commands.SystemAuthCommands
{
    public class CreateProfileCommand : AsyncCommandBase
    {
        private readonly IProfileDataStore _profileDataStore;
        private readonly SystemProfileCreationViewModel _systemProfileCreationViewModel;
        private readonly NavigationService<HomeViewModel> _navigationService;
        public CreateProfileCommand(IProfileDataStore profileDataStore, SystemProfileCreationViewModel systemProfileCreationViewModel, NavigationService<HomeViewModel> navigationService)
        {
            _profileDataStore = profileDataStore;
            _systemProfileCreationViewModel = systemProfileCreationViewModel;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            SystemProfile systemProfile = new SystemProfile();
            systemProfile.DisplayName = _systemProfileCreationViewModel.DisplayName!;
            systemProfile.PhoneNumber = _systemProfileCreationViewModel.PhoneNumber!;
            systemProfile.BirthDate = _systemProfileCreationViewModel.BirthDate!;
            systemProfile.Gender = Core.Common.GenderType.Male;
            await _profileDataStore.Add(systemProfile);
            _systemProfileCreationViewModel.OnProfileCreatedAction();
            _navigationService.ReNavigate();
        }
    }
}
