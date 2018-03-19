using System;
using System.ComponentModel;

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
        public event PropertyChangedEventHandler CellPropertyChanged;

        public spreadsheet(int row, int col)
        {
            this.rows = row;
            this.columns = col;
            ss = new Cell[row, col];
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    ss[r, c] = new UsedCell(r, c, "");
                    ss[r, c].PropertyChanged += SPropertyChanged;
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
            if(e.PropertyName == "Text")
            {
                if(!((Cell)sender).setText.StartsWith("="))
                {
                    ((Cell)sender).Value = ((Cell)sender).setText;
                }
                else
                {
                    string equation = ((Cell)sender).setText.Substring(1);
                    int column = Convert.ToInt16(equation[0]) - 'A';
                    int row = Convert.ToInt16(equation.Substring(1)) - 1;
                    ((Cell)sender).Value = (GetCell(row, column)).Value;
                }
            }
            CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
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
