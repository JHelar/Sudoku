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
    class BackTrackSolver
    {
        #region Private Variables
        private string digits;
        private string rows;
        private string cols;
        private List<string> squares;
        private Dictionary<string, List<List<string>>> units;
        private Dictionary<string, List<string>> peers;
        private Dictionary<string, string> values; 
        #endregion

        #region Public Methods
        public BackTrackSolver()
        {
            List<List<string>> unitList = new List<List<string>>();
            List<string> rsL = new List<string>();
            List<string> csL = new List<string>();
            List<string> tempStrList = new List<string>();
            List<List<string>> tempStrListD = new List<List<string>>();

            bool excist = new bool();

            squares = new List<string>();
            units = new Dictionary<string, List<List<string>>>();
            peers = new Dictionary<string, List<string>>();
            values = new Dictionary<string, string>();

            rsL.Add("ABC");
            rsL.Add("DEF");
            rsL.Add("GHI");
            csL.Add("123");
            csL.Add("456");
            csL.Add("789");
            digits = "123456789";
            rows = "ABCDEFGHI";
            cols = digits;
            squares = cross(rows, cols);
            foreach (char c in cols)
                unitList.Add(cross(rows, c));
            foreach (char r in rows)
                unitList.Add(cross(r, cols));
            foreach (string rs in rsL)
                foreach (string cs in csL)
                    unitList.Add(cross(rs, cs));

            foreach (string s in squares)
            {
                foreach (List<string> u in unitList)
                {
                    excist = false;
                    foreach (string uElem in u)
                    {
                        if (uElem == s)
                            excist = true;
                    }
                    if (excist)
                        tempStrListD.Add(u);
                }
                units[s] = tempStrListD;
                tempStrListD = new List<List<string>>();
            }
            bool breakFlag = new bool();
            foreach (string s in squares)
            {
                foreach (List<string> u in units[s])
                {
                    foreach (string uElem in u)
                    {
                        breakFlag = false;
                        if (uElem != s)
                        {
                            if (tempStrList.Count > 0)
                            {
                                foreach (string t in tempStrList)
                                    if (uElem == t)
                                    {
                                        breakFlag = true;
                                        break;
                                    }
                                if (!breakFlag)
                                    tempStrList.Add(uElem);
                            }
                            else
                                tempStrList.Add(uElem);
                        }
                    }
                }
                peers[s] = tempStrList;
                tempStrList = new List<string>();
            }
        }

        public Dictionary<string, string> solve(List<List<List<List<Button>>>> buttons)
        {
            parse_grid(buttons);
            Dictionary<string, string> value_copy = new Dictionary<string, string>(values);
            return search(value_copy);
        }

        public bool parse_grid(List<List<List<List<Button>>>> buttons)
        {
            foreach (string s in squares)
                values[s] = digits;
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons[i].Count; j++)
                {
                    for (int k = 0; k < buttons[i][j].Count; k++)
                    {
                        for (int l = 0; l < buttons[i][j][k].Count; l++)
                        {
                            if (buttons[i][j][k][l].Text != "")
                            {
                                if (assign(this.values, buttons[i][j][k][l].Name, buttons[i][j][k][l].Text).Count == 0)
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region Private Methods

        private Dictionary<string, string> search(Dictionary<string, string> values) 
        {
            bool Done = true;
            string bestKey = "";
            int bestCount = 100;
            if (values.Count == 0)
                return new Dictionary<string, string>();
            foreach (string s in squares) 
            {
                if (values[s].Count() != 1)
                    Done = false;
            }
            if (Done)
                return values;
            foreach (string s in squares) 
            {
                if (values[s].Count() > 1) 
                {
                    if (values[s].Count() < bestCount)
                    {
                        bestCount = values[s].Count();
                        bestKey = s;
                    }
                }
            }
            Dictionary<string, string> values_copy = new Dictionary<string,string>(values);
            if (bestKey == "")
                return new Dictionary<string, string>();
            foreach (char d in values[bestKey])
            {
                return some(search(assign(values_copy, bestKey, d.ToString())));
            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> some(Dictionary<string, string> values) 
        {
            if (values.Count == 0)
                return new Dictionary<string, string>();
            else
                return values;
        } 

        private Dictionary<string, string> assign(Dictionary<string, string> values,string key, string value) 
        {
            string other_values = values[key];
            other_values = other_values.Replace(value, "");
            foreach (char otherVal in other_values) 
            {
                if (eliminate(values, key, otherVal.ToString()).Count == 0)
                    return new Dictionary<string,string>();
            }
            return values;
        }

        private Dictionary<string, string> eliminate(Dictionary<string, string> values, string key, string value) 
        {
            string lastVal = "";
            List<string> keyPlaces = new List<string>();

            if (!values[key].Contains(value))
                return values;

            values[key] = values[key].Replace(value, "");

            if(values[key].Count() == 0)
                return new Dictionary<string,string>();
            else if (values[key].Count() == 1)
            {
                lastVal = values[key];
                foreach (string keyP in peers[key]) 
                {
                    if (eliminate(values, keyP, lastVal).Count == 0)
                        return new Dictionary<string,string>();
                }
            }
            foreach (List<string> u in units[key]) 
            {
                foreach (string s in u) 
                {
                    if(values[s].Contains(value))
                        keyPlaces.Add(s);
                }
                if (keyPlaces.Count == 0)
                    return new Dictionary<string,string>();
                else if (keyPlaces.Count == 1)
                    if (assign(values, keyPlaces[0], value).Count == 0)
                        return new Dictionary<string,string>();
            }
            return values;
        }

        private List<string> cross(char a, string B)
        {
            List<string> returnList = new List<string>();

            foreach (char b in B)
                returnList.Add(a.ToString() + b.ToString());

            return returnList;
        }

        private List<string> cross(string A, char b)
        {
            List<string> returnList = new List<string>();

            foreach (char a in A)
                returnList.Add(a.ToString() + b.ToString());

            return returnList;
        }

        private List<string> cross(string A, string B)
        {
            List<string> returnList = new List<string>();
            foreach (char a in A)
            {
                foreach (char b in B)
                    returnList.Add(a.ToString() + b.ToString());
            }

            return returnList;
        } 
        #endregion
    }
}
