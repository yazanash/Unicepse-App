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
    public class LicenseDataStore : IDataStore<License>
    {
        public event Action<License>? Created;
        public event Action? Loaded;
        public event Action<License>? Updated;
        public event Action<int>? Deleted;



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

        public event Action? StateChanged;


        private readonly LicenseApiDataService _licenseApiDataService;
        private readonly LicenseDataService _licenseDataService;
        public LicenseDataStore(LicenseApiDataService licenseApiDataService, LicenseDataService licenseDataService)
        {
            _licenseApiDataService = licenseApiDataService;
            _licenseDataService = licenseDataService;
            _currentLicense = _licenseDataService.ActiveLicenses();
        }

        public Task Add(License entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int entity_id)
        {
            throw new NotImplementedException();
        }

        public Task GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task Update(License entity)
        {
            throw new NotImplementedException();
        }

        public async Task VerifyLicense(string ProductKey)
        {
            License license = await _licenseApiDataService.GetLicense(ProductKey);
            License created_license = await _licenseDataService.Create(license);
            CurrentLicense = created_license;
            Created?.Invoke(created_license);
        }
    }
}
