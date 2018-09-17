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
using System.Xml;
using System.IO;

namespace Spreadsheet
{
    public partial class Form1 : Form
    {
        private spreadsheet basesheet = new spreadsheet(50, 26);
        private int curr_row = 0;
        private int curr_col = 0;
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
            //2 pts - DONE
            dataGridView1.CellEnter += CellEnter; //shows cell text in upper textbox [WORKING] 
            textBox1.KeyDown += keydown; //checks to see if key pressed
            textBox1.Leave += leave;


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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = basesheet.GetCell(e.RowIndex, e.ColumnIndex).setText;
            curr_row = e.RowIndex;
            curr_col = e.ColumnIndex;
        }

        private void keydown(object sender, KeyEventArgs e) //https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/how-to-detect-when-the-enter-key-pressed
        {
            if(Keys.Return == e.KeyCode)
            {
                leave(sender, new EventArgs());
            }
        }

        private void leave(object sender, EventArgs e)
        {
            dataGridView1.Rows[curr_row].Cells[curr_col].Value = textBox1.Text;
            CellEndEdit(dataGridView1, new DataGridViewCellEventArgs(curr_col, curr_row));
            textBox1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void file_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //********************************* SAVE/LOAD/ABOUT

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream s;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                s = sfd.OpenFile();
                //string path = sfd.FileName;
                //dt.WriteXml(@path, XmlWriteMode.WriteSchema);
                basesheet.save(s);
            }
            
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            if(ofd.ShowDialog() == DialogResult.OK) //https://www.youtube.com/watch?v=1oBHpE7H12A
            {
                Clear();
                FileStream s = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                //string path = ofd.FileName;
                //dt.ReadXml(@path);
                //dataGridView1.DataSource = dt.Tables;
                basesheet.load(s);
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) //https://www.youtube.com/watch?v=aqrZJSmirk0
        {
            MessageBox.Show(" 321 Spreadsheet App - 2018 \n Version 5.0 \n Katherine Freund \n katherine.freund@wsu.edu \n Copyright © 2018 \n Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)", "About");
        }

        private void Clear()
        {
            for (int i = 0; i < 50; ++i)
            {
                for (int j = 0; j < 26; ++j)
                {
                    basesheet.GetCell(i, j).setText = "";
                }
            }
        }
    }
}

