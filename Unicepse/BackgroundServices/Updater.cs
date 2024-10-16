using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Unicepse.BackgroundServices
{
    class Updater
    {
        private static readonly string updateXmlUrl = "https://trio-verse.com/app/desktop/updates";
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        public static async Task<bool> CheckForUpdate()
        {

            using (var client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync(updateXmlUrl);
                if(res.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
                var xmlContent = await client.GetStringAsync(updateXmlUrl);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                var latestVersion = xmlDoc.SelectSingleNode("//Update/Version")!.InnerText;
                return !string.Equals(currentVersion, latestVersion);
            }
        }

        public static async Task<string> DownloadUpdate(ProgressWindow progressWindow)
        {
            using (var client = new HttpClient())
            {
                var xmlContent = await client.GetStringAsync(updateXmlUrl);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                var updateUrl = xmlDoc.SelectSingleNode("//Update/Url")!.InnerText;
                var fileName = xmlDoc.SelectSingleNode("//Update/Version")!.InnerText;

                var response = await client.GetAsync(updateUrl, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? 1L;
                var downloadedBytes = 0L;

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var buffer = new byte[8192];
                    int bytesRead;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        downloadedBytes += bytesRead;
                        var progress = (int)((double)downloadedBytes / totalBytes * 100);
                        progressWindow.Dispatcher.Invoke(() => progressWindow.UpdateProgress(progress));
                    }
                }

                return fileName;
            }
        }
    }
}
