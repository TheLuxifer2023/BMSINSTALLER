using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ConsoleApp1.XAMPP;
using ConsoleApp1.XAMPP.MenuS;
using Spectre.Console;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;

internal class CheckWorkShopContent
{
    public void checkworkshop()
    {
        string steamfloader = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;
        string appdata = ConsoleApp1.XAMPP.SteamCMD.Direct.bmsInstallerPath;
        string sourcemods = steamfloader + "\\steamapps\\sourcemods";
        string workshop = steamfloader + "\\steamapps\\workshop\\content\\17520";
        string workshopconetent = steamfloader + "\\steamapps\\workshop\\content";
        string blackMesaSynergyCustomPatch = steamfloader + "\\steamapps\\common\\Synergy\\synergy\\custom";

        if (!Directory.Exists(workshopconetent))
        {
            AnsiConsole.MarkupLine("[blue1]Папка workshop/content создана...[/]");
            AnsiConsole.MarkupLine("");

            Directory.CreateDirectory(steamfloader + "\\steamapps\\workshop\\content");
        }
        else
        {
            AnsiConsole.MarkupLine("[blue1]Папка workshop/content уже существует...[/]");
            AnsiConsole.MarkupLine("");
        }


        if (!Directory.Exists(workshop))
        {
            AnsiConsole.MarkupLine("[grey69]Контент не скачан через стим, произвожу скачивание автоматически...[/]");
            AnsiConsole.MarkupLine("");

            int workshopid = 17520;
            int workshopappid = 1817140991;

            AnsiConsole.MarkupLine("[yellow]Запуск SteamCmd...[/]");

            Thread.Sleep(5000);

            SteamCMDConPTY steamCMDConPTY = new SteamCMDConPTY();
            steamCMDConPTY.WorkingDirectory = steamfloader;
            steamCMDConPTY.Arguments = $" +login anonymous +workshop_download_item {workshopid} {workshopappid} +quit";
            steamCMDConPTY.FilterControlSequences = true;
            steamCMDConPTY.TitleReceived += delegate (object sender, string data)
            {
                Console.Title = data;
            };
            steamCMDConPTY.OutputDataReceived += delegate (object sender, string data)
            {
                Console.Write(data);
            };
            steamCMDConPTY.Exited += delegate (object sender, int exitCode)
            {
                switch (exitCode)
                {
                    case 0:
                        Console.WriteLine("Установка workshop файлов успешно завершено.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        setup();
                        break;
                    case 7:
                        Console.WriteLine("Установка workshop файлов успешно завершено.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        setup();
                        break;
                    default:
                        Console.WriteLine("SteamCMD exited with code: " + exitCode);
                        Console.ReadKey(intercept: true);
                        Environment.Exit(exitCode);
                        break;
                }
            };

            ProcessInfo processInfo = steamCMDConPTY.Start(120, 30);

            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += delegate (object s, ConsoleCancelEventArgs e)
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
                    break;
                }
                steamCMDConPTY.WriteLine(input);
            }
            steamCMDConPTY.Dispose();
        }
        else
        {
            AnsiConsole.MarkupLine("[blue1]Папка с Контентом уже существует...[/]");
            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine("[red3]Копирую контент в Synergy...[/]");
            AnsiConsole.MarkupLine("");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "robocopy.exe",
                Arguments = "/S /NP /NJS /NJH /NS \"" + workshop + "\" \"" + blackMesaSynergyCustomPatch + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process robocopyProcess55 = process;
            robocopyProcess55.Start();
            robocopyProcess55.WaitForExit();

            AnsiConsole.MarkupLine("[palegreen3_1]Успешно...[/]");
            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine("[darkred_1]Контент успешно установлен...[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            StartMenu st = new StartMenu();
            Console.Clear();
            st.Start();
        }
    }

    public void setup()
    {
        string steamfloader = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;
        string appdata = ConsoleApp1.XAMPP.SteamCMD.Direct.bmsInstallerPath;
        string sourcemods = steamfloader + "\\steamapps\\sourcemods";
        string workshop = steamfloader + "\\steamapps\\workshop\\content\\17520";
        string workshopconetent = steamfloader + "\\steamapps\\workshop\\content";
        string blackMesaSynergyCustomPatch = steamfloader + "\\steamapps\\common\\Synergy\\synergy\\custom";

        AnsiConsole.MarkupLine("[red3]Копирую контент в Synergy...[/]");
        AnsiConsole.MarkupLine("");

        Process process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "robocopy.exe",
            Arguments = "/S /NP /NJS /NJH /NS \"" + workshop + "\" \"" + blackMesaSynergyCustomPatch + "\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process robocopyProcess55 = process;
        robocopyProcess55.Start();
        robocopyProcess55.WaitForExit();

        AnsiConsole.MarkupLine("[palegreen3_1]Успешно...[/]");
        AnsiConsole.MarkupLine("");

        AnsiConsole.MarkupLine("[darkred_1]Контент успешно установлен...[/]");
        AnsiConsole.MarkupLine("");

        StartMenu st = new StartMenu();
        Console.Clear();
        st.Start();
    }
}
