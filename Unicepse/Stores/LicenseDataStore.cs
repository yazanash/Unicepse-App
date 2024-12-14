using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.API.Services;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using Unicepse.Core.Services;
using Unicepse.Entityframework.Services;

namespace Unicepse.Stores
{
    public class LicenseDataStore
    {
        public event Action<License>? Created;
        public event Action? Loaded;
        string LogFlag = "[Licenses] ";
        private readonly ILogger<LicenseDataStore> _logger;
        private readonly Lazy<Task> _initializeLazy;



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
                _logger.LogInformation(LogFlag + "selected license changed");
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
                _logger.LogInformation(LogFlag + "selected gym profile changed");
                _currentGymProfile = value;
                GymChanged?.Invoke();
            }
        }

        public event Action? StateChanged;
        public event Action? GymChanged;
        private readonly List<License> _licenses;

        public IEnumerable<License> Licenses => _licenses;
        private readonly LicenseApiDataService _licenseApiDataService;
        private readonly IDataService<License> _licenseDataService;
        private readonly IDataService<GymProfile> _gymProfileDataService;

        private readonly IGetSingleLatestService<License> _licenseGetSingleLatestService;
        private readonly IGetSingleLatestService<GymProfile> _gymProfileGetSingleLatestService;

        private readonly IPublicIdService<License> _licensePublicIdService;
        private readonly IPublicIdService<GymProfile> _gymProfilePublicIdService;
        public LicenseDataStore(LicenseApiDataService licenseApiDataService, IDataService<License> licenseDataService, IDataService<GymProfile> gymProfileDataService, ILogger<LicenseDataStore> logger, IGetSingleLatestService<License> licenseGetSingleLatestService, IGetSingleLatestService<GymProfile> gymProfileGetSingleLatestService, IPublicIdService<License> licensePublicIdService, IPublicIdService<GymProfile> gymProfilePublicIdService)
        {
            _licenseGetSingleLatestService = licenseGetSingleLatestService;
            _gymProfileGetSingleLatestService = gymProfileGetSingleLatestService;
            _licensePublicIdService = licensePublicIdService;
            _gymProfilePublicIdService = gymProfilePublicIdService;
            _licenseApiDataService = licenseApiDataService;
            _licenseDataService = licenseDataService;
            _gymProfileDataService = gymProfileDataService;
            _licenses = new List<License>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
          
        }


        public async Task GetLicenses()
        {
            await _initializeLazy.Value;
            Loaded?.Invoke();
        }
        private async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "init license");
            IEnumerable<License> licenses = await _licenseDataService.GetAll();
            _licenses.Clear();
            _licenses.AddRange(licenses);
        }
        public void ActiveLicense()
        {
            _logger.LogInformation(LogFlag + "activated license");
            _currentLicense =  _licenseGetSingleLatestService.Get();
            _currentGymProfile =  _gymProfileGetSingleLatestService.Get();
        }
        public async Task GetGymProfile(License license)
        {
            _logger.LogInformation(LogFlag + "get gym profile");
            GymProfile gymProfile = await _licenseApiDataService.GetGymProfile(license.GymId!);
            _logger.LogInformation(LogFlag + "check gym profile if exists");
            GymProfile? ExistGymProfile = await _gymProfilePublicIdService.GetByUID(gymProfile.GymId!);
            if (ExistGymProfile != null)
            {
                _logger.LogInformation(LogFlag + "update gym profile");
                _currentGymProfile = await _gymProfileDataService.Update(gymProfile);
            }

            else
            {
                _logger.LogInformation(LogFlag + "create gym profile");
                _currentGymProfile = await _gymProfileDataService.Create(gymProfile);
            }
               
        }
        public async Task VerifyLicense(string ProductKey)
        {
            _logger.LogInformation(LogFlag + "verify licenses started");
            License license = await _licenseApiDataService.GetLicense(ProductKey);
            if (license != null)
            {
                _logger.LogInformation(LogFlag + "get existed licenses");
                IEnumerable<GymProfile> ExistGyms = await _gymProfileDataService.GetAll();
                if (ExistGyms.Count() > 0 && !ExistGyms.Where(x => x.GymId == license.GymId).Any())
                {
                    _logger.LogInformation(LogFlag + "rejected license");
                    throw new Exception("لا يتطابق هذا الترخيص من هذا النادي");
                }
                License created_license;
                License? exist_license = await _licensePublicIdService.GetByUID(license.LicenseId!);
                if (exist_license != null)
                {
                    _logger.LogInformation(LogFlag + "update license info");
                    created_license = await _licenseDataService.Update(license);

                }
                else
                {
                    _logger.LogInformation(LogFlag + "store license info");
                    created_license = await _licenseDataService.Create(license);

                }
                await GetGymProfile(created_license);
                _logger.LogInformation(LogFlag + "set current license");
                CurrentLicense = created_license;
                Created?.Invoke(created_license);
            }

        }
        public async Task CheckLicenseValidation()
        {
            _logger.LogInformation(LogFlag + "check license validation");
            if (_currentLicense != null)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "get license from api");
                    License license = await _licenseApiDataService.GetLicenseById(_currentLicense.LicenseId!);
                    if (license != null)
                    {
                        _logger.LogInformation(LogFlag + "get license from local");
                        License? exist_license = await _licensePublicIdService.GetByUID(_currentLicense.LicenseId!);
                        if (exist_license != null)
                        {
                            _logger.LogInformation(LogFlag + "update license info");
                            _currentLicense = await _licenseDataService.Update(license);

                        }

                    }
                    _logger.LogInformation(LogFlag + "verify license from api");
                    int verificaion = await _licenseApiDataService.VerifyLicense();
                    if (verificaion == 202 && _currentLicense != null)
                    {
                        _logger.LogInformation(LogFlag + "license validation status : {0}",verificaion);
                        await GetGymProfile(_currentLicense);
                    }
                       
                }
                catch (NotExistException ex)
                {
                    _logger.LogError(LogFlag + "license validation erre: {0}", ex.Message);
                    MessageBox.Show("خطا في الترخيص يرجى التواصل مع فريق الدعم التقني");
                    await _licenseDataService.Delete(_currentLicense.Id);
                    _currentLicense = null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "license validation erre: {0}", ex.Message);
                }
            }
        }

    }
}
