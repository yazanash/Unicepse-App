using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Models;
using Unicepse.Entityframework.Services;

namespace Unicepse.Stores
{
    public class LicenseDataStore
    {
        public event Action<License>? Created;
        //public event Action? Loaded;
        //public event Action<License>? Updated;
        //public event Action<int>? Deleted;



        private License? _currentLicense;
        public License? CurrentLicense
        {
            get
            {
                return _currentLicense;
            }
            set
            {
                _currentLicense = value;
                StateChanged?.Invoke();
            }
        }


        private GymProfile? _currentGymProfile;
        public GymProfile? CurrentGymProfile
        {
            get
            {
                return _currentGymProfile;
            }
            set
            {
                _currentGymProfile = value;
                GymChanged?.Invoke();
            }
        }

        public event Action? StateChanged;
        public event Action? GymChanged;

        private readonly LicenseApiDataService _licenseApiDataService;
        private readonly LicenseDataService _licenseDataService;
        private readonly GymProfileDataService _gymProfileDataService;
        public LicenseDataStore(LicenseApiDataService licenseApiDataService, LicenseDataService licenseDataService, GymProfileDataService gymProfileDataService)
        {
            _licenseApiDataService = licenseApiDataService;
            _licenseDataService = licenseDataService;
            _gymProfileDataService = gymProfileDataService;
        }
        public async void ActiveLicense()
        {
            _currentLicense = _licenseDataService.ActiveLicenses();
            _currentGymProfile = await _gymProfileDataService.Get();
        }
        public async Task GetGymProfile(License license)
        {
            GymProfile gymProfile = await _licenseApiDataService.GetGymProfile(license.GymId!);
            GymProfile? ExistGymProfile = await _gymProfileDataService.GetByGymID(gymProfile.GymId!);
            if (ExistGymProfile != null)
                _currentGymProfile = await _gymProfileDataService.Update(gymProfile);
            else
                _currentGymProfile = await _gymProfileDataService.Create(gymProfile);
        }
        public async Task VerifyLicense(string ProductKey)
        {
            License license = await _licenseApiDataService.GetLicense(ProductKey);
            if (license != null)
            {
                License created_license = await _licenseDataService.Create(license);
                await GetGymProfile(created_license);

                CurrentLicense = created_license;
                Created?.Invoke(created_license);
            }

        }
        public async Task CheckLicenseValidation()
        {
            bool verificaion = await _licenseApiDataService.VerifyLicense();
            if (verificaion && _currentLicense != null)
                await GetGymProfile(_currentLicense);
            if (!verificaion)
            {
                if (CurrentLicense != null)
                {
                    await _licenseDataService.Delete(CurrentLicense.Id);

                    CurrentLicense = null;
                }
            }
        }

    }
}
