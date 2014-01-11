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
    public static class ExtensionMethods 
    {
        public static T Pop<T>(this List<T> theList) 
        {
            var local = theList[theList.Count - 1];
            theList.RemoveAt(theList.Count - 1);
            return local;
        }
    };

    public partial class MainForm : Form
    {
        List<List<List<List<Button>>>> buttons;
        List<Button> buttonsLeft;
        List<List<CheckButton>> checkButtons;
        Button currButton;
        Button oldButton;

        public MainForm()
        {
            currButton = new Button();

            checkButtons = new List<List<CheckButton>>();
            buttons = new List<List<List<List<Button>>>>();
            buttonsLeft = new List<Button>();
            List<List<List<Button>>> buttonsRow = new List<List<List<Button>>>();
            List<List<Button>> buttonSquare = new List<List<Button>>();
            List<Button> buttonSquareRow = new List<Button>();
            Button tempButton = new Button();

            MenuStrip ms = new MenuStrip();
            ms.Parent = this;

            ToolStripMenuItem file = new ToolStripMenuItem("&File");
            ToolStripMenuItem exit = new ToolStripMenuItem("&Exit", null, new EventHandler(OnExit));
            ToolStripMenuItem newGame = new ToolStripMenuItem("&New Game", null, new EventHandler(OnNewGame));
            ToolStripMenuItem solve = new ToolStripMenuItem("&Solve", null, new EventHandler(OnSolve));

            file.DropDownItems.Add(newGame);
            file.DropDownItems.Add(solve);
            file.DropDownItems.Add(exit);

            ms.Items.Add(file);
            MainMenuStrip = ms;

            InitializeComponent();
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            tempButton.Location = new Point((k * 40 + (i * 40 * 3)) + i * 5, (35 + j * 40 + (l * 40 * 3)) + l * 5);
                            tempButton.Size = new Size(40, 40);
                            tempButton.Parent = this;
                            tempButton.Click += new EventHandler(button_click);
                            tempButton.Name = l + ":" + i + ":" + j + ":" + k;
                            tempButton.Text = "";
                            buttonSquareRow.Add(tempButton);
                            tempButton = new Button();
                        }
                        buttonSquare.Add(buttonSquareRow);
                        buttonSquareRow = new List<Button>();
                    }
                    buttonsRow.Add(buttonSquare);
                    buttonSquare = new List<List<Button>>();
                }
                buttons.Add(buttonsRow);
                buttonsRow = new List<List<List<Button>>>();
            }
            List<CheckButton> cbl = new List<CheckButton>();
            for (int i = 1; i <= 9; i++) 
            {
                for (int j = 0; j < 9; j++) 
                {
                    cbl.Add(new CheckButton(i));
                }
                checkButtons.Add(cbl);
                cbl = new List<CheckButton>();
            }
        }

        #region EventHandling
        void button_click(object sender, EventArgs e)
        {
            oldButton = new Button();
            oldButton = currButton;
            currButton = new Button();
            Button btn = (Button)sender;
            currButton = btn;
            oldButton.BackColor = Color.Transparent;
            currButton.BackColor = Color.Red;
        }

        void OnNewGame(object sender, EventArgs e)
        {
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            buttons[l][i][j][k].Text = "";
                        }
                    }
                }
            }
        }

        void OnSolve(object sender, EventArgs e)
        {
            this.solve();
        }

        void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.D1)
                currButton.Text = "1";
            else if (keyData == Keys.D2)
                currButton.Text = "2";
            else if (keyData == Keys.D3)
                currButton.Text = "3";
            else if (keyData == Keys.D4)
                currButton.Text = "4";
            else if (keyData == Keys.D5)
                currButton.Text = "5";
            else if (keyData == Keys.D6)
                currButton.Text = "6";
            else if (keyData == Keys.D7)
                currButton.Text = "7";
            else if (keyData == Keys.D8)
                currButton.Text = "8";
            else if (keyData == Keys.D9)
                currButton.Text = "9";
            else if (keyData == Keys.Back)
                currButton.Text = "";
            return true;
        }
        
        #endregion

        #region Backtracking and solving
        void solve()
        {
            Constraints csp = new Constraints();
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons[i].Count; j++)
                {
                    for (int k = 0; k < buttons[i][j].Count; k++)
                    {
                        for (int l = 0; l < buttons[i][j][k].Count; l++)
                        {
                            if (buttons[i][j][k][l].Text == "")
                                buttonsLeft.Add(buttons[i][j][k][l]);
                            else
                                checkButtons[Convert.ToInt32(buttons[i][j][k][l].Text) - 1].RemoveAt(checkButtons[Convert.ToInt32(buttons[i][j][k][l].Text) - 1].Count - 1);
                        }
                    }
                }
            }
            Dictionary<string, CheckButton> assignment = new Dictionary<string, CheckButton>();
            backtrack(csp, assignment);
            setAssignment(assignment);
        }

        List<Button> buttonList(CheckButton CB, Constraints csp, Dictionary<string, CheckButton> assignment)
        {
            List<Button> returnList = new List<Button>();
            Button tempBtn = new Button();

            foreach (Button btnLeft in buttonsLeft)
            {
                tempBtn.Name = btnLeft.Name;
                tempBtn.Text = Convert.ToString(CB.number);
                if (csp.checkConstraints(buttons, tempBtn, assignment))
                {
                    returnList.Add(btnLeft);
                }
                tempBtn = new Button();
            }
            return returnList;
        }

        void assign(CheckButton CB, Button btn, Dictionary<string, CheckButton> ass)
        {
            ass[btn.Name] = CB;
            buttonsLeft.Remove(btn);
        }

        void unAssign(Button btn, Dictionary<string, CheckButton> ass)
        {
            ass.Remove(btn.Name);
            buttonsLeft.Add(btn);
        }

        void findPotentials(Constraints csp, Dictionary<string, CheckButton> ass)
        {
            for (int i = 0; i < checkButtons.Count; i++)
            {
                foreach (CheckButton potential in checkButtons[i])
                {
                    potential.potentialButtons = buttonList(potential, csp, ass);
                }
            }
        }

        CheckButton mostConstrainedVariable(Constraints csp, Dictionary<string, CheckButton> ass)
        {
            int count = 100000;
            int cbRowIndx = 0;
            CheckButton CBToReturn = new CheckButton();
            findPotentials(csp, ass);

            for (int i = 0; i < checkButtons.Count; i++)
            {
                foreach (CheckButton CB in checkButtons[i])
                {
                    if (CB.potentialButtons.Count < count)
                    {
                        count = CB.potentialButtons.Count;
                        CBToReturn = CB;
                        cbRowIndx = i;
                    }
                }
            }
            checkButtons[cbRowIndx].Remove(CBToReturn);
            return CBToReturn;
        }

        Button leastConstrainingVal(CheckButton CB, Constraints csp, Dictionary<string, CheckButton> ass)
        {
            Button btnToReturn = new Button();
            Dictionary<string, CheckButton> tempAss = new Dictionary<string,CheckButton>(ass);
            btnToReturn.Text = "None";
            int bestBtnCount = 0;
            int possibleBtnCount = 0;

            if (CB.potentialButtons.Count == 0)
                return btnToReturn;
            else if (buttonsLeft.Count == 0)
                return CB.potentialButtons.Pop();
            foreach (Button pbtn in CB.potentialButtons)
            {
                possibleBtnCount = 0;
                assign(CB, pbtn, tempAss);
                for (int i = 0; i < checkButtons.Count; i++)
                {
                    foreach (CheckButton affectedBtn in checkButtons[i])
                    {
                        List<Button> tempAffectBtn = buttonList(affectedBtn, csp, tempAss);
                        possibleBtnCount += tempAffectBtn.Count;
                        if (possibleBtnCount > bestBtnCount)
                        {
                            bestBtnCount = possibleBtnCount;
                            btnToReturn = pbtn;
                            btnToReturn.Text = "";
                        }
                    }
                }
                unAssign(pbtn,tempAss);
            }
            if (bestBtnCount == 0)
                return btnToReturn;
            CB.potentialButtons.Remove(btnToReturn);
            return btnToReturn;
        }

        bool backtrack(Constraints csp, Dictionary<string, CheckButton> assignment)
        {
            if (checkButtonsEmpty())
                return true;
            CheckButton currCB = mostConstrainedVariable(csp, assignment);
            Button currBtn = leastConstrainingVal(currCB, csp, assignment);
            Button prevCurrBtn = currBtn;
            while (currBtn.Text != "None")
            {
                assign(currCB, currBtn, assignment);
                bool result = backtrack(csp, assignment);
                if (!result)
                    return result;
                unAssign(currBtn, assignment);
                prevCurrBtn = currBtn;
                currBtn = leastConstrainingVal(currCB, csp, assignment);
            }
            if(prevCurrBtn.Text != "None")
                checkButtons[Convert.ToInt32(prevCurrBtn.Text) - 1].Add(currCB);
            return false;
        }

        bool checkButtonsEmpty() 
        {
            for (int i = 0; i < checkButtons.Count; i++) 
            {
                if (checkButtons[i].Count != 0)
                    return false;
            }
            return true;
        }

        void setAssignment(Dictionary<string, CheckButton> assignment) 
        {
            foreach (string location in assignment.Keys)
            {
                string[] tl = location.Split(':');
                buttons[Convert.ToInt32(tl[0])][Convert.ToInt32(tl[1])][Convert.ToInt32(tl[2])][Convert.ToInt32(tl[3])].Text = Convert.ToString(assignment[location].number);
            }
        }
        #endregion
    }

}
