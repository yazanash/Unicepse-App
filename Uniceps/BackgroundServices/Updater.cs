
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Uniceps.Services;
using Uniceps.ViewModels;
using Uniceps.Views;
using Windows.Media.Protection.PlayReady;

namespace Uniceps.BackgroundServices
{
    public class ReleaseDto
    {
        public int Id { get; set; }
        public string Version { get; set; } = "";
        public int TargetOS { get; set; } 
        public string DownloadUrl { get; set; } = "";
        public string ChangeLog { get; set; } = "";
        public string ChangeLogAr { get; set; } = "";
    }
    public static class Updater
    {
        private const string UpdateUrl = "Release/app/1/latest/1";
        public static async Task RunUpdater()
        {
            try
            {
                var update = await Updater.CheckForUpdate();
                if (update != null)
                {
                    ProgressWindow progressWindow = new ProgressWindow();
                    progressWindow.DataContext = new UpdateWindowViewModel(update);
                    progressWindow.Topmost = true;
                    progressWindow.ShowDialog();
                }
            }
            catch { }

        }
        public static async Task<ReleaseDto?> CheckForUpdate()
        {
            try
            {
                string? apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                using var client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl!);
                client.DefaultRequestHeaders.Add("User-Agent", "Uniceps-WPF-Client");

                // استخدام خيارات تجاهل حالة الأحرف
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                HttpResponseMessage response = await client.GetAsync(UpdateUrl);

                if (response.IsSuccessStatusCode)
                {
                    var windowsUpdate = await response.Content.ReadFromJsonAsync<ReleaseDto>(options);

                    if (windowsUpdate != null && !string.IsNullOrEmpty(windowsUpdate.Version))
                    {
                        string currentRawVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

                        Version current = new Version(currentRawVersion);
                        Version latest = new Version(windowsUpdate.Version);

                        if (latest > current)
                        {
                            return windowsUpdate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update check failed: {ex.Message}");
            }

            return null;
        }
        public static void OpenDownloadPage(int releaseId)
        {
            try
            {
                string? apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
                string trackingUrl =  $"{apiUrl}Release/download/{releaseId}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = trackingUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not open download link: {ex.Message}");
            }
        }
    }
}

