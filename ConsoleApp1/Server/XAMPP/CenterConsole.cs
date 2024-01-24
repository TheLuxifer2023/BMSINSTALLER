using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP
{
    internal class CenterConsole
    {
        public void center()
        {
            // Get the console window size
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;

            // Calculate the center position
            int centerX = windowWidth / 2;
            int centerY = windowHeight / 2;

            // Move the cursor to the center position
            Console.SetCursorPosition(centerX, centerY);
        }
    }
}
