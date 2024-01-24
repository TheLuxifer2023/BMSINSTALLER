using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP.MenuS
{
    public class Switcher : MenuItem
    {
        //==================== ATRIBUTES ====================//

        private bool State;
        private string OnText;
        private string OffText;


        //==================== CONSTRUCTOR ====================//

        public Switcher(string Title, bool State = false, string OnText = "on", string OffText = "off")
        {
            base.Data.Add(State);
            base.Title = Title;
            this.State = State;
            this.OnText = OnText;
            this.OffText = OffText;
        }


        //==================== METHODS ====================//

        //print componet into console
        public override string Show()
        {
            if (State)
            {
                return Title + ": " + OnText;
            }
            else
            {
                return Title + ": " + OffText;
            }
        }

        //action when enter is pressed
        public override bool Action()
        {
            State = !State;
            Data[0] = State;
            return true;
        }
    }
}
