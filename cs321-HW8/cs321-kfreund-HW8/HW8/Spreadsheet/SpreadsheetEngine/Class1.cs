using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        protected int rowIndex, columnIndex;
        protected string text;
        protected string value;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Cell() //set in constructor
        {
            
        }
        public int getRow //row getter
        {
            get
            {
                return this.rowIndex;
            }
            protected set
            {
                this.rowIndex = value;
            }
        }
        public int getCol //row getter
        {
            get
            {
                return this.columnIndex;
            }
            protected set
            {
                this.columnIndex = value;
            }
        }
        public string setText //text setter & getter
        {
            get
            {
                return text;
            }
            set
            {
                if (value == text)
                {
                    return; //ignore
                }
                else
                {
                    text = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }
        public string Value //value setter & getter
        {
            get
            {
                return this.value;
            }
            protected internal set //ONLY THE CELL CLASS CAN MESS WITH THIS
            {
                if (value == this.value)
                {
                    return; //ignore
                }
                else
                {
                    this.value = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }
    public class UsedCell : Cell
    {
        public UsedCell(int row, int col, string txt)
        {
            rowIndex = row;
            columnIndex = col;
            this.text = txt;
            value = txt;
        }
    }

    

    public class spreadsheet
    {
        private int rows, columns;
        private Cell[,] ss;
        private Dictionary<Cell, HashSet<Cell>> dep;
        public event PropertyChangedEventHandler CellPropertyChanged;

        public spreadsheet(int row, int col)
        {
            this.rows = row;
            this.columns = col;
            this.dep = new Dictionary<Cell, HashSet<Cell>>();
            ss = new Cell[row, col];

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    ss[r, c] = new UsedCell(r, c, "");
                    ss[r, c].PropertyChanged += new PropertyChangedEventHandler(SPropertyChanged);
                }
            }
        }

        public Cell GetCell(int r, int c)
        {
            if (r > ss.GetLength(0) || c > ss.GetLength(1))
            {
                return null;
            }
            else
            {
                return ss[r, c];
            }
        }
        public int ColumnCount
        {
            get
            {
                return rows;
            }
        }
        public int RowCount
        {
            get
            {
                return columns;
            }
        }
        public void SPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell c = (Cell)sender;
            if(e.PropertyName == "Text")
            {
                try
                {
                    //removedep(c);
                    getCellValue(c);
                }
                catch (Exception ex)
                {

                }
                CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
            }
            else if(e.PropertyName == "Value")
            {
                try
                {
                    cascading(c);
                }
                catch(Exception ex)
                {

                }
                CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
            }
            
        }

        private void removedep(Cell c)
        {
            if (dep.ContainsKey(c)) //remove from the start
            {
                dep[c].Clear();
            }
            dep.Remove(c);
        }

        private void getCellValue(Cell c)
        {
            //c = (cell)sender
            if (c.setText.Length == 0)
            {
                c.Value = "";
            }
            else if (!c.setText.StartsWith("=")) //no formula
            {
                c.Value = c.setText;
            }
            else // formula
            {
                if (c.setText.Length > 1)
                {
                    try
                    {
                        string equation = c.setText.Substring(1);
                        ExpTree xp = new ExpTree(equation);
                        string[] cref = xp.getVariables();

                        foreach (string name in cref)
                        {
                            double val = getCorrCellVal(name);
                            xp.SetVar(name, val);
                            
                            Cell key = getCorrCell(name);
                            if (!dep.ContainsKey(key)) //add dependency
                            {
                                dep.Add(key, new HashSet<Cell>());
                            }
                            dep[key].Add(c);
                        }
                        c.Value = xp.Eval().ToString();
                    }
                    catch
                    {
                        c.Value = "#REF!";
                        throw;
                    }
                }
            }
        }


        private void cascading(Cell c)
        {
            /*
            if(norefcycles(c, c))
            {
                foreach(Cell key in dep.Keys)
                {
                    if (dep[key].Contains(c))
                    {
                        getCellValue(key);
                    }
                }
            }
            */
            if (dep.ContainsKey(c)) //add dependency
            {           
                foreach (Cell k in dep[c])
                {
                    getCellValue(k);
                }
            }

        }

        /*
        private bool norefcycles(Cell root, Cell c)
        {
            if(!dep.ContainsKey(c))
            {
                return true;
            }
            bool x = true;
            foreach(Cell ac in dep[c])
            {
                if (ReferenceEquals(ac, root))
                {
                    return false;                   
                }
                x = x && norefcycles(root, ac);
            }
            return x;
        }
        */







        public Cell getCorrCell(string s)
        {
            s = "=" + s;
            int col = s[1] - 'A';
            int row = Int32.Parse(s.Substring(2)) - 1; 

            return GetCell(row, col); 
        }
        public double getCorrCellVal(string s)
        {
            s = "=" + s;
            int col = s[1] - 'A';
            int row;
            if (Int32.TryParse((s.Substring(2)), out row)) 
            {
                double result;
                if (double.TryParse(GetCell(row - 1, col).Value, out result))
                {
                    return result;
                }
                return 0.0;
            }
            else
                return 0.0; 
        }




        







        public void Demo()
        {
            int i = 0;
            Random rand = new Random();

            while (i < 50)
            {
                int rc = rand.Next(0, 25);
                int rr = rand.Next(0, 49);

                Cell x = GetCell(rr, rc);
                x.setText = "Hello World!";
                this.ss[rr, rc] = x;
                i++;
            }
            for (i = 0; i < 50; i++)
            {
                this.ss[i, 1].setText = "This is cell B" + (i + 1);
            }
            for (i = 0; i < 50; i++)
            {
                this.ss[i, 0].setText = "=B" + (i + 1);
            }

        }
    }
}
