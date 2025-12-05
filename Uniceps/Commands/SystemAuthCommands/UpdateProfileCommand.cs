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
    public class UpdateProfileCommand : AsyncCommandBase
    {
        private readonly IProfileDataStore _profileDataStore;
        private readonly AppInfoViewModel _systemProfileCreationViewModel;
        public UpdateProfileCommand(IProfileDataStore profileDataStore, AppInfoViewModel systemProfileCreationViewModel)
        {
            _profileDataStore = profileDataStore;
            _systemProfileCreationViewModel = systemProfileCreationViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                SystemProfile systemProfile = new SystemProfile();
                systemProfile.DisplayName = _systemProfileCreationViewModel.Name!;
                systemProfile.OwnerName = _systemProfileCreationViewModel.OwnerName!;
                systemProfile.Address = _systemProfileCreationViewModel.Address!;
                systemProfile.PhoneNumber = _systemProfileCreationViewModel.Phone!;
                systemProfile.Gender = Core.Common.GenderType.Male;
                await _profileDataStore.CreateOrUpdate(systemProfile);
                MessageBox.Show("تم تحديث المعلومات بنجاح");
                _systemProfileCreationViewModel.OnProfileUpdated();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
