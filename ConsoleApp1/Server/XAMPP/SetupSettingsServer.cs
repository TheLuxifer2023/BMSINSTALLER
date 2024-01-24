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

namespace ConsoleApp1.XAMPP
{
    internal class SetupSettingsServer
    {
        public void SetupSettings()
        {
            AnsiConsole.MarkupLine("[teal]Произвожу настройку сервер...[/]");
            AnsiConsole.MarkupLine("");

            string cldir = XAMPP.SteamCMD.Direct.SteamPatch; //Steam Floder
            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory; // Synergy Server Floder
            string syntype = "56.16";

            if (!Directory.Exists($"{cldir}\\steamapps\\common\\Half-Life 2"))
            {
                var path = new TextPath(@"Half-Life 2 Не установлен или не куплен, установите игру!");

                AnsiConsole.Write(path);

                Console.ReadKey();
            }
            else
            {
                goto installhl2;
            };

        installhl2:

            AnsiConsole.MarkupLine("[deeppink4_1]Создаю ссылку на папку HL2...[/]");
            AnsiConsole.MarkupLine("");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c mklink /j \"{synpath}\\steamapps\\common\\Half-Life 2\" \"{cldir}\\steamapps\\common\\Half-Life 2\"",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process.Start(psi);

            AnsiConsole.MarkupLine("[darkred]Скачиваю нужны файлы для сервера...[/]");
            AnsiConsole.MarkupLine("");
            WebClient wc3 = new WebClient();
            wc3.DownloadFile($"https://github.com/Balimbanana/SM-Synergy/raw/master/scripts/weapon_betagun.txt", $"{synpath}\\steamapps\\common\\Synergy\\synergy\\scripts\\weapon_betagun.txt");
            if (!Directory.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\models\\weapons")) Directory.CreateDirectory($"{synpath}\\steamapps\\common\\Synergy\\synergy\\models\\weapons");
            wc3.DownloadFile($"https://github.com/Balimbanana/SourceScripts/raw/master/synotherfilefixes/w_physics.phy", $"{synpath}\\steamapps\\common\\Synergy\\synergy\\models\\weapons\\w_physics.phy");
            AnsiConsole.MarkupLine("[blue1]Создаю файл конфигурации сервера...[/]");
            AnsiConsole.MarkupLine("");
            if (!File.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\server2.cfg"))
            {
                File.WriteAllText($"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\server2.cfg", $@"hostname First Syn {syntype} Server
sv_lan 0
mp_friendlyfire 0
mp_reset 1
mp_antirush_percent 50
mp_transition_time 45
mp_transition_percent 68
sv_vote_enable 1
sv_vote_failure_timer 300
sv_vote_interval 10
sv_vote_percent_difficulty 67
sv_vote_percent_kick 67
sv_vote_percent_map 67
sv_vote_percent_restore 67
host_thread_mode 2
net_splitrate 3
net_splitpacket_maxrate 100000
net_maxcleartime 0.01
sv_parallel_sendsnapshot 1
sv_rollangle 0.0
//Change this to a different savenumber for forked servers
sv_savedir save2/
content_mount_synergy_mod_path_priority 2");
            }
            else
            {
                AnsiConsole.MarkupLine("[blue1]Файл конфигурации сервера уже существует...[/]");
                AnsiConsole.MarkupLine("");
            }
            if (!File.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\workshop_srv.cfg"))
            {
                File.WriteAllText($"{synpath}\\steamapps\\common\\Synergy\\synergy\\cfg\\workshop_srv.cfg", @"1082553471
492916281
647127451
647128829
678214923
664873590
683512034
682177824
692269416
694312354
691111508
698969705
689508204
1917233439
647128322
733880910
751771158
762217131
783933738
860392418
886714754
909637644
918216553
931794062
1230906124
1286998604
1427833667
1650998121
1654962168
1657567270
1817140991");
            }
            else
            {
                AnsiConsole.MarkupLine("[blue1]Файл конфигурации сервера WorkShop.cfg уже существует...[/]");
                AnsiConsole.MarkupLine("");
            }


            if (!Directory.Exists($"{synpath}\\steamapps\\common\\Synergy\\synergy\\content"))
            {
                Directory.CreateDirectory($"{synpath}\\steamapps\\common\\Synergy\\synergy\\content");
            }

                XAMPP.OpenSettingsServerFile settings = new OpenSettingsServerFile();

                settings.opensettings();
            




        }
    }
    }


