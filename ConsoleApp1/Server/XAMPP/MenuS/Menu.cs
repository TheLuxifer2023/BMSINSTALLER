using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApp1.XAMPP.MenuS
{
    public class ConsoleMenu
    {
        //==================== ATRIBUTES ====================//

        //list of console components
        private List<MenuItem> MenuItems = new List<MenuItem>();

        //header of menu
        private string MenuHeader;

        //index of activated menu item
        private int ActiveItem = 0;

        //if cursor is on bottom then cursor will go on top
        private bool MenuLoop;

        //color defintions
        public ConsoleColor TextColor = ConsoleColor.White;
        public ConsoleColor HighlightBackground = ConsoleColor.Green;
        public ConsoleColor HighlightText = ConsoleColor.Black;


        //==================== CONSTRUCTOR ====================//

        public ConsoleMenu(string MenuHeader, bool MenuLoop = false)
        {
            this.MenuHeader = MenuHeader;
            this.MenuLoop = MenuLoop;
        }


        //==================== METHODS ====================//

        //add new menu item into menu
        public void AddItem(MenuItem Item)
        {
            MenuItems.Add(Item);
        }

        //opens teh menu and returns index of pressed button
        public int Display()
        {

            //true when menu is opened
            bool Running = true;

            //set console
            Console.Clear();
            Console.CursorVisible = true;

            //pressed button
            ConsoleKeyInfo btn;

            while (Running)
            {

                //set console colores
                Console.SetCursorPosition(0, 0);
                Console.ResetColor();
                Console.ForegroundColor = TextColor;

                //print menu header
                Console.WriteLine(MenuHeader + "\n");

                Console.ResetColor();

                //list and print all menu items
                for (int i = 0; i < MenuItems.Count; i++)
                {

                    //it item is active, then print highlighted item
                    if (i == ActiveItem)
                    {
                        Console.ForegroundColor = HighlightText;
                        Console.BackgroundColor = HighlightBackground;
                        Console.Write(MenuItems[i].Show().PadRight(Console.WindowWidth));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = TextColor;
                        Console.Write(MenuItems[i].Show().PadRight(Console.WindowWidth));
                        Console.ResetColor();
                    }

                }

                btn = Console.ReadKey();

                switch (btn.Key)
                {

                    //activate item on the activeted item
                    case ConsoleKey.UpArrow:
                        if (ActiveItem > 0)
                        {
                            ActiveItem--;
                        }
                        else
                        {
                            if (MenuLoop)
                            {
                                //go to bottom of menu
                                ActiveItem = MenuItems.Count - 1;
                            }
                            else
                            {
                                //stay on top
                                ActiveItem = 0;
                            }
                        }
                        break;

                    //activate item under the activeted item
                    case ConsoleKey.DownArrow:
                        if (ActiveItem < MenuItems.Count - 1)
                        {
                            ActiveItem++;
                        }
                        else
                        {
                            if (MenuLoop)
                            {
                                //go to top of menu
                                ActiveItem = 0;
                            }
                            else
                            {
                                //stay on bottom
                                ActiveItem = MenuItems.Count - 1;
                            }
                        }
                        break;

                    //activete item when enter is pressed
                    case ConsoleKey.Enter:
                        Console.ForegroundColor = HighlightText;
                        Console.BackgroundColor = HighlightBackground;
                        Running = MenuItems[ActiveItem].Action();
                        Console.ResetColor();
                        break;

                    default:
                        break;
                }

            }
            Console.CursorVisible = true;

            return ActiveItem;
        }

        public int Display2()
        {

            //true when menu is opened
            bool Running = true;

            //set console
            Console.CursorVisible = true;

            //pressed button
            ConsoleKeyInfo btn;

            while (Running)
            {


                //list and print all menu items
                for (int i = 0; i < MenuItems.Count; i++)
                {

                    //it item is active, then print highlighted item
                    if (i == ActiveItem)
                    {
                        //Console.ForegroundColor = HighlightText;
                        //Console.BackgroundColor = HighlightBackground;
                        Console.Write(MenuItems[i].Show().PadRight(Console.WindowWidth));
                        //Console.ResetColor();
                    }
                    else
                    {
                        //Console.ForegroundColor = TextColor;
                        Console.Write(MenuItems[i].Show().PadRight(Console.WindowWidth));
                        //Console.ResetColor();
                    }

                }

                btn = Console.ReadKey();

                switch (btn.Key)
                {

                    //activate item on the activeted item
                    case ConsoleKey.UpArrow:
                        if (ActiveItem > 0)
                        {
                            ActiveItem--;
                        }
                        else
                        {
                            if (MenuLoop)
                            {
                                //go to bottom of menu
                                ActiveItem = MenuItems.Count - 1;
                            }
                            else
                            {
                                //stay on top
                                ActiveItem = 0;
                            }
                        }
                        break;

                    //activate item under the activeted item
                    case ConsoleKey.DownArrow:
                        if (ActiveItem < MenuItems.Count - 1)
                        {
                            ActiveItem++;
                        }
                        else
                        {
                            if (MenuLoop)
                            {
                                //go to top of menu
                                ActiveItem = 0;
                            }
                            else
                            {
                                //stay on bottom
                                ActiveItem = MenuItems.Count - 1;
                            }
                        }
                        break;

                    //activete item when enter is pressed
                    case ConsoleKey.Enter:
                        Console.ForegroundColor = HighlightText;
                        Console.BackgroundColor = HighlightBackground;
                        Running = MenuItems[ActiveItem].Action();
                        Console.ResetColor();
                        break;

                    default:
                        break;
                }

            }
            Console.CursorVisible = true;

            return ActiveItem;
        }

        //returns data from all menu items
        public ArrayList GetData()
        {
            ArrayList MenuData = new ArrayList();

            for (int i = 0; i < MenuItems.Count; i++)
            {
                //some items don't have data (e.g. Button)
                try
                {
                    MenuData.Add(MenuItems[i].Data[0]);
                }
                catch
                {
                    MenuData.Add(null);
                }
            }

            return MenuData;
        }

    }
}
