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
    class Constraints
    {
        public bool checkConstraints(List<List<List<List<Button>>>> buttons, Button one, Dictionary<string, CheckButton> assignment)
        {
            List<List<List<List<Button>>>> tempButtons = new List<List<List<List<Button>>>>(buttons);
            addAssignment(tempButtons, assignment);
            if (!this.checkSquare(this.getSquare(tempButtons, one), one))
                return false;
            if (!this.checkRow(this.getRow(tempButtons, one), one))
                return false;
            if (!this.checkCollumn(this.getCollumn(tempButtons, one), one))
                return false;
            if (!this.checkValid(tempButtons, one))
                return false;
            return true;
        }

        private void addAssignment(List<List<List<List<Button>>>> tempButtons, Dictionary<string, CheckButton> assignment)
        {
            foreach (string location in assignment.Keys)
            {
                string[] tl = location.Split(':');
                tempButtons[Convert.ToInt32(tl[0])][Convert.ToInt32(tl[1])][Convert.ToInt32(tl[2])][Convert.ToInt32(tl[3])].Text = Convert.ToString(assignment[location].number);
            }
        }
        private List<List<Button>> getSquare(List<List<List<List<Button>>>> buttons, Button one) 
        {
            string[] oneInfo = one.Name.Split(':');
            return buttons[Convert.ToInt32(oneInfo[0])][Convert.ToInt32(oneInfo[1])];
        }

        private List<Button> getRow(List<List<List<List<Button>>>> buttons, Button one) 
        {
            List<Button> returnRow = new List<Button>();
            List<List<List<Button>>> buttonSquareRow = new List<List<List<Button>>>();
            string[] oneInfo = one.Name.Split(':');

            buttonSquareRow = buttons[Convert.ToInt32(oneInfo[0])];
            foreach (List<List<Button>> btnSquare in buttonSquareRow) 
            {
                foreach (Button btn in btnSquare[Convert.ToInt32(oneInfo[2])]) 
                {
                    returnRow.Add(btn);
                }
            }
            return returnRow;
        }

        private List<Button> getCollumn(List<List<List<List<Button>>>> buttons, Button one) 
        {
            List<Button> returnCollumn = new List<Button>();
            List<List<Button>> square = new List<List<Button>>();
            string[] oneInfo = one.Name.Split(':');

            foreach (List<List<List<Button>>> btnSquareRow in buttons) 
            {
                square = btnSquareRow[Convert.ToInt32(oneInfo[1])];
                foreach (List<Button> btnL in square) 
                {
                    returnCollumn.Add(btnL[Convert.ToInt32(oneInfo[3])]);
                }
            }
            return returnCollumn;
        }

        private bool checkSquare(List<List<Button>> square, Button one) 
        {
            foreach (List<Button> btnL in square) 
            {
                foreach (Button btn in btnL) 
                {
                    if (btn.Text == one.Text)
                        return false;
                }
            }
            return true;
        }

        private bool checkRow(List<Button> row, Button one) 
        {
            foreach (Button btn in row) 
            {
                if (btn.Text == one.Text)
                    return false;
            }
            return true;
        }

        private bool checkCollumn(List<Button> collumn, Button one) 
        {
            foreach (Button btn in collumn) 
            {
                if (btn.Text == one.Text)
                    return false;
            }
            return true;
        }

        private bool checkValid(List<List<List<List<Button>>>> buttons, Button one) 
        {
            string[] oneInfo = one.Name.Split(':');
            if (buttons[Convert.ToInt32(oneInfo[0])][Convert.ToInt32(oneInfo[1])][Convert.ToInt32(oneInfo[2])][Convert.ToInt32(oneInfo[3])].Text != "")
                return false;
            else
                return true;
        }
    }
}
