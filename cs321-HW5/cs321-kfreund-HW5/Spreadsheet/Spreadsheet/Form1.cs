using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321;

namespace Spreadsheet
{
    public partial class Form1 : Form
    {
        private spreadsheet basesheet = new spreadsheet(50, 26);
        public Form1()
        {
            InitializeComponent();
        }

        void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            dataGridView1.Rows[row].Cells[column].Value = (basesheet.GetCell(row, column)).setText;
        }
        void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            string text = "";
            Cell cell = basesheet.GetCell(row, column);

            try
            {
                text = dataGridView1.Rows[row].Cells[column].Value.ToString();
            }
            catch (NullReferenceException)
            {
                text = "";
            }
            cell.setText = text;

        }

        private void SCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = (Cell)sender;
            if (cell != null && e.PropertyName == "Value")
            {
                dataGridView1.Rows[cell.getRow].Cells[cell.getCol].Value = cell.Value;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            basesheet.CellPropertyChanged += SCellPropertyChanged;
            dataGridView1.CellBeginEdit += CellBeginEdit;
            dataGridView1.CellEndEdit += CellEndEdit;
            dataGridView1.Columns.Clear();
            for(char i = 'A'; i <= 'Z'; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = i.ToString();
                dataGridView1.Columns.Add(column);
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(50);
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            basesheet.Demo();
        }
    }
}

