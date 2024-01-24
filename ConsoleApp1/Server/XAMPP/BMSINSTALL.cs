using Spectre.Console;
using SteamCMD.ConPTY.Interop.Definitions;
using SteamCMD.ConPTY;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using IWshRuntimeLibrary;
using IronPython.Runtime;
using IronPython.Hosting;
using System.ComponentModel;
using System.Net;
using SevenZipExtractor;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using IronPython.Modules;
using static Community.CsharpSqlite.Sqlite3;

namespace ConsoleApp1.XAMPP
{
    internal class BMSINSTALL
    {
        public void BMS()
        {

            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory;
            string synergyexe = $"{synpath}\\steamapps\\common\\Synergy\\hl2.exe";
            string blackMesaSynergyPatch = $"{synpath}\\steamapps\\workshop\\content";
            string blackMesaSynergyCustomPatch = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom";
            string steamFloader = XAMPP.SteamCMD.Direct.SteamPatch;
            string blackMesaSupportPath = $"{steamFloader}\\steamapps\\workshop\\content\\17520";
            string worshopinstalled = $"{synpath}\\steamapps\\workshop\\content\\17520";
            string vpkPath = blackMesaSynergyCustomPatch + @"\1817140991\BlackMesaSupport.vpk";
            string extractvpk = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\blackmesasupport";
            string movevpkextract = $"{synpath}\\steamapps\\common\\Synergy\\synergy";
            string appdata = XAMPP.SteamCMD.Direct.bmsInstallerPath;
            string pythonfile = $"{appdata}\\App.zip";
            string pythonpatch = $"{appdata}\\App";

            // Check if SteamCMD is installed
            if (!System.IO.File.Exists($"{synpath}\\steamcmd.exe"))
            {
                Console.WriteLine("SteamCMD is not installed. Please install it and try again.");
                return;
            }else
            {
                AnsiConsole.MarkupLine("[blue1]SteamCMD устновлен все ок...[/]");
                AnsiConsole.MarkupLine("");
            }

            // Check if Workshop Floader in Synergy Server Floader is Exists
            if (!Directory.Exists($"{synpath}\\steamapps\\workshop\\content"))
            {
                AnsiConsole.MarkupLine("[blue1]Папка workshop/content создана...[/]");
                AnsiConsole.MarkupLine(""); 

                Directory.CreateDirectory($"{synpath}\\steamapps\\workshop\\content");

                goto bmsinstall;
            }
            else
            {
                AnsiConsole.MarkupLine("[blue1]Папка workshop/content уже существует...[/]");
                AnsiConsole.MarkupLine("");
                goto bmsinstall;
            }

        bmsinstall:

            // Check if Black Mesa Support is installed
            if (!Directory.Exists(blackMesaSupportPath))
            {
                // Процесс установки workshop контента через SteamCmd

                int workshopid = 17520;

                int workshopappid = 1817140991;

                AnsiConsole.MarkupLine("[yellow]Запуск SteamCmd...[/]");

                Thread.Sleep(5000);

                // Create a new SteamCMDConPTY instance
                SteamCMDConPTY steamCMDConPTY = new SteamCMDConPTY();

                // Set the working directory to the current directory
                steamCMDConPTY.WorkingDirectory = synpath;

                // Set the arguments to install the server
                steamCMDConPTY.Arguments = $" +login anonymous +workshop_download_item {workshopid} {workshopappid} +quit";

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
                        Console.WriteLine("Установка workshop файлов успешно завершено.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        setup();
                    }
                    else if (exitCode == 7)
                    {
                        Console.WriteLine("Установка workshop файлов успешно завершено.");
                        Thread.Sleep(5000);
                        Console.Clear();
                        setup();
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
            else
            {
                // Copy files from Black Mesa workshop content to Synergy directory
                Process robocopyProcess55 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "robocopy.exe",
                        Arguments = $@"/S /NP /NJS /NJH /NS ""{blackMesaSupportPath}"" ""{blackMesaSynergyPatch}""",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                robocopyProcess55.Start();
                robocopyProcess55.WaitForExit();

                // Copy files from Black Mesa workshop content(custom floader) to Synergy directory
                Process robocopyProcess66 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "robocopy.exe",
                        Arguments = $@"/S /NP /NJS /NJH /NS ""{blackMesaSupportPath}"" ""{blackMesaSynergyCustomPatch}""",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                robocopyProcess66.Start();
                robocopyProcess66.WaitForExit();

                // mklink workshop content => custom floader
                ProcessStartInfo psi77 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mklink /j \"{blackMesaSynergyCustomPatch}\" \"{blackMesaSupportPath}\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process.Start(psi77);

                AnsiConsole.MarkupLine("[blue1]Произвожу распаковку VPK файла...[/]");
                AnsiConsole.MarkupLine("");

                // Install Python

                AnsiConsole.MarkupLine("[blue1]Скачивание Python....[/]");
                AnsiConsole.MarkupLine("[blue1]Это может занять несколько минут в зависимости от скорости загрузки.[/]");
                AnsiConsole.MarkupLine("");

                string address1 = "https://github.com/TheLuxifer2023/bmsresource/releases/download/fd/App.zip";
                string address2 = "https://raw.githubusercontent.com/TheLuxifer2023/bmsresource/main/get-pip.py";
                string address3 = "https://raw.githubusercontent.com/TheLuxifer2023/bmsresource/main/get-pip2.py";

                string patch1 = $"{appdata}\\App.zip";
                string patch2 = $"{appdata}\\App\\Python\\get-pip.py";
                string patch3 = $"{appdata}\\App\\Python\\get-pip2.py";


                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    webClient.DownloadFile(new Uri(address1), patch1);
                }

                AnsiConsole.MarkupLine("[red]Распаковка....[/]"); ;
                AnsiConsole.MarkupLine("");

                using (ArchiveFile archiveFile = new ArchiveFile($@"{appdata}\App.zip"))
                {
                    archiveFile.Extract(appdata); // extract Python
                };

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                    webClient.DownloadFile(new Uri(address2), patch2);
                    webClient.DownloadFile(new Uri(address3), patch3);
                }

                AnsiConsole.MarkupLine("[fuchsia]Применяю настройки для Python....[/]"); ;
                AnsiConsole.MarkupLine("");

                string func1 = @"function Add-Path-User($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""User"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if (-not $newPaths.Contains($newPath)) {
        $newPaths = @($newPath) + $newPaths
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""User"" )
    }
}";
                string func2 = @"function Add-Path-Machine($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""Machine"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if (-not $newPaths.Contains($newPath)) {
        $newPaths = @($newPath) + $newPaths
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""Machine"" )
    }
}";
                string func3 = @"function Remove-Path-Machine($pathToRemove) {
    $currentPath = [Environment]::GetVariable(""Path"", ""Machine"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if ($newPaths.Contains($pathToRemove)) {
        $newPaths = $newPaths | Where-Object { $_ -ne $pathToRemove }
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable(""Path"", $Path, ""Machine"")
    }
}";
                string func4 = @"function Remove-Path-User($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""User"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if ($newPaths.Contains($newPath)) {
        $newPaths = $newPaths | Where-Object { $_ -ne $newPath }
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""User"" )
    }
}";

                string script = $@"{func1}
                
                {func2}

Add-Path-User -newPath ""{appdata}\App\Python""

Add-Path-User -newPath ""{appdata}\App\Python\Scripts""

Add-Path-Machine -newPath ""{appdata}\App\Python""

Add-Path-Machine -newPath ""{appdata}\App\Python\Scripts""

";

                RunScript(script);

                AnsiConsole.MarkupLine("[red3]Если видите ошибки или WARING не обращаем внимание!!![/]"); ;
                AnsiConsole.MarkupLine("");
                string cmdCommand5 = $"{appdata}\\App\\Python\\python.exe \"{appdata}\\App\\Python\\get-pip.py\"";
                string cmdCommand6 = $"{appdata}\\App\\Python\\python.exe \"{appdata}\\App\\Python\\get-pip2.py\"";

                ProcessStartInfo psi8 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process process8 = new Process { StartInfo = psi8 };
                process8.Start();

                // Send the command to the cmd process
                process8.StandardInput.WriteLine(cmdCommand6);
                process8.StandardInput.WriteLine("exit");

                Thread.Sleep(3000);

                ProcessStartInfo psi7 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process process7 = new Process { StartInfo = psi7 };
                process7.Start();

                // Send the command to the cmd process
                process7.StandardInput.WriteLine(cmdCommand5);
                process7.StandardInput.WriteLine("exit");

                // Extract VPK using VPK Python Module

                // Read the output of the cmd process
                string output8 = process7.StandardOutput.ReadToEnd();
                process7.WaitForExit();

                string synpatchPath = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\BlackMesaSupport"; // replace with the actual path to the synpatch folder

                string cmdCommand = $"{appdata}\\App\\Python\\Scripts\\vpk.exe \"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\BlackMesaSupport.vpk\" -x \"{synpatchPath}\"";

                ProcessStartInfo psi5 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process process99 = new Process { StartInfo = psi5 };
                process99.Start();

                // Send the command to the cmd process
                process99.StandardInput.WriteLine(cmdCommand);
                process99.StandardInput.WriteLine("exit");

                // Read the output of the cmd process
                string output12 = process99.StandardOutput.ReadToEnd();

                Console.WriteLine(output12);

                process99.WaitForExit();

                // process99.Close();

                Thread.Sleep(5000);

                // Copy files from Black Mesa vpk extract files to Synergy directory
                string cmdCommand2 = $"/wait /min robocopy /S /NP /NJS /NJH /NS \"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\blackmesasupport\" \"{synpath}\\steamapps\\common\\Synergy\\synergy\"";

                ProcessStartInfo psi6 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process process2 = new Process { StartInfo = psi6 };
                process2.Start();

                // Send the command to the cmd process
                process2.StandardInput.WriteLine(cmdCommand);
                process2.StandardInput.WriteLine("exit");

                // Read the output of the cmd process
                string output2 = process2.StandardOutput.ReadToEnd();
                process2.WaitForExit();

                AnsiConsole.MarkupLine("[mediumorchid3]Удаление Python...[/]"); ;
                AnsiConsole.MarkupLine("");

                DeleteDirectory(pythonpatch);

                System.IO.File.Delete(pythonfile);

                Thread.Sleep(5000);

                AnsiConsole.MarkupLine("[mediumpurple2]Удаление PATCH PYTHON...[/]"); ;
                AnsiConsole.MarkupLine("");

                string script2 = $@"{func3}
                {func4}

Remove-Path-User -newPath ""{appdata}\App\Python""

Remove-Path-User -newPath ""{appdata}\App\Python\Scripts""

Remove-Path-Machine -newPath ""{appdata}\App\Python""

Remove-Path-Machine -newPath ""{appdata}\App\Python\Scripts""

";

                RunScript(script2);

                AnsiConsole.MarkupLine("[blue1]Произвожу удаление временных файлов...[/]");
                AnsiConsole.MarkupLine("");

                DeleteDirectory(extractvpk);

                System.IO.File.Delete(vpkPath);

                Thread.Sleep(5000);

                // Copy files from Black Mesa workshop content(custom floader) to Synergy directory
                Process robocopyProcess16 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "robocopy.exe",
                        Arguments = $@"/S /NP /NJS /NJH /NS ""{worshopinstalled}"" ""{blackMesaSynergyCustomPatch}""",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                robocopyProcess16.Start();
                robocopyProcess16.WaitForExit();

                // mklink workshop content => custom floader
                ProcessStartInfo psi0 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c mklink /j \"{blackMesaSynergyCustomPatch}\" \"{worshopinstalled}\"",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process.Start(psi0);

                AnsiConsole.MarkupLine("[red]Black Mesa установлена в Synergy Server.[/]");

                Thread.Sleep(5000);

                XAMPP.MenuS.StartMenu st = new MenuS.StartMenu();
                Console.Clear();
                st.Start();
            }
        }


        public void setup()
        {

            string synpath = XAMPP.SteamCMD.Direct.ServerInstallDirectory;
            string synergyexe = $"{synpath}\\steamapps\\common\\Synergy";
            string blackMesaSynergyPatch = $"{synpath}\\steamapps\\workshop\\content";
            string blackMesaSynergyCustomPatch = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom";
            string steamFloader = XAMPP.SteamCMD.Direct.SteamPatch;
            string blackMesaSupportPath = $"{steamFloader}\\steamapps\\workshop\\content\\17520";
            string worshopinstalled = $"{synpath}\\steamapps\\workshop\\content\\17520";
            string vpkPath = blackMesaSynergyCustomPatch + @"\1817140991\BlackMesaSupport.vpk";
            string extractvpk = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\blackmesasupport";
            string movevpkextract = $"{synpath}\\steamapps\\common\\Synergy\\synergy";
            string appdata = XAMPP.SteamCMD.Direct.bmsInstallerPath;
            string pythonfile = $"{appdata}\\App.zip";
            string pythonpatch = $"{appdata}\\App";
            string blackMesaSupportPath2 = $"{synpath}\\steamapps\\workshop\\content\\17520";


            // Copy files from Black Mesa workshop content to Synergy directory
            Process robocopyProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "robocopy.exe",
                    Arguments = $@"/S /NP /NJS /NJH /NS ""{blackMesaSupportPath2}"" ""{blackMesaSynergyPatch}""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            robocopyProcess.Start();
            robocopyProcess.WaitForExit();

            // Copy files from Black Mesa workshop content(custom floader) to Synergy directory
            Process robocopyProcess1 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "robocopy.exe",
                    Arguments = $@"/S /NP /NJS /NJH /NS ""{blackMesaSupportPath2}"" ""{blackMesaSynergyCustomPatch}""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            robocopyProcess1.Start();
            robocopyProcess1.WaitForExit();

            // mklink workshop content => custom floader
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c mklink /j \"{blackMesaSynergyCustomPatch}\" \"{blackMesaSupportPath2}\"",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process.Start(psi);


            AnsiConsole.MarkupLine("[blue1]Произвожу распаковку VPK файла...[/]");
            AnsiConsole.MarkupLine("");

            // Install Python

            AnsiConsole.MarkupLine("[blue1]Скачивание Python....[/]");
            AnsiConsole.MarkupLine("[blue1]Это может занять несколько минут в зависимости от скорости загрузки.[/]");
            AnsiConsole.MarkupLine("");

            string address1 = "https://github.com/TheLuxifer2023/bmsresource/releases/download/fd/App.zip";
            string address2 = "https://raw.githubusercontent.com/TheLuxifer2023/bmsresource/main/get-pip.py";
            string address3 = "https://raw.githubusercontent.com/TheLuxifer2023/bmsresource/main/get-pip2.py";

            string patch1 = $"{appdata}\\App.zip";
            string patch2 = $"{appdata}\\App\\Python\\get-pip.py";
            string patch3 = $"{appdata}\\App\\Python\\get-pip2.py";


            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                webClient.DownloadFile(new Uri(address1), patch1);
            }

            AnsiConsole.MarkupLine("[red]Распаковка....[/]");;
            AnsiConsole.MarkupLine("");

            using (ArchiveFile archiveFile = new ArchiveFile($@"{appdata}\App.zip"))
            {
                archiveFile.Extract(appdata); // extract Python
            };

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                webClient.DownloadFile(new Uri(address2), patch2);
                webClient.DownloadFile(new Uri(address3), patch3);
            }

            AnsiConsole.MarkupLine("[fuchsia]Применяю настройки для Python....[/]"); ;
            AnsiConsole.MarkupLine("");

            string func1 = @"function Add-Path-User($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""User"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if (-not $newPaths.Contains($newPath)) {
        $newPaths = @($newPath) + $newPaths
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""User"" )
    }
}";
            string func2 = @"function Add-Path-Machine($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""Machine"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if (-not $newPaths.Contains($newPath)) {
        $newPaths = @($newPath) + $newPaths
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""Machine"" )
    }
}";
            string func3 = @"function Remove-Path-Machine($pathToRemove) {
    $currentPath = [Environment]::GetVariable(""Path"", ""Machine"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if ($newPaths.Contains($pathToRemove)) {
        $newPaths = $newPaths | Where-Object { $_ -ne $pathToRemove }
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable(""Path"", $Path, ""Machine"")
    }
}";
            string func4 = @"function Remove-Path-User($newPath) {
    $currentPath = [Environment]::GetEnvironmentVariable(""PATH"", ""User"")
    $newPaths = $currentPath.Split([IO.Path]::PathSeparator)
    if ($newPaths.Contains($newPath)) {
        $newPaths = $newPaths | Where-Object { $_ -ne $newPath }
        $Path = $newPaths -join [IO.Path]::PathSeparator
        [Environment]::SetEnvironmentVariable( ""Path"", $Path, ""User"" )
    }
}";

            string script = $@"{func1}
                {func2}

Add-Path-User -newPath ""{appdata}\App\Python""

Add-Path-User -newPath ""{appdata}\App\Python\Scripts""

Add-Path-Machine -newPath ""{appdata}\App\Python""

Add-Path-Machine -newPath ""{appdata}\App\Python\Scripts""

";

            RunScript(script);

            AnsiConsole.MarkupLine("[red3]Если видите ошибки или WARING не обращаем внимание!!![/]"); ;
            AnsiConsole.MarkupLine("");

            string cmdCommand5 = $"{appdata}\\App\\Python\\python.exe \"{appdata}\\App\\Python\\get-pip.py\"";

            string cmdCommand6 = $"{appdata}\\App\\Python\\python.exe \"{appdata}\\App\\Python\\get-pip2.py\"";

            ProcessStartInfo psi8 = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process8 = new Process { StartInfo = psi8 };
            process8.Start();

            // Send the command to the cmd process
            process8.StandardInput.WriteLine(cmdCommand6);
            process8.StandardInput.WriteLine("exit");

            Thread.Sleep(3000);

            ProcessStartInfo psi7 = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process7 = new Process { StartInfo = psi7 };
            process7.Start();

            // Send the command to the cmd process
            process7.StandardInput.WriteLine(cmdCommand5);
            process7.StandardInput.WriteLine("exit");

            // Extract VPK using VPK Python Module

            // Read the output of the cmd process
            string output8 = process7.StandardOutput.ReadToEnd();
            process7.WaitForExit();

            string synpatchPath = $"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\BlackMesaSupport"; // replace with the actual path to the synpatch folder

            string cmdCommand = $"{appdata}\\App\\Python\\Scripts\\vpk.exe \"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\BlackMesaSupport.vpk\" -x \"{synpatchPath}\"";

            ProcessStartInfo psi5 = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process99 = new Process { StartInfo = psi5 };
            process99.Start();

            // Send the command to the cmd process
            process99.StandardInput.WriteLine(cmdCommand);
            process99.StandardInput.WriteLine("exit");

            // Read the output of the cmd process
            string output12 = process99.StandardOutput.ReadToEnd();

            Console.WriteLine(output12);

            process99.WaitForExit();

           // process99.Close();

            Thread.Sleep(5000);

            // Copy files from Black Mesa vpk extract files to Synergy directory
            string cmdCommand2 = $"/wait /min robocopy /S /NP /NJS /NJH /NS \"{synpath}\\steamapps\\common\\Synergy\\synergy\\custom\\1817140991\\blackmesasupport\" \"{synpath}\\steamapps\\common\\Synergy\\synergy\"";

            ProcessStartInfo psi6 = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process2 = new Process { StartInfo = psi6 };
            process2.Start();

            // Send the command to the cmd process
            process2.StandardInput.WriteLine(cmdCommand);
            process2.StandardInput.WriteLine("exit");

            // Read the output of the cmd process
            string output2 = process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();

            AnsiConsole.MarkupLine("[mediumorchid3]Удаление Python...[/]"); ;
            AnsiConsole.MarkupLine("");

            DeleteDirectory(pythonpatch);

            System.IO.File.Delete(pythonfile);

            Thread.Sleep(5000);

            AnsiConsole.MarkupLine("[mediumpurple2]Удаление PATCH PYTHON...[/]"); ;
            AnsiConsole.MarkupLine("");

            string script2 = $@"{func3}
                {func4}

Remove-Path-User -newPath ""{appdata}\App\Python""

Remove-Path-User -newPath ""{appdata}\App\Python\Scripts""

Remove-Path-Machine -newPath ""{appdata}\App\Python""

Remove-Path-Machine -newPath ""{appdata}\App\Python\Scripts""

";

            RunScript(script2);

            AnsiConsole.MarkupLine("[blue1]Произвожу удаление временных файлов...[/]");
            AnsiConsole.MarkupLine("");

            DeleteDirectory(extractvpk);

            System.IO.File.Delete(vpkPath);

            Thread.Sleep(5000);

            // Copy files from Black Mesa workshop content(custom floader) to Synergy directory
            Process robocopyProcess16 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "robocopy.exe",
                    Arguments = $@"/S /NP /NJS /NJH /NS ""{worshopinstalled}"" ""{blackMesaSynergyCustomPatch}""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            robocopyProcess16.Start();
            robocopyProcess16.WaitForExit();

            // mklink workshop content => custom floader
            ProcessStartInfo psi0 = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c mklink /j \"{blackMesaSynergyCustomPatch}\" \"{worshopinstalled}\"",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process.Start(psi0);

            AnsiConsole.MarkupLine("[red]Black Mesa установлена в Synergy Server.[/]");

            Thread.Sleep(5000);

            XAMPP.MenuS.StartMenu st = new MenuS.StartMenu();
            Console.Clear();
            st.Start();

        }

        static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }


        private static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("Error downloading file: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                Console.WriteLine("Download has been canceled.");
            }
            else
            {
                AnsiConsole.MarkupLine("[lime]Успешно скачанно.[/]");
                AnsiConsole.MarkupLine("");
            }
        }

        static void RunScript(string script)
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript(script);

                    using (PowerShell ps = PowerShell.Create())
                    {
                        ps.Runspace = runspace;
                        ps.Commands.AddScript(script);

                        ps.Invoke();
                    }
                }

                runspace.Close();
            }
        }

    }
}
    

