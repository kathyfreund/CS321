using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int round1 = 0;
            int round2 = 0;
            int round3 = 0;

            int val;
            var rand = new Random();
            var rlist = new List<int>();

            Dictionary<int, int> hash = new Dictionary<int, int>(); //Method 1

            for (int i = 0; i < 10000; i++)
            {
                val = rand.Next(0, 20000);
                rlist.Add(val);
                if(!hash.ContainsKey(val.GetHashCode()))
                {
                    hash.Add(val.GetHashCode(), val);   //Method 1    
                }        
            }


            //1.Hash Table
                //insert values into dictionary - make sure to check if key was already used
                //once finished, count the dictionary
            round1 = hash.Count();
            textBox1.Text += "Method 1 [Hash Table] :      " + round1 + " random variables.";
            textBox1.AppendText(Environment.NewLine);

            //2.O(1)
                //use distinct built in function to get list of distinct numbers, then count them - is this cheating :(
            round2 = rlist.Distinct().Count();    // this is my cop-out if i can't figure something else out -> https://msdn.microsoft.com/en-us/library/bb348436(v=vs.110).aspx
            textBox1.Text += "Method 2 [O(1)] :                 " + round2 + " random variables.";
            textBox1.AppendText(Environment.NewLine);

            //3.Sort
                //compare current value with next value in sorted list
                //if current value does not match next value (last repeat). increment counter
            rlist.Sort();
            int prev = -1;
            foreach(int item in rlist)
            {
                if(prev != item)
                {
                    round3++;
                }
                prev = item;
            }
            textBox1.Text += "Method 3 [Sorting] :             " + round3 + " random variables.";
            textBox1.AppendText(Environment.NewLine);
            //PRINT IT ALL OUT

            textBox1.AppendText(Environment.NewLine);
            textBox1.Text += "*HASH TIME COMPLEXITY* - I thought it would be a good idea to insert the ";
            textBox1.Text += "keys/values into the dictionary as we were building the randomized list. ";
            textBox1.Text += "Each round would check if the key was already used and it it wasnt, it ";
            textBox1.Text += "would be put into the dictionary. Afterwards it ";
            textBox1.Text += "was just a matter of counting the number of elements in the dictionary. ";
            textBox1.Text += "All in all, the time complexity ended up being O(n) since it did have to ";
            textBox1.Text += "go through the entire list and check to see if the values were already inserted. ";
            textBox1.AppendText(Environment.NewLine);


        }
    }
}
