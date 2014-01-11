using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Input;

namespace Sudoku
{
    class CheckButton
    {
        public int number;
        public List<Button> potentialButtons;

        public CheckButton() 
        {
            this.number = 1;
            potentialButtons = new List<Button>();
        }

        public CheckButton(int number) 
        {
            this.number = number;
            potentialButtons = new List<Button>();
        }
    }
}
