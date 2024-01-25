using IronPython.Hosting;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ConsoleApp1.XAMPP.MenuS.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using ConsoleApp1.Client;
using ConsoleApp1.Server.XAMPP.MenuS;

namespace ConsoleApp1.XAMPP.MenuS
{
    internal class StartMenu
    {

        [STAThread]
        public void Start()
        {
            //create instance with menu header, set true for looped menu cursor
            ConsoleMenu menu = new ConsoleMenu(@"
 ▄▄▄▄    ███▄ ▄███▓  ██████  ██▓ ███▄    █   ██████ ▄▄▄█████▓ ▄▄▄       ██▓     ██▓    ▓█████  ██▀███  
▓█████▄ ▓██▒▀█▀ ██▒▒██    ▒ ▓██▒ ██ ▀█   █ ▒██    ▒ ▓  ██▒ ▓▒▒████▄    ▓██▒    ▓██▒    ▓█   ▀ ▓██ ▒ ██▒
▒██▒ ▄██▓██    ▓██░░ ▓██▄   ▒██▒▓██  ▀█ ██▒░ ▓██▄   ▒ ▓██░ ▒░▒██  ▀█▄  ▒██░    ▒██░    ▒███   ▓██ ░▄█ ▒
▒██░█▀  ▒██    ▒██   ▒   ██▒░██░▓██▒  ▐▌██▒  ▒   ██▒░ ▓██▓ ░ ░██▄▄▄▄██ ▒██░    ▒██░    ▒▓█  ▄ ▒██▀▀█▄  
░▓█  ▀█▓▒██▒   ░██▒▒██████▒▒░██░▒██░   ▓██░▒██████▒▒  ▒██▒ ░  ▓█   ▓██▒░██████▒░██████▒░▒████▒░██▓ ▒██▒
░▒▓███▀▒░ ▒░   ░  ░▒ ▒▓▒ ▒ ░░▓  ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░  ▒ ░░    ▒▒   ▓▒█░░ ▒░▓  ░░ ▒░▓  ░░░ ▒░ ░░ ▒▓ ░▒▓░
▒░▒   ░ ░  ░      ░░ ░▒  ░ ░ ▒ ░░ ░░   ░ ▒░░ ░▒  ░ ░    ░      ▒   ▒▒ ░░ ░ ▒  ░░ ░ ▒  ░ ░ ░  ░  ░▒ ░ ▒░
 ░    ░ ░      ░   ░  ░  ░   ▒ ░   ░   ░ ░ ░  ░  ░    ░        ░   ▒     ░ ░     ░ ░      ░     ░░   ░ 
 ░             ░         ░   ░           ░       ░                 ░  ░    ░  ░    ░  ░   ░  ░   ░     
      ░                                                                                                
", true);

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bmsInstallerPath = Path.Combine(appDataPath, "BMSINSTALLER");

            if (!Directory.Exists(bmsInstallerPath))
            {
                Directory.CreateDirectory(bmsInstallerPath);
            }

            string firstLine = "";
            string secondLine = "";

            try
            {
                string[] firstTwoLines = File.ReadLines($"{bmsInstallerPath}\\settings.dat").Take(2).ToArray();

                firstLine = firstTwoLines[0];
                secondLine = firstTwoLines[1];


                // ... остальные действия с файлом
            }
            catch (FileNotFoundException)
            {
                Console.Clear();
            }

            //adding items into menu
            menu.AddItem(new Button("Установить клиент BMS"));                 //item index 0
            menu.AddItem(new Button("Установить сервер BMS"));                 //item index 1
            menu.AddItem(new Button("Запустить сервер BMS"));                 //item index 2
            menu.AddItem(new Input("Путь до папки Steam", firstLine));                  //item index 3
            menu.AddItem(new InputServerLocation("Путь где будет хранится сервер", secondLine));                  //item index 4
            menu.AddItem(new Button("[Опционально] Сбросить все настройки"));                 //item index 5






            //method Display() returns index of pressed button and show interactive menu in console
            int menu_id = menu.Display();


            //write id of pressed button
            Console.WriteLine(menu_id);

            //write data of menu item with index 0
            Console.WriteLine(menu.GetData()[0]);

            //write data of menu item with index 1
            Console.WriteLine(menu.GetData()[1]);

            //write data of menu item with index 2
            Console.WriteLine(menu.GetData()[2]);

            //write data of menu item with index 3
            Console.WriteLine(menu.GetData()[3]);

            //write data of menu item with index 4
            Console.WriteLine(menu.GetData()[4]);

            //write data of menu item with index 5
            Console.WriteLine(menu.GetData()[5]);


            switch (menu_id)
            {
                case 0:
                    Console.Clear();

                    Client.XAMPP.DownloadBMS h = new Client.XAMPP.DownloadBMS();


                    h.dwnbms();
                    
                    break;
                case 1:

                    Console.Clear();

                    if (!File.Exists($"{bmsInstallerPath}\\save.dat"))
                    {
                        XAMPP.Steam.Authorization auth = new Steam.Authorization();
                        auth.authAsync();
                    }
                    else
                    {
                        //XAMPP.SetupSettingsServer g = new SetupSettingsServer();
                        //XAMPP.BMSINSTALL g = new BMSINSTALL();
                        XAMPP.SteamCMD install = new SteamCMD();
                        install.InstallServerSynergy();
                        //g.SetupSettings();
                        // g.BMS();
                    }

                    break;
             case 2:

                    Console.Clear();

                    XAMPP.SourceModsInstall lp = new SourceModsInstall();

                    XAMPP.StartServer ststart = new XAMPP.StartServer();

                    ststart.StartBMSServer();

                    break;
                case 5:
                    Console.Clear();

                    DeleteSettings del = new DeleteSettings();

                    del.delsettings();

                    break;
            }




        }

    }
}
