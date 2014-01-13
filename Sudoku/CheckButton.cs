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
        public List<string> potential;
        public string name;
        public string buttonId;

        public CheckButton() 
        {
            potential = new List<string>();
        }
    }
}
