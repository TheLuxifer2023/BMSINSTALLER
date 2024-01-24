using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using JetBrains.Annotations;
using PH.PicoCrypt2;
using static IronPython.Runtime.Profiler;

namespace ConsoleApp1.Client.Download
{
    internal class DownloadFilesGoogleDrive
    {

        public void servapi()
        {
            TcpClient client = new TcpClient();

            client.Connect("127.0.0.1", 13000);

            NetworkStream stream = client.GetStream();

            Byte[] bytes = new Byte[256];
            int i;

            string data = string.Empty;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data += System.Text.Encoding.UTF8.GetString(bytes, 0, i);
            }

            client.Close();

            Console.WriteLine("gey");
        }

        #region Download Drive Files
        public static async Task<string> GetFilesAsync(string fileId, string _saveTo)
        {

            // Создайте экземпляр ResourceManager для доступа к ресурсам проекта
            ResourceManager resourceManager = new ResourceManager("ConsoleApp1.Properties.Resources", Assembly.GetExecutingAssembly());

            // Получите файл ресурса (например, исполняемый файл) как массив байтов
            byte[] fileBytes = (byte[])resourceManager.GetObject("ServerApiGet");

            // Сохраните массив байтов в файл на диске
            File.WriteAllBytes("ServerApiGet.exe", fileBytes);

            // Запустите приложение
            Process.Start("ServerApiGet.exe");

            TcpClient client = new TcpClient();

            client.Connect("127.0.0.1", 13000);

            NetworkStream stream = client.GetStream();

            Byte[] bytes = new Byte[256];
            int i;

            string data = string.Empty;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data += System.Text.Encoding.UTF8.GetString(bytes, 0, i);
            }

            client.Close();



            try
            {

                string apiKey = data;

                var _service = new DriveService(new BaseClientService.Initializer()
                {
                    ApiKey = apiKey,
                    ApplicationName = "GoogleDriveApiDownload",
                });
                FilesResource.GetRequest request = _service.Files.Get(fileId);
                string fileName = request.Execute().Name;
                string FilePath = Path.Combine(_saveTo, fileName);

                await DownloadFileAsync(request, FilePath);

                return fileId;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static async Task DownloadFileAsync(FilesResource.GetRequest request, string FilePath)
        {
            FileStream stream2 = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);

            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.NotStarted:
                        break;
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        break;
                    case Google.Apis.Download.DownloadStatus.Completed:
                        stream2.Flush();
                        stream2.Close();

                        break;
                    case Google.Apis.Download.DownloadStatus.Failed:
                        break;
                    default:
                        break;
                }
            };
            await request.DownloadAsync(stream2);
            stream2.Close();

            string processName = "ServerApiGet.exe";
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
        #endregion
    }
}

