using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BSTtree
{
    // ****************************** NODE JUNK **********************************

    class BSTNode
    {
        private int val;
        public BSTNode left;
        public BSTNode right;

        public BSTNode(int inval)
        {
            val = inval;
            left = null;
            right = null;
        }

        public BSTNode(int inval, BSTNode inleft, BSTNode inright)
        {
            val = inval;
            left = inleft;
            right = inright;
        }

        public int getVal()
        {
            return val;
        }


        public override string ToString() { return val.ToString(); }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            BSTNode other = (BSTNode)obj;
            return this.val == other.val;
        }



        public override int GetHashCode()
        {
            return this.val;
        }


        public static bool operator ==(BSTNode a, BSTNode b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.val == b.val;
        }


        public static bool operator !=(BSTNode a, BSTNode b)
        {
            return !(a == b);
        }

        public static Boolean operator >=(BSTNode a, BSTNode b)
        {
            return a.val >= b.val;
        }

        public static Boolean operator <=(BSTNode a, BSTNode b)
        {
            return a.val <= b.val;
        }

        public static Boolean operator <(BSTNode a, BSTNode b)
        {
            return a.val < b.val;
        }
		public static Boolean operator >(BSTNode a, BSTNode b)
		{
			return a.val > b.val;
		}

    }
     

    // ******************************* TREE JUNK *********************************

    class BSTTree
    {
        private BSTNode root;

        public BSTTree()
        {
            root = null;
        }

        public bool Empty()
        {
            return root == null;
        }

        public int Levels()
        {
            int maxDepth = 0;
            maxDepth = CalcMaxDepth(this.root, 0);
            return maxDepth;
        }

        private int CalcMaxDepth(BSTNode currNode, int parentDepth)
        {
            if (currNode == null) { return parentDepth; }
            return Math.Max(CalcMaxDepth(currNode.left, parentDepth + 1), CalcMaxDepth(currNode.right, parentDepth + 1));
        }


        public int Count()
        {
            int count = 0;
            return CountHelper(this.root, ref count);
        }

        private int CountHelper(BSTNode currNode, ref int count)
        {
            if (currNode == null) //empty
            {
                
                return count;
            }

            count++;
            CountHelper(currNode.left, ref count);
            CountHelper(currNode.right, ref count);
            return (count);
        }


        public int MinHeight()
        {
            int i = 1, height = 1, nodes = this.Count();

            while(i <= nodes) //is this legal - c/c++?
            {
				i = (i * 2) + 1;
                height++;
            }
            return height;
        }


        public bool Contains (int val)
        {
            BSTNode currNode = this.root;
            BSTNode testNode = new BSTNode(val);
            while(currNode != null)
            {
                if(testNode == currNode)
                {
                    return true;
                }
                else if(testNode < currNode)
                {
                    currNode = currNode.left;
                }
                else
                {
                    currNode = currNode.right;
                }
            }
            return false;
        }

        public void Insert(int val)
        {
            BSTNode tapNode = new BSTNode(val);
            this.InsertHelper(ref this.root, ref tapNode);
        }


        public void InsertHelper(ref BSTNode curr_root, ref BSTNode newNode)
        {
            //Console.WriteLine("Currently inserting : {0}", newNode);
            //Console.WriteLine("Current root : {0}", curr_root);

            if(curr_root == null)
            {
                curr_root = newNode;
				//this.root = newNode; //forgot about recursion

				//checking...
				//Console.WriteLine("Root added : {0}", this.root.getVal());
            }
            else if(newNode < curr_root) //not actually checking values?
            {
                //Console.WriteLine("Lefty added.");
                this.InsertHelper(ref curr_root.left, ref newNode);
            }
            else if(newNode > curr_root) //not actually checking values?
			{
                //Console.WriteLine("Righty added.");
                this.InsertHelper(ref curr_root.right, ref newNode);
            }
            else
            {
                //Console.WriteLine(" [!!!] - Tried to insert duplicate");
            }
			//Console.WriteLine("End of Insert.\n");

		}

        public void PrintPreOrder()
        {
            PrintPreOrderHelper(ref this.root);
            Console.Write("\n");
        }

        public void PrintPreOrderHelper(ref BSTNode node)
        {
            if (node == null) { return; }
            Console.Write(node + " ");
            PrintPreOrderHelper(ref node.left);
            PrintPreOrderHelper(ref node.right);
        }

        public void PrintPostOrder()
        {
            PrintPostOrderHelper(ref this.root);
            Console.Write("\n");

        }

        private void PrintPostOrderHelper(ref BSTNode node)
        {
            if (node == null) { return; }
            PrintPostOrderHelper(ref node.left);
            PrintPostOrderHelper(ref node.right);
            Console.Write(node + " ");
        }

        public void PrintinOrder()
        {
            PrintinOrderHelper(ref this.root);
            Console.Write("\n");
        }

        private void PrintinOrderHelper(ref BSTNode node)
        {
            if (node == null) { return; }
            PrintinOrderHelper(ref node.left);
            Console.Write(node + " ");
            PrintinOrderHelper(ref node.right);
        }

        static void Main(string[] args)
        {

            //************************* TESTING 1 2 3 ****************************

            BSTTree tree = new BSTTree();

			Console.WriteLine(" \n******************* ");
			Console.WriteLine(" ***** TESTING ***** ");
			Console.WriteLine(" ******************* \n");

            Console.WriteLine("[!] Inserting 9, 4, 24, 12, and 20: ");

            tree.Insert(9);
            tree.Insert(4);
            tree.Insert(24);
            tree.Insert(12);
            tree.Insert(20);

            Console.Write("\n");

			Console.Write("In Order: ");
			tree.PrintinOrder();
			Console.Write("PreOrder(Root, Left, Right): ");
			tree.PrintPreOrder();
			Console.Write("PostOrder(Left, Right, Root): ");
			tree.PrintPostOrder();
			
            Console.WriteLine("\n[!] Now inserting 78, 32, and 41: ");

            tree.Insert(78);
            tree.Insert(32);
            tree.Insert(41);

            Console.Write("\n");

			Console.Write("In Order: ");
			tree.PrintinOrder();
			Console.Write("PreOrder(Root, Left, Right): ");
			tree.PrintPreOrder();
			Console.Write("PostOrder(Left, Right, Root): ");
			tree.PrintPostOrder();

            Console.WriteLine("\nDoes the tree have 99 in it? (No): " + tree.Contains(99));
            Console.WriteLine("How about 12? (Yes): " + tree.Contains(12));

            Console.Write("\n");
			//********************** ACTUAL ASSIGNMENT ***************************

			BSTTree tree2 = new BSTTree();

            Console.WriteLine(" ******************* ");
            Console.WriteLine(" ****** C# BST ***** ");
            Console.WriteLine(" ******************* \n");

            Console.WriteLine("Please enter a list of integer number (1-100): ");
            string inline = Console.ReadLine();

            Console.WriteLine("User entered this data: " + inline);
            char[] delimiterChars = { ' ', ',', '_', ':', '\t' }; //Part of Crandall's in class code

            string[] words = inline.Split(delimiterChars);

            Console.WriteLine("Length of array: {0}", words.Length);

            foreach (string s in words) //Part of Crandall's in class code
			{
                int newVal = 0;
                if( Int32.TryParse(s, out newVal))
                {
                    tree2.Insert(newVal);
                }

            }

            Console.WriteLine("\n[!] Here's Your Tree: \n");

            Console.Write("In Order: ");
            tree2.PrintinOrder();
            Console.Write("PreOrder(Root, Left, Right): ");
            tree2.PrintPreOrder();
            Console.Write("PostOrder(Left, Right, Root): ");
            tree2.PrintPostOrder();

            Console.WriteLine("\n[!] Tree Info: \n");

            Console.WriteLine("Nodes: {0}", tree2.Count());
            Console.WriteLine("Levels: {0}", tree2.Levels());
            Console.WriteLine("Min Height given # of Nodes: {0}", tree2.MinHeight()); //log2(n)+1

            //im done c:
        }
    }

}
