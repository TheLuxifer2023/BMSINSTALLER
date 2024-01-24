using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using ConsoleApp1.Client.Download;
using ConsoleApp1.XAMPP;
using ConsoleApp1.XAMPP.MenuS;
using SevenZipExtractor;
using Spectre.Console;

internal class DownloadBMS
{
    public void dwnbms()
    {
        string steamfloader2 = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;

        if (!Directory.Exists(steamfloader2 + "\\steamapps\\common\\Synergy"))
        {
            AnsiConsole.MarkupLine("[blue1]Игра Synergy не скачана куда ты?...[/]");
            AnsiConsole.MarkupLine("[red]Дальнейшая установка не может быть произведена, попробуйте еще раз...[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            StartMenu st2 = new StartMenu();
            Console.Clear();
            st2.Start();
        }
        else
        {
            AnsiConsole.MarkupLine("[slateblue1]Игра Synergy скачана, продолжаю установку...[/]");
            AnsiConsole.MarkupLine("");
        }

        string steamfloader = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;
        string dwnbms = "1aEV-_kw4Rlk8rOTGZbgEp92dD_RklAFr";
        string appdata = ConsoleApp1.XAMPP.SteamCMD.Direct.bmsInstallerPath;
        string sourcemods = steamfloader + "\\steamapps\\sourcemods";
        string address1 = "https://github.com/Balimbanana/SourceScripts/raw/master/synotherfilefixes/bmscripts.zip";
        string patch1 = sourcemods + "\\bmscripts.zip";

        if (!Directory.Exists(steamfloader + "\\steamapps\\sourcemods"))
        {
            AnsiConsole.MarkupLine("[blue1]Папка sourcemods создана...[/]");
            AnsiConsole.MarkupLine("");

            Directory.CreateDirectory(steamfloader + "\\steamapps\\sourcemods");
        }
        else
        {
            AnsiConsole.MarkupLine("[blue1]Папка sourcemods уже существует...[/]");
            AnsiConsole.MarkupLine("");
        }

        AnsiConsole.MarkupLine("[blue1]Произвожу скачивание BMS.7z может занять время (зависит от скорости интернета)...[/]");
        AnsiConsole.MarkupLine("");

        DownloadFilesGoogleDrive.GetFilesAsync(dwnbms, sourcemods).Wait();

        string processName = "ServerApiGet.exe";
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/c taskkill /IM " + processName + " /F";
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        Process process = new Process();

        process.StartInfo = startInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        AnsiConsole.MarkupLine("[orangered1]Скачивание успешно завершенно...[/]");
        AnsiConsole.MarkupLine("[red]Произвожу распаковку...[/]");
        AnsiConsole.MarkupLine("");

        using (ArchiveFile archiveFile2 = new ArchiveFile(sourcemods + "\\BMS.7z"))
        {
            archiveFile2.Extract(sourcemods);
        }

        AnsiConsole.MarkupLine("[gold1]Распакова успешно завершена...[/]");
        AnsiConsole.MarkupLine("");

        Thread.Sleep(5000);

        AnsiConsole.MarkupLine("[lightgoldenrod1]Применяю скрипты на BMS...[/]");
        AnsiConsole.MarkupLine("");

        if (!Directory.Exists(sourcemods + "\\BMS\\scripts"))
        {
            AnsiConsole.MarkupLine("[blue1]Папки scripts не существует...[/]");
            AnsiConsole.MarkupLine("[red]Дальнейшая установка не может быть произведена, попробуйте еще раз...[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            StartMenu st = new StartMenu();
            Console.Clear();
            st.Start();

        }

        using (WebClient webClient = new WebClient())
        {
            webClient.DownloadFileCompleted += DownloadCompleted;
            webClient.DownloadFile(new Uri(address1), patch1);
        }

       /* string currentPath = sourcemods + "\\BMS\\scripts";
        string newName = "scriptsback";
        if (!Directory.Exists(currentPath))
        {
            Console.WriteLine("Source directory does not exist.");
            return;
        }
        try
        {
            Directory.Move(currentPath, newName);
            Console.WriteLine("Directory successfully renamed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error renaming directory. " + ex.Message);
        }*/

        using (ArchiveFile archiveFile = new ArchiveFile(sourcemods + "\\bmscripts.zip"))
        {
            archiveFile.Extract(sourcemods + "\\BMS\\scripts");
        }

        AnsiConsole.MarkupLine("[deeppink1]Скрипты успешно установлены...[/]");
        AnsiConsole.MarkupLine("");

        Thread.Sleep(5000);

        DownloadXEN xen = new DownloadXEN();
        Console.Clear();
        xen.dwnxen();
    }

    private static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            Console.WriteLine("Error downloading file: " + e.Error.Message);
            return;
        }
        if (e.Cancelled)
        {
            Console.WriteLine("Download has been canceled.");
            return;
        }
        AnsiConsole.MarkupLine("[lime]Успешно скачанно.[/]");
        AnsiConsole.MarkupLine("");
    }

    private static void RenameFolder(string currentPath, string newName)
    {
        try
        {
            if (!Directory.Exists(currentPath))
            {
                Console.WriteLine("The specified folder does not exist.");
                return;
            }
            string newPath = Path.Combine(Path.GetDirectoryName(currentPath), newName);
            Directory.Move(currentPath, newPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
