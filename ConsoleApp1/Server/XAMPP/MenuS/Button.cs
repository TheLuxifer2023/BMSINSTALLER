using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.XAMPP.MenuS
{
    public class Button : MenuItem
    {
        //==================== ATRIBUTES ====================//

        //==================== CONSTRUCTOR ====================//

        public Button(string Title)
        {
            base.Title = Title;
        }


        //==================== METHODS ====================//

        public override string Show()
        {
            return Title;
        }

        public override bool Action()
        {
            return false;
        }
    }
}
