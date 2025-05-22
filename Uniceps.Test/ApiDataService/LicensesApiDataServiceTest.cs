using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API;
using Uniceps.API.Services;
using Uniceps.Core.Models;
using Uniceps.API.Models;

namespace Uniceps.Test.ApiDataService
{
    [TestFixture]
    public class LicensesApiDataServiceTest
    {
        LicenseApiDataService? licenseDataService;
        UnicepseApiPrepHttpClient? _client;
        [OneTimeSetUp]
        public void OnetimeSetUp()
        {
            HttpClient _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://127.0.0.1:5000/api/v1/")

            };
            _client = new UnicepseApiPrepHttpClient(_httpClient, new UnicepsePrepAPIKey(""));
            licenseDataService = new(_client);
        }

        [OneTimeTearDown]
        public void Onetimetear()
        {

        }

        [SetUp]
        public void SetUp()
        {

        }
        [TearDown]
        public void TearDown()
        {

        }


        [Test]
        // it should create metric and return 201(created) status code 
        public async Task GetLicensesWithGymProfile()
        {
            string? licenseKey = "K1HO-QPUF-QUHW-J5TT";
            //Act
            License actual_license = await licenseDataService!.GetLicense(licenseKey);
            //Assert
            Assert.NotNull(actual_license);

            GymProfile gym = await licenseDataService!.GetGymProfile(actual_license.GymId!);
            Assert.AreEqual(gym.GymId, actual_license.GymId);
        }


    }
}
