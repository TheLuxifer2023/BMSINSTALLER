using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP
{
    internal class Disclamer
    {
        public void Disclaim()
        {

            XAMPP.MenuS.StartMenu c = new MenuS.StartMenu();

            var sb = new StringBuilder();


                sb.AppendLine("[red] ДИСКЛЕЙМОР[/]");
                sb.AppendLine();
                sb.AppendLine("[blue] Программа не крадет ваши пароли от стима! [/]");
                sb.AppendLine("[green] Логин и пароль от стима нужны для SteamCMD [/]");
                sb.AppendLine("[purple] Программа выпускается с открытым кодом. [/]");

            // Create the layout
            var layout = new Layout("Root")
                .SplitColumns(
                    new Layout("Left"));

            // Update the left column
            layout["Left"].Update(
                new Panel(
                    Align.Center(
                        new Markup(sb.ToString()),
                     VerticalAlignment.Middle))
                    .Expand());

            // Render the layout
            AnsiConsole.Write(layout);

            if (!AnsiConsole.Confirm("[Red]Если согласны напишите 'y' и нажмите 'Enter'[/]"))
            {
                AnsiConsole.MarkupLine("Ok... :(");
                return;
            }

            goto continueLabel;

        continueLabel:
            c.Start();

        }
    }
}
