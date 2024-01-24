using ConsoleApp1.XAMPP.MenuS;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ConsoleApp1.XAMPP.SteamCMD;
using static System.Windows.Forms.LinkLabel;

namespace ConsoleApp1.XAMPP.Steam
{
    internal class Authorization
    {
        object lockObject = new object();
        public class Direct4
        {
            public string login;
            public string pass;
        }


        public async Task authAsync()
        {

            XAMPP.SteamCMD installst = new SteamCMD();


            var rule = new Rule("[red]АВТОРИЗАЦИЯ СТИМ[/]");
            AnsiConsole.Write(rule);

            var name = AnsiConsole.Ask<string>("Логин [green]стима[/]:");

            var pass = AnsiConsole.Prompt(
            new TextPrompt<string>("Пароль [green]стима[/]:")
            .PromptStyle("red")
            .Secret());

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bmsInstallerPath = Path.Combine(appDataPath, "BMSINSTALLER");
            string settingsFilePath = Path.Combine(bmsInstallerPath, "save.dat");

            if (!File.Exists($"{bmsInstallerPath}\\save.dat"))
            {
                File.WriteAllText(settingsFilePath, @""
);
            }

            if (File.Exists($"{bmsInstallerPath}\\save.dat"))
            {
                string[] lines = File.ReadAllLines(settingsFilePath);
                if (lines.Length >= 2 && lines[0].Trim() == name && lines[1].Trim() == pass)
                {
                    Console.WriteLine("Values are already written on line 1 and 2 of the file.");
                    return;
                }
            }

            File.WriteAllLines(settingsFilePath, new string[] { name, pass });

            Thread.Sleep(5000);

           /* string[] kek = File.ReadAllLines(Path.Combine(bmsInstallerPath, "save.dat"));

            if (kek.Length < 2)
            {
                throw new ArgumentOutOfRangeException("save.dat", "The file must have at least 2 lines.");
            }

            var direct = new Direct4();
            direct.login = kek[0]; */

            if (!AnsiConsole.Confirm("[Red]Проверьте правильный логин или пароль, если верно напишите 'y' и нажмите 'Enter'[/]"))
            {
                XAMPP.Steam.Authorization la = new Authorization();
                Console.Clear();
                await la.authAsync();
                return;
            }

            goto continueLabel;

        continueLabel:
            installst.InstallServerSynergy();

        }


    }
}
