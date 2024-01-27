using IWshRuntimeLibrary;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1.XAMPP
{
    internal class StartServer
    {
        public void StartBMSServer()
        {

            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory;
            string worshopinstalled = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991";
            string cfgServerFile = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\server2.cfg";
            string SourceMods = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\addons\\sourcemod";

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bmsInstallerPath = Path.Combine(appDataPath, "BMSINSTALLER");
            string settingsFilePath = Path.Combine(bmsInstallerPath, "settingsServer.txt");

            if (!System.IO.File.Exists($"{bmsInstallerPath}\\settingsServer.txt"))
            {
                var rule = new Rule("[red]Введите Интернет (sv_lan 1) или Локальный (sv_lan 0)[/]");
                var rule1 = new Rule("[red]для сервера Интернет (открытые порты) => Локальный(RadminVPN, Hamachi)[/]");
                AnsiConsole.Write(rule);
                AnsiConsole.Write(rule1);
                AnsiConsole.MarkupLine("");

                var svlan = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Интернет (sv_lan 1) или Локальный (sv_lan 0)[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Двигайтесь вверх и вниз)[/]")
                        .AddChoices(new[] {
            "sv_lan 1", "sv_lan 0",
                        }));

                var port = AnsiConsole.Ask<string>("[green]Введите порт для сервера (Дефолт 27015)[/]:");

                if (!System.IO.File.Exists($"{bmsInstallerPath}\\settingsServer.txt"))
                {
                    System.IO.File.WriteAllText(settingsFilePath, @""
    );
                }

                if (System.IO.File.Exists($"{bmsInstallerPath}\\settingsServer.txt"))
                {
                    string[] lines = System.IO.File.ReadAllLines(settingsFilePath);
                    if (lines.Length >= 2 && lines[0].Trim() == svlan && lines[1].Trim() == port)
                    {
                        Console.WriteLine("Values are already written on line 1 and 2 of the file.");
                        return;
                    }
                }

                System.IO.File.WriteAllLines(settingsFilePath, new string[] { svlan, port });

                Console.Clear();

            }
            else
            {
                AnsiConsole.MarkupLine($"[blue1] Файл с настройками IP и PORTS уже существует.[/]");
                AnsiConsole.MarkupLine("");
            }


            Thread.Sleep(5000);

            string ips = XAMPP.SteamCMD.Direct.serverIP;
            string ports = XAMPP.SteamCMD.Direct.serverPort;
            string servsettingsipports = $"{bmsInstallerPath}\\settingsServer.txt";

            if (!Directory.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991"))
            {
                AnsiConsole.MarkupLine($"[blue1] Сервер не был запущен (либо вы его не установили, либо отсуствует папка с контентом).[/]");
                AnsiConsole.MarkupLine("");

                Thread.Sleep(5000);

                XAMPP.MenuS.StartMenu st = new MenuS.StartMenu();
                Console.Clear();
                st.Start();
            }
            else
            {
                goto bmsstart;
            }

        bmsstart:

            string shortcutPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\StartServerSynergy.lnk";

            if (!System.IO.File.Exists(shortcutPath1))
            {
                string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\StartServerSynergy.lnk";
                WshShell shell4 = new WshShell();
                IWshShortcut shortcut4 = (IWshShortcut)shell4.CreateShortcut(shortcutPath);

                // Set the target path of the shortcut
                shortcut4.TargetPath = $"{synpath}\\steamapps\\common\\Synergy\\srcds.exe";

                // Set the arguments for the shortcut
                shortcut4.Arguments = $"-console -game synergy +maxplayers 64 +{ips} +map \"hl2 d1_trainstation_06\" +exec {cfgServerFile} -ip 0.0.0.0 -port {ports} -nocrashdialog -insecure -nohltv -threads 8 -heapsize 2048000 -mem_max_heapsize 2048 -mem_max_heapsize_dedicated 512";

                // Save the shortcut
                shortcut4.Save();

                Thread.Sleep(5000);

                string shortcutPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\SettingsServerSynergy.lnk";
                WshShell shell2 = new WshShell();
                IWshShortcut shortcut2 = (IWshShortcut)shell2.CreateShortcut(shortcutPath2);

                // Set the target path of the shortcut
                shortcut2.TargetPath = $"{cfgServerFile}";

                // Set the arguments for the shortcut
                shortcut2.Arguments = $"";

                // Save the shortcut
                shortcut2.Save();

                Thread.Sleep(5000);

                string shortcutPath3 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\SettingsServerIP+PORTS.lnk";
                WshShell shell3 = new WshShell();
                IWshShortcut shortcut3 = (IWshShortcut)shell3.CreateShortcut(shortcutPath3);

                // Set the target path of the shortcut
                shortcut3.TargetPath = $"{servsettingsipports}";

                // Set the arguments for the shortcut
                shortcut3.Arguments = $"";

                // Save the shortcut
                shortcut3.Save();

                AnsiConsole.MarkupLine($"[darkblue] Ярлык запуска и настройки сервера создан на рабочем столе.[/]");
                AnsiConsole.MarkupLine("");

                Thread.Sleep(5000);

                Process robocopyProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = $"{synpath}\\steamapps\\common\\Synergy\\srcds.exe",
                        Arguments = $"-console -game synergy +maxplayers 64 +{ips} +map \"hl2 d1_trainstation_06\" +exec {cfgServerFile} -ip 0.0.0.0 -port {ports} -nocrashdialog -insecure -nohltv -threads 8 -heapsize 2048000 -mem_max_heapsize 2048 -mem_max_heapsize_dedicated 512",
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                robocopyProcess.Start();

                /*ProcessStartInfo startInfo2 = new ProcessStartInfo
                {
                    FileName = $"{synpath}\\steamapps\\common\\Synergy\\srcds.exe",
                    Arguments = $" -console -game synergy +maxplayers 64 +sv_lan 0 +map \"hl2 d1_trainstation_06\" +exec {cfgServerFile} -ip {ips} -port {ports} -nocrashdialog -insecure -nohltv -threads 8 -heapsize 2048000 -mem_max_heapsize 2048 -mem_max_heapsize_dedicated 512",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process2 = Process.Start(startInfo2))
                {
                    process2.Start();
                    process2.WaitForExit();
                }*/

                AnsiConsole.MarkupLine($"[blue1] {DateTime.Now} SynDS Сервер запущен.[/]");
                AnsiConsole.MarkupLine("");

                AnsiConsole.MarkupLine($"[darkorange3_1] Нажмите любую кнопку выйти в главное меню.[/]");
                Console.ReadKey();

                XAMPP.MenuS.StartMenu st2 = new MenuS.StartMenu();

                st2.Start();

            }
            else
            {
                AnsiConsole.MarkupLine($"[darkblue] Ярлык запуска и настройки сервера уже существует на рабочем столе.[/]");
                AnsiConsole.MarkupLine("");

                Process robocopyProcess2 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = $"{synpath}\\steamapps\\common\\Synergy\\srcds.exe",
                        Arguments = $"-console -game synergy +maxplayers 64 +{ips} +map \"hl2 d1_trainstation_06\" +exec {cfgServerFile} -ip 0.0.0.0 -port {ports} -nocrashdialog -insecure -nohltv -threads 8 -heapsize 2048000 -mem_max_heapsize 2048 -mem_max_heapsize_dedicated 512",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                robocopyProcess2.Start();





                /*    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = $"{synpath}\\steamapps\\common\\Synergy\\srcds.exe",
                        Arguments = $" -console -game synergy +maxplayers 64 +sv_lan 0 +map \"hl2 d1_trainstation_06\" +exec {cfgServerFile} -ip {ips} -port {ports} -nocrashdialog -insecure -nohltv -threads 8 -heapsize 2048000 -mem_max_heapsize 2048 -mem_max_heapsize_dedicated 512",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = Process.Start(startInfo))
                    {
                        process.Start();
                        process.WaitForExit();
                    }*/

                AnsiConsole.MarkupLine($"[blue1] {DateTime.Now} SynDS Сервер запущен.[/]");
                AnsiConsole.MarkupLine("");

                AnsiConsole.MarkupLine($"[darkorange3_1] Нажмите любую кнопку выйти в главное меню.[/]");
                Console.ReadKey();

                XAMPP.MenuS.StartMenu st2 = new MenuS.StartMenu();

                st2.Start();
            }


            }

        }
    }

    

