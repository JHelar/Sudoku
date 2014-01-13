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
        Button currButton;
        Button oldButton;

        public MainForm()
        {
            currButton = new Button();
            buttons = new List<List<List<List<Button>>>>();

            List<List<List<Button>>> buttonsRow = new List<List<List<Button>>>();
            List<List<Button>> buttonSquare = new List<List<Button>>();
            List<Button> buttonSquareRow = new List<Button>();
            Button tempButton = new Button();
            List<string> rsL = new List<string>();
            List<string> csL = new List<string>();

            MenuStrip ms = new MenuStrip();
            ms.Parent = this;

            ToolStripMenuItem file = new ToolStripMenuItem("&File");
            ToolStripMenuItem exit = new ToolStripMenuItem("&Exit", null, new EventHandler(OnExit));
            ToolStripMenuItem newGame = new ToolStripMenuItem("&New Game", null, new EventHandler(OnNewGame));
            ToolStripMenuItem solve = new ToolStripMenuItem("&Solve", null, new EventHandler(OnSolve));

            exit.ShortcutKeys = Keys.Alt | Keys.F4;
            newGame.ShortcutKeys = Keys.F2;
            solve.ShortcutKeys = Keys.F3;

            file.DropDownItems.Add(newGame);
            file.DropDownItems.Add(solve);
            file.DropDownItems.Add(exit);

            ms.Items.Add(file);
            MainMenuStrip = ms;

            InitializeComponent();
            this.BackColor = Color.Black;
            this.MaximizeBox = false;
            this.Text = "Sudoku Solver";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            rsL.Add("ABC");
            rsL.Add("DEF");
            rsL.Add("GHI");
            csL.Add("123");
            csL.Add("456");
            csL.Add("789");

            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            tempButton.Location = new Point((5 + k * 40 + (i * 40 * 3)) + i * 5, (35 + j * 40 + (l * 40 * 3)) + l * 5);
                            tempButton.Size = new Size(40, 40);
                            tempButton.Parent = this;
                            tempButton.Click += new EventHandler(button_click);
                            tempButton.Name = rsL[l][j].ToString() + csL[i][k].ToString();
                            tempButton.Text = "";
                            tempButton.BackColor = Color.White;
                            tempButton.Font = new Font("Arial", 10, FontStyle.Regular);
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
        }

        #region EventHandling
        void button_click(object sender, EventArgs e)
        {
            oldButton = new Button();
            oldButton = currButton;
            currButton = new Button();
            Button btn = (Button)sender;
            currButton = btn;
            oldButton.BackColor = Color.White;
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
                            buttons[l][i][j][k].Font = new Font("Arial", 10, FontStyle.Regular);
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
            currButton.Font = new Font("Arial", 10, FontStyle.Bold);
            return true;
        }
        
        #endregion

        #region Backtracking and Solving
        void solve()
        {
            BackTrackSolver BTS = new BackTrackSolver();
            if (setButtons(BTS.solve(buttons)))
                MessageBox.Show("Solve success!");
            else
                MessageBox.Show("Unsolvable problem!");
        }

        bool setButtons(Dictionary<string, string> values) 
        {
            bool breakFlag = new bool();

            if (values.Count == 0)
                return false;

            foreach (string key in values.Keys)
            {
                breakFlag = false;
                for (int i = 0; i < buttons.Count; i++)
                {
                    for (int j = 0; j < buttons[i].Count; j++)
                    {
                        for (int k = 0; k < buttons[i][j].Count; k++)
                        {
                            for (int l = 0; l < buttons[i][j][k].Count; l++)
                            {
                                if (buttons[i][j][k][l].Name == key)
                                {
                                    buttons[i][j][k][l].Text = values[key];
                                    breakFlag = true;
                                    break;
                                }
                            }
                            if (breakFlag)
                                break;
                        }
                        if (breakFlag)
                            break;
                    }
                    if (breakFlag)
                        break;
                }
            }
            return true;
        }
        #endregion
    }

}
