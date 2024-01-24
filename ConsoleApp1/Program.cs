using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;
using ConsoleApp1.XAMPP;
using ShellProgressBar;
using ConsoleApp1.XAMPP.MenuS;
using static System.Console;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFOEX
        {
            internal int cbSize;
            internal int nFont;
            internal short FontSize;
            internal int FontFamily;
            internal int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            internal string FaceName;
        }

        const int STD_OUTPUT_HANDLE = -11;
        const int TMPF_TRUETYPE = 4;


        [STAThread]
        static void Main(string[] args)
        {

            IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

            CONSOLE_FONT_INFOEX info = new CONSOLE_FONT_INFOEX
            {
                cbSize = Marshal.SizeOf<CONSOLE_FONT_INFOEX>(),
                nFont = 0,
                FontSize = (12),
                FontFamily = TMPF_TRUETYPE,
                FontWeight = 0,
                FaceName = "Lucida Console" // Set to a font that supports the desired character
            };

            SetCurrentConsoleFontEx(consoleHandle, false, ref info);
;
            XAMPP.Disclamer dis = new Disclamer();;

            dis.Disclaim();


        }





    }
    }

