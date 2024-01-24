using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP
{
    internal class PluginsInstall
    {
        public void plinstall()
        {
            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory;
            string pluginspatch = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\addons\\sourcemod\\plugins";
            string translationpatch = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\addons\\sourcemod\\translations";

            AnsiConsole.MarkupLine("[blue1]Это позволит напрямую загрузить необходимые плагины для SourceMods....[/]");
            AnsiConsole.MarkupLine("[blue1]Это может занять несколько минут в зависимости от скорости загрузки.[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            string address1 = "https://github.com/Balimbanana/SM-Synergy/raw/master/plugins/synsaverestore.smx";

            string address2 = "https://github.com/Balimbanana/SM-Synergy/raw/master/plugins/healthdisplay.smx";

            string address3 = "https://github.com/Balimbanana/SM-Synergy/raw/master/translations/healthdisplay.phrases.txt"; // translations files

            string address4 = "https://github.com/Balimbanana/SM-Synergy/raw/master/translations/colors.phrases.txt"; // translations files

            string address5 = "https://github.com/Balimbanana/SM-Synergy/raw/master/plugins/synsweps.smx";

            string address6 = "https://github.com/Balimbanana/SM-Synergy/raw/master/plugins/synfixesdev.smx";

            string patch1 = $"{pluginspatch}\\synsaverestore.smx";

            string patch2 = $"{pluginspatch}\\healthdisplay.smx";

            string patch3 = $"{translationpatch}\\healthdisplay.phrases.txt"; // translations files

            string patch4 = $"{translationpatch}\\colors.phrases.txt"; // translations files

            string patch5 = $"{pluginspatch}\\synsweps.smx";

            string patch6 = $"{pluginspatch}\\synfixesdev.smx";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                webClient.DownloadFile(new Uri(address1), patch1);
                webClient.DownloadFile(new Uri(address2), patch2);
                webClient.DownloadFile(new Uri(address3), patch3);
                webClient.DownloadFile(new Uri(address4), patch4);
                webClient.DownloadFile(new Uri(address5), patch5);
                webClient.DownloadFile(new Uri(address6), patch6);
            }

            if (!File.Exists($"{pluginspatch}\\synsaverestore.smx"))
            {
                Console.WriteLine("Не удалось загрузить synsaverestore.smx. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{pluginspatch}\\healthdisplay.smx"))
            {
                Console.WriteLine("Не удалось загрузить healthdisplay.smx. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{pluginspatch}\\synsweps.smx"))
            {
                Console.WriteLine("Не удалось загрузит synsweps.smx. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{pluginspatch}\\synfixesdev.smx"))
            {
                Console.WriteLine("Не удалось загрузит synfixesdev.smx. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{translationpatch}\\healthdisplay.phrases.txt"))
            {
                Console.WriteLine("Не удалось загрузит healthdisplay.phrases.txt. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{translationpatch}\\colors.phrases.txt"))
            {
                Console.WriteLine("Не удалось загрузит colors.phrases.txt. Ошибка интернета или еше какие проблемы.");
                Console.ReadKey();
                return;
            }


            if (!File.Exists($"{translationpatch}\\colors.phrases.txt"))
            {
                Console.WriteLine("Не удалось установить плагины!");
                Console.ReadKey();
                return;
            }

            AnsiConsole.MarkupLine("[red]Плагины успешно установлены.[/]");

            Thread.Sleep(5000);

            XAMPP.BMSINSTALL bms = new BMSINSTALL();

            Console.Clear();

            bms.BMS();

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
