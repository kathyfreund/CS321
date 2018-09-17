//Katherine Freund
//11485476

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Numerics;


namespace HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e) //LOAD
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK) //if user hits ok
            {
                TextReader read = new StreamReader(File.OpenRead(ofd.FileName));
                LoadText(read);
            }
        }

        private void saveFile(object sender, EventArgs e) //SAVE
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Open File";
            sfd.Filter = "Text Files (*.txt)|*.txt| All Files (*.*)|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter w = new StreamWriter(File.Create(path));
                w.Write(textBox1.Text); 
                w.Dispose();
            }
        }






        private void fib50_Click(object sender, EventArgs e)
        {
            FibonacciTextReader f50 = new FibonacciTextReader(50);
            LoadText(f50);
        }

        private void fib100_Click(object sender, EventArgs e)
        {
            FibonacciTextReader f100 = new FibonacciTextReader(100);
            LoadText(f100);
        }





        private void LoadText(TextReader sr)
        {
            textBox1.Text = sr.ReadToEnd(); //read in stuff from file - PUT INTO A LOOP?
            sr.Dispose();
        }
    }












    class FibonacciTextReader: TextReader //inherited class
    {
        private int maxLines;

        public FibonacciTextReader(int max) //constructor
        {
            maxLines = max;
        }


        private string ReadLine(int n)
        {
            BigInteger temp = calculate(n);
            return temp.ToString();
        }


        override public string ReadToEnd()
        {
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < maxLines; i++)
            {
                temp.AppendLine((i + 1) + ": " + ReadLine(i));
            }
            return temp.ToString();
        }


        private BigInteger calculate(int max)
        {
            int x = 0;
            int y = 1;
            int z = 0;

            if(max == 0) { return 0; } //special case 1
            else if(max == 1) { return 1; } //special case 2

            for (int i = 2; i < max; i++)
            {
                z = x + y;
                x = y;
                y = z;
            }
            return z;
        }

    }
}
