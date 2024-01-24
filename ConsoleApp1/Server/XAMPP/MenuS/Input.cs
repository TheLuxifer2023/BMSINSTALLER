using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1.XAMPP.MenuS
{
    public class Input : MenuItem
    {
        //==================== ATRIBUTES ====================//

        private const int IdentationSize = 2;

        //input data that is returned and displayed in console
        public string InputData = "";

        //cursor position
        private int CTop = 0;

        //==================== CONSTRUCTORS ====================//

        public Input(string Title, string InputData = "")
        {
            base.Title = Title;
            this.InputData = InputData;

            base.Data.Add(InputData);
        }

        //==================== METHODS ====================//

        //print componet into console
        public override string Show()
        {
            CTop = Console.CursorTop;

            return Title + ": " + InputData;

        }
        [STAThread]
        public override bool Action()
        {
            Console.CursorVisible = true;

            Console.SetCursorPosition(Title.Length + IdentationSize, CTop);

            Console.SetCursorPosition(Title.Length + IdentationSize, CTop);

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                InputData = folderPath;
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string bmsInstallerPath = Path.Combine(appDataPath, "BMSINSTALLER");
                string settingsFilePath = Path.Combine(bmsInstallerPath, "settings.dat");

                if (!File.Exists("settings.dat"))
                {
                    File.WriteAllText(settingsFilePath, @"test
test
");
                }

                SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

                Task SaveStringToFileAsync(string stringToSave, string filePath)
                {
                    return Task.Run(async () => {
                        await semaphore.WaitAsync();
                        try
                        {
                            AppendLineToFile(filePath, stringToSave, 0);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });
                }

                SaveStringToFileAsync(folderPath, settingsFilePath);


            }


            Console.CursorVisible = false;
            Data[0] = InputData;
            return true;

        }


        static void SaveStringToFile(string stringToSave, string filePath)
        {
            int lineNumber = 0;
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
                // Otherwise, insert the new line at the specified position
                List<string> list = new List<string>(lines);
                list.Insert(lineNumber, lineToAppend);
                lines = list.ToArray();
            }
            else
            {
                // If it does, replace its content
                lines[lineNumber] = lineToAppend;
            }

            // Convert the modified array back to a single string
            string fileContent = string.Join(Environment.NewLine, lines);

            // Overwrite the file with the modified content
            File.WriteAllText(filePath, fileContent);
        }

    }

}
