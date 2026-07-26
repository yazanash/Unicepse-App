using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uniceps.LicenseManager.Models;

namespace Uniceps.LicenseManager
{
    public class ActivationService
    {
        private readonly HttpClient _httpClient;
        private string PublicKeyXml;

        public ActivationService(string baseUrl, string publicKeyXml)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            PublicKeyXml = publicKeyXml;
        }
        public async Task<string> ActivateFromLicenseFile(string filePath)
        {
            try
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                var licenseData = JsonSerializer.Deserialize<LicenseFileModel>(jsonContent);

                if (licenseData == null || licenseData.Id == Guid.Empty|| !VerifyInitialLicense(licenseData))
                    throw new Exception("ملف الترخيص غير صالح أو تالف.");
              
                var activationRequest = new
                {
                    LicenseId = licenseData.Id,
                    MachineId = HardwareFingerprint.GetId() 
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(activationRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"Licenses/activate", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    string token = doc.RootElement.GetProperty("token").GetString()!;
                    DateTime activatedAt = doc.RootElement.GetProperty("activatedAt").GetDateTime();
                    DateTime? expiryDate = null;
                    if (doc.RootElement.TryGetProperty("expiryDate", out var expProp) && expProp.ValueKind != JsonValueKind.Null)
                    {
                        expiryDate = expProp.GetDateTime();
                    }
                    SaveActivationInfo(licenseData.Id, token, activatedAt.Ticks, expiryDate);

                    return "تم التفعيل بنجاح! يمكنك الآن استخدام البرنامج.";
                }
                else
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    string message = doc.RootElement.TryGetProperty("message", out var msg)
                      ? (msg.GetString() ?? "فشل التفعيل")
                      : "فشل التفعيل";
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                return $"خطأ: {ex.Message}";
            }
        }
        public bool VerifyInitialLicense(LicenseFileModel license)
        {
            try
            {
                using var rsa = RSA.Create();
                rsa.FromXmlString(PublicKeyXml);

                string rawData = $"{license.Id}|{license.CustomerName}|{license.MaxDevices}";
                byte[] dataBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] signatureBytes = Convert.FromBase64String(license.ServerSignature);

                return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch { return false; }
        }
        private void SaveActivationInfo(Guid licenseId, string token, long ticks, DateTime? expiryDate)
        {
            var filePath = GetConfigPath(); 

            var data = new
            {
                Lid = licenseId,
                Key = token,
                Mid = HardwareFingerprint.GetId(),
                Time = ticks,
                Exp = expiryDate?.Ticks
            };

            File.WriteAllText(filePath, JsonSerializer.Serialize(data));
        }
        public bool IsActivationValid()
        {
            try
            {
                var path = GetConfigPath();
                if (!File.Exists(path)) return false;

                var json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                string savedLid = root.GetProperty("Lid").GetGuid().ToString();
                string savedKey = root.GetProperty("Key").GetString()!;
                string savedMid = root.GetProperty("Mid").GetString()!;
                long savedTime = root.GetProperty("Time").GetInt64();
                if (DateTime.Now < new DateTime(savedTime)) return false;
                if (savedMid != HardwareFingerprint.GetId()) return false;

                string rawData = $"{savedLid}|{savedMid}|{savedTime}";

                using var rsa = RSA.Create();
                rsa.FromXmlString(PublicKeyXml);

                byte[] dataBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] signatureBytes = Convert.FromBase64String(savedKey);

                return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch { return false; }
        }
        public LicenseStatus GetCurrentLicenseStatus()
        {
            try
            {
                if (!IsActivationValid())
                    return LicenseStatus.DefaultTrial();

                var path = GetConfigPath();
                var json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                DateTime? expiryDate = null;
                if (root.TryGetProperty("Exp", out var expProp) && expProp.ValueKind != JsonValueKind.Null)
                {
                    expiryDate = new DateTime(expProp.GetInt64());
                }
                if (expiryDate.HasValue && expiryDate.Value < DateTime.Now)
                {
                    return new LicenseStatus
                    {
                        PlanName = "Expired",
                        IsFullVersion = false,
                        ExpiryDate = expiryDate
                    };
                }
                long savedTicks = root.GetProperty("Time").GetInt64();
                DateTime activatedAt = new DateTime(savedTicks);

                return new LicenseStatus
                {
                    PlanName = expiryDate.HasValue ? "Subscription Plan" : "Lifetime Plan",
                    IsFullVersion = true,
                    ExpiryDate = expiryDate,
                    MachineId = root.GetProperty("Mid").GetString()!
                };
            }
            catch
            {
                return LicenseStatus.DefaultTrial();
            }
        }
        private string GetConfigPath()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Uniceps");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return Path.Combine(folder, "activation.conf");
        }
    }
}
