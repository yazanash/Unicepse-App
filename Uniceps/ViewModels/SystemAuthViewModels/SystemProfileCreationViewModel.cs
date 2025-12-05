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

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class SystemProfileCreationViewModel : ViewModelBase
    {
        private readonly IProfileDataStore _profileStore;
        public event Action? ProfileCreatedAction;
        public SystemProfileCreationViewModel(IProfileDataStore profileStore)
        {
            _profileStore = profileStore;
            CreateProfileCommand = new CreateProfileCommand(_profileStore, this);
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
        private string? _ownerName;
        public string? OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; OnPropertyChanged(nameof(OwnerName)); }
        }
        private string? _address;
        public string? Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }
        public GenderType Gender { get; set; }

    }

}
