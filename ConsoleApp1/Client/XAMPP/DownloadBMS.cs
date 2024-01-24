using DK.Standard;
using PH.PicoCrypt2;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.Client.Download.DownloadFilesGoogleDrive;

namespace ConsoleApp1.Client.XAMPP
{
    internal class DownloadBMS
    {
        public void dwnbms()
        {
            string steamfloader = ConsoleApp1.XAMPP.SteamCMD.Direct.SteamPatch;
            string dwnbms = "1aEV-_kw4Rlk8rOTGZbgEp92dD_RklAFr";

            // Check if SourceMods Floader in Steam Floader is Exists
            if (!Directory.Exists($"{steamfloader}\\steamapps\\sourcemods"))
            {
                AnsiConsole.MarkupLine("[blue1]Папка sourcemods создана...[/]");
                AnsiConsole.MarkupLine("");

                Directory.CreateDirectory($"{steamfloader}\\steamapps\\sourcemods");

                goto bmsdwn;
            }
            else
            {
                AnsiConsole.MarkupLine("[blue1]Папка sourcemods уже существует...[/]");
                AnsiConsole.MarkupLine("");
                goto bmsdwn;
            }

        bmsdwn:

            AnsiConsole.MarkupLine("[blue1]Произвожу скачивание BMS.7z может занять время (зависит от скорости интернета)...[/]");
            AnsiConsole.MarkupLine("");

            Client.Download.DownloadFilesGoogleDrive.GetFilesAsync(dwnbms, steamfloader).Wait();



        }
    }
}
