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
        Dictionary<string, string> solution;
        Button currButton;
        Button oldButton;
        Form newGameForm;
        RadioButton prevRbtn;
        ToolTip TT;
        string difficulty;
        bool newGameTrigger;

        public MainForm()
        {
            TT = new ToolTip();
            currButton = new Button();
            buttons = new List<List<List<List<Button>>>>();
            newGameForm = new Form();
            newGameTrigger = new bool();
            newGameTrigger = false;
            prevRbtn = new RadioButton();

            List<List<List<Button>>> buttonsRow = new List<List<List<Button>>>();
            List<List<Button>> buttonSquare = new List<List<Button>>();
            List<Button> buttonSquareRow = new List<Button>();
            Button tempButton = new Button();
            List<string> rsL = new List<string>();
            List<string> csL = new List<string>();

            RadioButton superEasy = new RadioButton();
            RadioButton easy = new RadioButton();
            RadioButton normal = new RadioButton();
            RadioButton hard = new RadioButton();
            Button ok = new Button();
            Button cancel = new Button();

            MenuStrip ms = new MenuStrip();
            ms.Parent = this;

            ToolStripMenuItem file = new ToolStripMenuItem("&File");
            ToolStripMenuItem exit = new ToolStripMenuItem("&Exit", null, new EventHandler(OnExit));
            ToolStripMenuItem newGame = new ToolStripMenuItem("&New Game", null, new EventHandler(OnNewGame));
            ToolStripMenuItem solve = new ToolStripMenuItem("&Solve", null, new EventHandler(OnSolve));
            ToolStripMenuItem cleanBoard = new ToolStripMenuItem("&Clean Board", null, new EventHandler(OnClean));

            exit.ShortcutKeys = Keys.Alt | Keys.F4;
            newGame.ShortcutKeys = Keys.F2;
            solve.ShortcutKeys = Keys.F3;
            cleanBoard.ShortcutKeys = Keys.F1;

            file.DropDownItems.Add(newGame);
            file.DropDownItems.Add(solve);
            file.DropDownItems.Add(cleanBoard);
            file.DropDownItems.Add(exit);

            ms.Items.Add(file);
            MainMenuStrip = ms;

            InitializeComponent();
            this.BackColor = Color.Black;
            this.MaximizeBox = false;
            this.Text = "Sudoku Solver";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            newGameForm.Size = new System.Drawing.Size(220, 200);
            newGameForm.BackColor = Color.White;
            newGameForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            newGameForm.MinimizeBox = false;
            newGameForm.Hide();

            superEasy.Parent = newGameForm;
            superEasy.Location = new Point(30, 20);
            superEasy.Text = "Super easy";
            superEasy.Click += new EventHandler(OnCheck);
            superEasy.MouseHover += new EventHandler(OnHover);

            easy.Parent = newGameForm;
            easy.Location = new Point(30, 40);
            easy.Text = "Easy";
            easy.Click += new EventHandler(OnCheck);
            easy.MouseHover += new EventHandler(OnHover);

            normal.Parent = newGameForm;
            normal.Location = new Point(30, 60);
            normal.Text = "Normal";
            normal.Click += new EventHandler(OnCheck);
            normal.MouseHover += new EventHandler(OnHover);

            hard.Parent = newGameForm;
            hard.Location = new Point(30, 80);
            hard.Text = "Hard";
            hard.Click += new EventHandler(OnCheck);
            hard.MouseHover += new EventHandler(OnHover);

            ok.Parent = newGameForm;
            ok.Location = new Point(20, 120);
            ok.Text = "Ok";
            ok.Click += new EventHandler(OnOk);

            cancel.Parent = newGameForm;
            cancel.Location = new Point(100, 120);
            cancel.Text = "Cancel";
            cancel.Click += new EventHandler(OnCancel);

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
            currButton.BackColor = Color.LightPink;
        }

        void OnHover(object sender, EventArgs e) 
        {
            TT.BackColor = Color.White;
            TT.ForeColor = Color.Black;
            TT.Active = true;
            RadioButton rbtn = (RadioButton)sender;

            if (rbtn.Text == "Super easy" && prevRbtn != rbtn)
            {
                TT.Hide(newGameForm);
                TT.Show("Sets a new game of sudoku with 5+/- predetermined numbers in each 3x3 square. Also indicates if a given number is a valid number", newGameForm, new Point(60, 30));
            }
            else if (rbtn.Text == "Easy")
            {
                TT.Hide(newGameForm);
                TT.Show("Sets a new game of sudoku with 5+/- predetermined numbers in each 3x3 square.", newGameForm, new Point(60, 50));
            }
            else if (rbtn.Text == "Normal")
            {
                TT.Hide(newGameForm);
                TT.Show("Sets a new game of sudoku with 3+/- predetermined numbers in each 3x3 square.", newGameForm, new Point(60, 70));
            }
            else if (rbtn.Text == "Hard")
            {
                TT.Hide(newGameForm);
                TT.Show("Sets a new game of sudoku with 2+/- predetermined numbers in each 3x3 square.", newGameForm, new Point(60, 90));
            }
            prevRbtn = rbtn;
        }

        void OnNewGame(object sender, EventArgs e)
        {
            newGameForm.Show();
            newGameTrigger = true;

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
                            buttons[l][i][j][k].ForeColor = Color.Black;
                        }
                    }
                }
            }
        }

        void OnCheck(object sender, EventArgs e) 
        {
            RadioButton rbtn = (RadioButton)sender;
            difficulty = rbtn.Text;
        }

        void OnOk(object sender, EventArgs e) 
        {
            solve(difficulty);
            newGameForm.Hide();
        }

        void OnCancel(object sender, EventArgs e) 
        {
            newGameForm.Hide();
            newGameTrigger = false;
            return;
        }

        void OnClean(object sender, EventArgs e) 
        {
            newGameTrigger = false;
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
                            buttons[l][i][j][k].ForeColor = Color.Black;
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
            if (currButton.ForeColor == Color.DarkBlue)
                return true;
            else if (keyData == Keys.D1)
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
            currButton.ForeColor = Color.Black;
            if (difficulty == "Super easy")
                assistant();
            return true;
        }
        
        #endregion

        #region Backtracking, Solving and help
        void assistant() 
        {
            BackTrackSolver BTS = new BackTrackSolver();
            for(int i = 0; i < buttons.Count; i++)
                for(int j = 0; j < buttons[i].Count; j++)
                    for(int k = 0; k < buttons[i][j].Count; k++)
                        for (int l = 0; l < buttons[i][j][k].Count; l++) 
                        {
                            if (BTS.Peers()[currButton.Name].Contains(buttons[i][j][k][l].Name))
                                if (buttons[i][j][k][l].Text == currButton.Text)
                                    currButton.ForeColor = Color.Red;
                        }
        }
        void solve(string difficulty) 
        {
            BackTrackSolver BTS = new BackTrackSolver();
            Random randN = new Random();
            Random rand = new Random();

            solution = new Dictionary<string, string>();
            
            int col = 0;
            int row = 0;

            bool done = new bool();
            done = false;
            if (difficulty == "Easy" || difficulty == "Super easy")
            {
                while (!done)
                {
                    buttons[rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)].Text = randN.Next(1, 10).ToString();
                    solution = BTS.solve(buttons);
                    done = setButtons(solution);
                }
                for (int i = 0; i < buttons.Count; i++) 
                {
                    for (int j = 0; j < buttons[i].Count; j++) 
                    {
                        for (int count = 0; count < 7; count++) 
                        {
                            row = rand.Next(0, 3);
                            col = rand.Next(0, 3);

                            buttons[i][j][row][col].Text = "";
                        }
                    }
                }
            }
            else if (difficulty == "Normal") 
            {
                while (!done)
                {
                    buttons[rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)].Text = randN.Next(1, 10).ToString();
                    solution = BTS.solve(buttons);
                    done = setButtons(solution);
                }
                for (int i = 0; i < buttons.Count; i++)
                {
                    for (int j = 0; j < buttons[i].Count; j++)
                    {
                        for (int count = 0; count < 10; count++)
                        {
                            row = rand.Next(0, 3);
                            col = rand.Next(0, 3);

                            buttons[i][j][row][col].Text = "";
                        }
                    }
                }
            }
            else if (difficulty == "Hard") 
            {
                while (!done)
                {
                    buttons[rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)][rand.Next(0, 3)].Text = randN.Next(1, 10).ToString();
                    solution = BTS.solve(buttons);
                    done = setButtons(solution);
                }
                for (int i = 0; i < buttons.Count; i++)
                {
                    for (int j = 0; j < buttons[i].Count; j++)
                    {
                        for (int count = 0; count < 15; count++)
                        {
                            row = rand.Next(0, 3);
                            col = rand.Next(0, 3);

                            buttons[i][j][row][col].Text = "";
                        }
                    }
                }
            }
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            if(buttons[l][i][j][k].Text != "")
                            {
                                buttons[l][i][j][k].Font = new Font("Arial", 10, FontStyle.Bold);
                                buttons[l][i][j][k].ForeColor = Color.DarkBlue;
                            }
                        }
                    }
                }
            }
        }

        void solve()
        {
            BackTrackSolver BTS = new BackTrackSolver();
            if (newGameTrigger)
            {
                setButtons(solution);
                MessageBox.Show("Solve done, you cheater!");
            }
            else if (setButtons(BTS.solve(buttons)))
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
