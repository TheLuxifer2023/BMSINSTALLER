using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1.XAMPP.Steam
{
    internal class InputPass
    {
        public static void SaveStringToFile(string stringToSave, string filePath)
        {
            int lineNumber = 1;
            AppendLineToFile(filePath, stringToSave, lineNumber);
        }


        public static void AppendLineToFile(string filePath, string lineToAppend, int lineNumber)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Ensure that the line number is valid
            if (lineNumber < 0 || lineNumber > lines.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number is out of range.");
            }

            // Check if the specified line already exists
            if (lineNumber < lines.Length && lines[lineNumber] == lineToAppend)
            {
                // If it does, replace its content
                lines[lineNumber] = lineToAppend;
            }
            else
            {
                // Otherwise, insert the new line at the specified position
                Array.Resize(ref lines, lines.Length + 1);
                for (int i = lines.Length - 1; i > lineNumber; i--)
                {
                    lines[i] = lines[i - 1];
                }
                lines[lineNumber] = lineToAppend;
            }

            // Convert the modified array back to a single string
            string fileContent = string.Join(Environment.NewLine, lines);

            // Overwrite the file with the modified content
            File.WriteAllText(filePath, fileContent);
        }

    }
}
