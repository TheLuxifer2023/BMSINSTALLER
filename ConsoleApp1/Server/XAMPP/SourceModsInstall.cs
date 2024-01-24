using SevenZipExtractor;
using Spectre.Console;
using System;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;


namespace ConsoleApp1.XAMPP
{
    internal class SourceModsInstall
    {
        public void srcmdinstall()
        {

            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory;
            string cldir = XAMPP.SteamCMD.Direct.SteamPatch; //Steam Floder
            string steamdiskc = "C:\\Program Files (x86)\\Steam";
            string extractsrc = $"{synpath}\\steamapps\\common\\Synergy\\synergy";

            if (File.Exists($"{synpath}\\synergy\\addons\\metamod\\sourcemod.vdf"))
            {
                Console.WriteLine("SourceMod Уже установлен!");
                XAMPP.PluginsInstall pl1 = new PluginsInstall();
                Console.Clear();
                pl1.plinstall();
            }

            if (!Directory.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\addons"))
            {
                Directory.CreateDirectory($"{synpath}\\steamapps\\common\\Synergy\\synergy\\addons");

            }
            AnsiConsole.MarkupLine("[blue1]Это позволит напрямую загрузить необходимые файлы SourceMod, а затем извлечь их....[/]");
            AnsiConsole.MarkupLine("[blue1]Это может занять несколько минут в зависимости от скорости загрузки.[/]");
            AnsiConsole.MarkupLine("");

            string address1 = "https://sm.alliedmods.net/smdrop/1.10/sourcemod-1.10.0-git6443-windows.zip";

            string address2 = "https://mms.alliedmods.net/mmsdrop/1.10/mmsource-1.10.7-git959-windows.zip";

            string address3 = "https://users.alliedmods.net/~kyles/builds/SteamWorks/SteamWorks-git121-windows.zip";

            string patch1 = $"{extractsrc}\\sourcemod-1.10.0-git6443-windows.zip";

            string patch2 = $"{extractsrc}\\mmsource-1.10.7-git959-windows.zip";

            string patch3 = $"{extractsrc}\\SteamWorks-git121-windows.zip";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                webClient.DownloadFile(new Uri(address1), patch1);
                webClient.DownloadFile(new Uri(address2), patch2);
                webClient.DownloadFile(new Uri(address3), patch3);
            }

            if (!File.Exists($"{extractsrc}\\sourcemod-1.10.0-git6443-windows.zip"))
            {
                Console.WriteLine("Не удалось загрузить SourceMod. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{extractsrc}\\mmsource-1.10.7-git959-windows.zip"))
            {
                Console.WriteLine("Не удалось загрузить MetaMod. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{extractsrc}\\SteamWorks-git121-windows.zip"))
            {
                Console.WriteLine("Не удалось загрузит SteamWorks. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            AnsiConsole.MarkupLine("[aqua]Распакова файлов...[/]");
            AnsiConsole.MarkupLine("");

            using (ArchiveFile archiveFile = new ArchiveFile($@"{extractsrc}\sourcemod-1.10.0-git6443-windows.zip"))
            {
                archiveFile.Extract(extractsrc); // extract all sourcemod
            };

            using (ArchiveFile archiveFile = new ArchiveFile($@"{extractsrc}\mmsource-1.10.7-git959-windows.zip"))
            {
                archiveFile.Extract(extractsrc); // extract all mmsource
            };

            using (ArchiveFile archiveFile = new ArchiveFile($@"{extractsrc}\SteamWorks-git121-windows.zip"))
            {
                archiveFile.Extract(extractsrc); // extract all SteamWorks
            };

            Thread.Sleep(5000);

            AnsiConsole.MarkupLine("[grey]Удаление временных файлов...[/]");
            AnsiConsole.MarkupLine("");
            File.Delete($"{extractsrc}\\sourcemod-1.10.0-git6443-windows.zip");
            File.Delete($"{extractsrc}\\mmsource-1.10.7-git959-windows.zip");
            File.Delete($"{extractsrc}\\SteamWorks-git121-windows.zip");

            Directory.CreateDirectory($"{extractsrc}\\addons\\sourcemod\\gamedata\\sdkhooks.games\\custom");
            Directory.CreateDirectory($"{extractsrc}\\addons\\sourcemod\\gamedata\\sdktools.games\\custom");

            if (!Directory.Exists($"{synpath}\\steamapps\\sourcemods"))
            {
            

                AnsiConsole.MarkupLine("[deeppink4_1]Создаю ссылку на папку SourceMods...[/]");
                AnsiConsole.MarkupLine("");
            

                    ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mklink /j \"{synpath}\\steamapps\\sourcemods\" \"{cldir}\\steamapps\\sourcemods\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process.Start(psi);
            }
            else
            {
                AnsiConsole.MarkupLine("[deeppink4_1]Ссылка на папку SourceMods уже создана...[/]");
                AnsiConsole.MarkupLine("");
            }

            Thread.Sleep(5000);

            if (!Directory.Exists($"{steamdiskc}\\steamapps\\sourcemods\\BMS"))
            {

                ProcessStartInfo psi2 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mklink /j \"{steamdiskc}\\steamapps\\sourcemods\" \"{cldir}\\steamapps\\sourcemods\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process.Start(psi2);

                AnsiConsole.MarkupLine("[red]Создаю ссылку на папку SourceMods из Steam Floader to DISK C: ...[/]");
                AnsiConsole.MarkupLine("");
            } 

            if (!File.Exists($"{extractsrc}\\addons\\metamod.vdf"))
            {
                Console.WriteLine("Не удалось установить SourceMods!");
                Console.ReadKey();
                return;
            }


            AnsiConsole.MarkupLine("[red]SourceMods успешно установлен.[/]");

            Thread.Sleep(5000);

            XAMPP.PluginsInstall pl = new PluginsInstall();
            Console.Clear();
            pl.plinstall();
        }




        private static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("Error downloading file: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                Console.WriteLine("Download has been canceled.");
            }
            else
            {
                AnsiConsole.MarkupLine("[lime]Успешно скачанно.[/]");
                AnsiConsole.MarkupLine("");
            }
        }

    }
}
        
    
