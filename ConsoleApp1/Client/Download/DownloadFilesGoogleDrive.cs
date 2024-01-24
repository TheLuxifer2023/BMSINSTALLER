using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.XAMPP;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace ConsoleApp1.Client.Download
{
    internal class DownloadFilesGoogleDrive
    {
        public void servapi()
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 13000);
            NetworkStream stream = client.GetStream();
            byte[] bytes = new byte[256];
            string data = string.Empty;
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data += Encoding.UTF8.GetString(bytes, 0, i);
            }
            client.Close();
        }

        public static async Task<string> GetFilesAsync(string fileId, string _saveTo)
        {
            string appdata = ConsoleApp1.XAMPP.SteamCMD.Direct.bmsInstallerPath;
            ResourceManager resourceManager = new ResourceManager("ConsoleApp1.Properties.Resources", Assembly.GetExecutingAssembly());
            File.WriteAllBytes(bytes: (byte[])resourceManager.GetObject("ServerApiGet"), path: appdata + "\\ServerApiGet.exe");
            Process.Start(appdata + "\\ServerApiGet.exe");
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 13000);
            NetworkStream stream = client.GetStream();
            byte[] bytes = new byte[256];
            string data = string.Empty;
            while (true)
            {
                int num;
                int i = (num = stream.Read(bytes, 0, bytes.Length));
                if (num == 0)
                {
                    break;
                }
                data += Encoding.UTF8.GetString(bytes, 0, i);
            }
            client.Close();
            try
            {
                string apiKey = data;
                DriveService _service = new DriveService(new BaseClientService.Initializer
                {
                    ApiKey = apiKey,
                    ApplicationName = "GoogleDriveApiDownload"
                });
                FilesResource.GetRequest request = _service.Files.Get(fileId);
                string fileName = request.Execute().Name;
                string FilePath = Path.Combine(_saveTo, fileName);
                await DownloadFileAsync(request, FilePath);
                return fileId;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static async Task DownloadFileAsync(FilesResource.GetRequest request, string FilePath)
        {
            FileStream stream2 = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            request.MediaDownloader.ProgressChanged += delegate (IDownloadProgress progress)
            {
                switch (progress.Status)
                {
                    case DownloadStatus.NotStarted:
                        break;
                    case DownloadStatus.Downloading:
                        break;
                    case DownloadStatus.Completed:
                        stream2.Flush();
                        stream2.Close();
                        break;
                    case DownloadStatus.Failed:
                        break;
                }
            };
            await request.DownloadAsync(stream2);
            stream2.Close();
        }
    }
}
