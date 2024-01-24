using SevenZipExtractor;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;

namespace ConsoleApp1.XAMPP
{
    internal class SteamCMD
    {
        public class Direct
        {
            public static string ServerInstallDirectory
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "settings.dat")).Take(2).ToArray()[1]; }
            }

            public static string SteamPatch
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "settings.dat")).Take(2).ToArray()[0]; }
            }

            public static string SteamLogin
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "save.dat")).Take(2).ToArray()[0]; }
            }

            public static string SteamPassword
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "save.dat")).Take(2).ToArray()[1]; }
            }

            public static string serverIP
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "settingsServer.txt")).Take(2).ToArray()[0]; }
            }

            public static string serverPort
            {
                get { return File.ReadLines(Path.Combine(bmsInstallerPath, "settingsServer.txt")).Take(2).ToArray()[1]; }
            }

            public static string bmsInstallerPath
            {
                get
                {
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    return Path.Combine(appDataPath, "BMSINSTALLER");
                }
            }
        }


        public void InstallServerSynergy()
        {

            if(!File.Exists($"{Direct.bmsInstallerPath}\\settings.dat"))
            {

                AnsiConsole.MarkupLine("[red]У вас не указаны пути к папке Steam и куда нужно установить сервер.[/]");
                AnsiConsole.MarkupLine("[blue]Дальнейшая установка невозможна.[/]");

                Thread.Sleep(5000);

                XAMPP.MenuS.StartMenu st2 = new MenuS.StartMenu();
                Console.Clear();
                st2.Start();

            }
            else if (!Directory.Exists($"{Direct.SteamPatch}\\steamapps\\sourcemods\\BMS"))
            {

                AnsiConsole.MarkupLine("[red]У вас не установлен BMS в клиенте Synergy.[/]");
                AnsiConsole.MarkupLine("[blue]Дальнейшая установка невозможна.[/]");

                Thread.Sleep(5000);

                XAMPP.MenuS.StartMenu st2 = new MenuS.StartMenu();
                Console.Clear();
                st2.Start();

            }
            else
            {
                // Процесс установки сервера через SteamCmd


                XAMPP.SteamCMD.Direct direct = new XAMPP.SteamCMD.Direct();

                int SynergyAppId = 17520;

                string serverInstallDirectory = Direct.ServerInstallDirectory;

                string name = Direct.SteamLogin;

                string pass = Direct.SteamPassword;

                AnsiConsole.MarkupLine("[yellow]Запуск SteamCmd...[/]");

                Thread.Sleep(5000);

                // Create a new SteamCMDConPTY instance
                SteamCMDConPTY steamCMDConPTY = new SteamCMDConPTY();

                // Set the working directory to the current directory
                steamCMDConPTY.WorkingDirectory = serverInstallDirectory;

                // Set the arguments to install the server
                steamCMDConPTY.Arguments = $" +login {name} {pass} +app_update {SynergyAppId} +quit";

                // We need to filter the control sequences in console
                steamCMDConPTY.FilterControlSequences = true;

                // Set the title of the window when the title data is received
                steamCMDConPTY.TitleReceived += (sender, data) =>
                {
                    Console.Title = data;
                };

                // Append the output data to the console
                steamCMDConPTY.OutputDataReceived += (sender, data) =>
                {
                    Console.Write(data);
                };

                // Close the console when steamcmd.exe exits
                steamCMDConPTY.Exited += (sender, exitCode) =>
                {
                    if (exitCode == 0)
                    {
                        Console.WriteLine("Установка файлов сервера успешно завершена.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        XAMPP.SetupSettingsServer setup = new SetupSettingsServer();
                        setup.SetupSettings();
                    }
                    else if (exitCode == 7)
                    {
                        Console.WriteLine("Установка файлов сервера успешно завершена.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        XAMPP.SetupSettingsServer setup = new SetupSettingsServer();
                        setup.SetupSettings();
                    }
                    else
                    {
                        Console.WriteLine("SteamCMD exited with code: " + exitCode);
                        Console.ReadKey(true);
                        Environment.Exit(exitCode);
                    }
                };

                // Start steamcmd conpty
                ProcessInfo processInfo = steamCMDConPTY.Start();

                // Set up a listener for when the user presses enter, and send the input to steamcmd conpty
                Console.TreatControlCAsInput = false;
                Console.CancelKeyPress += (s, e) =>
                {
                    steamCMDConPTY.Dispose();
                    e.Cancel = true;
                };
                Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));
                while (true)
                {
                    string input = Console.ReadLine();

                    if (input == ":quit")
                    {
                        steamCMDConPTY.Dispose();
                        break;
                    }
                    steamCMDConPTY.WriteLine(input);
                }
            }


        }



    }


        }




