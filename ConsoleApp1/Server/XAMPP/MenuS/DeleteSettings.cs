using ConsoleApp1.XAMPP.MenuS;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.Server.XAMPP.MenuS
{
    internal class DeleteSettings
    {
        public void delsettings()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bmsInstallerPath = Path.Combine(appDataPath, "BMSINSTALLER");

            File.Delete($"{bmsInstallerPath}\\settings.dat");
            File.Delete($"{bmsInstallerPath}\\settingsServer.txt");
            File.Delete($"{bmsInstallerPath}\\save.dat");

            AnsiConsole.MarkupLine($"[darkblue] Настройки успешно сброшены.[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            StartMenu st2 = new StartMenu();
            Console.Clear();
            st2.Start();

        }
    }
}
