using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP
{
    internal class OpenSettingsServerFile
    {
        public void opensettings()
        {

            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory; // Synergy Server Floder

            string filePath = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\server2.cfg";

            AnsiConsole.MarkupLine("[purple]Открываю в блокноте файл конфигурации сервера[/]");

            Thread.Sleep(5000);

                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "notepad.exe",
                        Arguments = filePath
                    }
                };

                process.Start();

                XAMPP.SourceModsInstall lad = new SourceModsInstall();
                Console.Clear();
                lad.srcmdinstall();


            /* if (!AnsiConsole.Confirm("[purple]Хотите открыть в блокноте файл конфигурации сервера?[/]"))
             {
                 XAMPP.SourceModsInstall lad = new SourceModsInstall();
                 Console.Clear();
                 lad.srcmdinstall();
             }
             else
             {

                 string filePath = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\server2.cfg";

                 Process process = new Process
                 {
                     StartInfo = new ProcessStartInfo
                     {
                         FileName = "notepad.exe",
                         Arguments = filePath
                     }
                 };

                 process.Start();

                 XAMPP.SourceModsInstall lad = new SourceModsInstall();
                 Console.Clear();
                 lad.srcmdinstall();

             }*/
        }
    }
}
