using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ConsoleApp1.Client.Download;
using ConsoleApp1.XAMPP;
using SevenZipExtractor;
using Spectre.Console;

namespace ConsoleApp1.Client.XAMPP
{
    internal class DownloadXEN
    {
        public void dwnxen()
        {
            string steamfloader = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;
            string dwnxen = "1DEJwtJwn-syhsdUBoSNl36Ew0bRA1Hoh";
            string appdata = ConsoleApp1.XAMPP.SteamCMD.Direct.bmsInstallerPath;
            string sourcemods = steamfloader + "\\steamapps\\sourcemods";

            AnsiConsole.MarkupLine("[blue1]Произвожу скачивание BMS.7z может занять время (зависит от скорости интернета)...[/]");
            AnsiConsole.MarkupLine("");

            DownloadFilesGoogleDrive.GetFilesAsync(dwnxen, sourcemods).Wait();

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

            using (ArchiveFile archiveFile = new ArchiveFile(sourcemods + "\\xen.7z"))
            {
                archiveFile.Extract(sourcemods);
            }

            AnsiConsole.MarkupLine("[gold1]Распакова успешно завершена...[/]");
            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine("[hotpink]Удаление временных файлов...[/]");
            AnsiConsole.MarkupLine("");

            File.Delete(sourcemods + "\\BMS.7z");
            File.Delete(sourcemods + "\\xen.7z");
            File.Delete(sourcemods + "\\bmscripts.zip");

            AnsiConsole.MarkupLine("[yellow3]Успешно...[/]");
            AnsiConsole.MarkupLine("");

            Thread.Sleep(5000);

            CheckWorkShopContent check = new CheckWorkShopContent();
            Console.Clear();
            check.checkworkshop();
        }
    }
}
