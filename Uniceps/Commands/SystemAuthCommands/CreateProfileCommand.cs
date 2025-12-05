using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.navigation;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Commands.SystemAuthCommands
{
    public class CreateProfileCommand : AsyncCommandBase
    {
        private readonly IProfileDataStore _profileDataStore;
        private readonly SystemProfileCreationViewModel _systemProfileCreationViewModel;
        public CreateProfileCommand(IProfileDataStore profileDataStore, SystemProfileCreationViewModel systemProfileCreationViewModel)
        {
            _profileDataStore = profileDataStore;
            _systemProfileCreationViewModel = systemProfileCreationViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                SystemProfile systemProfile = new SystemProfile();
                systemProfile.DisplayName = _systemProfileCreationViewModel.DisplayName!;
                systemProfile.OwnerName = _systemProfileCreationViewModel.OwnerName!;
                systemProfile.Address = _systemProfileCreationViewModel.Address!;
                systemProfile.PhoneNumber = _systemProfileCreationViewModel.PhoneNumber!;
                systemProfile.BirthDate = _systemProfileCreationViewModel.BirthDate!;
                systemProfile.Gender = Core.Common.GenderType.Male;
                await _profileDataStore.CreateOrUpdate(systemProfile);
                _systemProfileCreationViewModel.OnProfileCreatedAction();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         
        }
    }
}
