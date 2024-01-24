using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using Spectre.Console.Cli;

namespace ConsoleApp1.XAMPP.MenuS
{
    internal class test
    {

        public static bool IsOutputRedirected { get; }

        public static bool IsInputRedirected { get; }



        public async Task fdsAsync()
        {


            await AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Default)
            .StartAsync(
            "Connecting...", ctx => {
                // Simulate some work
                AnsiConsole.MarkupLine("Doing some work...");
                Thread.Sleep(1000);
                ctx.Refresh();

                Console.OutputEncoding = System.Text.Encoding.UTF8;

                ctx.Status("Thinking some more");
                ctx.Spinner(Spinner.Known.Clock);
                ctx.SpinnerStyle(Style.Parse("green"));

                ReadKey();
                return Task.CompletedTask;
            });

        }
    }
}
