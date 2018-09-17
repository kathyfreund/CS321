using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BSTtree
{
    // ****************************** NODE JUNK **********************************

    class BSTNode<T>
    {
        private T val;
        public BSTNode<T> left;
        public BSTNode<T> right;

        public BSTNode()
        { }

        public BSTNode(T inval)
        {
            val = inval;
            left = null;
            right = null;
        }

        public BSTNode(T inval, BSTNode<T> inleft, BSTNode<T> inright) 
        {
            val = inval;
            left = inleft;
            right = inright;
        }

        public T getVal()
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
            BSTNode<T> other = (BSTNode<T>)obj;
            return val.Equals(other.val);
        }

        /*
        public override int GetHashCode()
        {
            return this.val;
        }
        */

        public static bool operator ==(BSTNode<T> a, BSTNode<T> b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a == b;
        }
        

        public static bool operator !=(BSTNode<T> a, BSTNode<T> b)
        {
            return !(a == b);
        }

        public static Boolean operator >=(BSTNode<T> a, BSTNode<T> b)
        {
            return (a >= b);
        }

        public static Boolean operator <=(BSTNode<T> a, BSTNode<T> b)
        {
            return (a <= b);
        }

        public static Boolean operator <(BSTNode<T> a, BSTNode<T> b)
        {
            return (a < b);
        }
		public static Boolean operator >(BSTNode<T> a, BSTNode<T> b)
		{
			return (a > b);
		}

    }





    // ******************************* ABSTRACT BINTREE CLASS *********************************

    public abstract class BinTree<T>
    {
        public abstract void Insert<T>(T val);

        public abstract bool Contains<T>(T val);

        public abstract void InOrder();

        public abstract void PreOrder();

        public abstract void PostOrder();

    }


    // ******************************* TREE JUNK *********************************

    class BSTTree<T> : BinTree<T> where T : IComparable<T>
    {
        private BSTNode<T> root;

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

        private int CalcMaxDepth(BSTNode<T> currNode, int parentDepth)
        {
            if (currNode == null) { return parentDepth; }
            return Math.Max(CalcMaxDepth(currNode.left, parentDepth + 1), CalcMaxDepth(currNode.right, parentDepth + 1));
        }


        public int Count()
        {
            int count = 0;
            return CountHelper(this.root, ref count);
        }

        private int CountHelper(BSTNode<T> currNode, ref int count)
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


        public override bool Contains<T>(T val)
        {
            BSTNode<T> currNode = this.root as BSTNode<T>;
            BSTNode<T> testNode = new BSTNode<T>(val);
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

        public override void Insert<T>(T val)
        {
            BSTNode<T> tapNode = new BSTNode<T>(val);
            BSTNode<T> currNode = this.root as BSTNode<T>;
            this.InsertHelper<T>(ref currNode, ref tapNode);
        }

        public void InsertHelper<T>(ref BSTNode<T> curr_root, ref BSTNode<T> newNode)
        {
            //Console.WriteLine("Currently inserting : {0}", newNode);
            //Console.WriteLine("Current root : {0}", curr_root);

            if (curr_root == null)
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

        public override void PreOrder()
        {
            PrintPreOrderHelper(ref this.root);
            Console.Write("\n");
        }

        public void PrintPreOrderHelper(ref BSTNode<T> node)
        {
            if (node == null) { return; }
            Console.Write(node + " ");
            PrintPreOrderHelper(ref node.left);
            PrintPreOrderHelper(ref node.right);
        }

        public override void PostOrder()
        {
            PrintPostOrderHelper(ref this.root);
            Console.Write("\n");

        }

        private void PrintPostOrderHelper(ref BSTNode<T> node)
        {
            if (node == null) { return; }
            PrintPostOrderHelper(ref node.left);
            PrintPostOrderHelper(ref node.right);
            Console.Write(node + " ");
        }

        public override void InOrder()
        {
            PrintinOrderHelper(ref this.root);
            Console.Write("\n");
        }

        private void PrintinOrderHelper(ref BSTNode<T> node)
        {
            if (node == null) { return; }
            PrintinOrderHelper(ref node.left);
            Console.Write(node + " ");
            PrintinOrderHelper(ref node.right);
        }

        static void Main(string[] args)
        {

            //************************* TESTING 1 2 3 ****************************

            BSTTree<T> tree = new BSTTree<T>();

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
			tree.InOrder();
			Console.Write("PreOrder(Root, Left, Right): ");
			tree.PreOrder();
			Console.Write("PostOrder(Left, Right, Root): ");
			tree.PostOrder();
			
            Console.WriteLine("\n[!] Now inserting 78, 32, and 41: ");

            tree.Insert(78);
            tree.Insert(32);
            tree.Insert(41);

            Console.Write("\n");

			Console.Write("In Order: ");
			tree.InOrder();
			Console.Write("PreOrder(Root, Left, Right): ");
			tree.PreOrder();
			Console.Write("PostOrder(Left, Right, Root): ");
			tree.PostOrder();

            Console.WriteLine("\nDoes the tree have 99 in it? (No): " + tree.Contains(99));
            Console.WriteLine("How about 12? (Yes): " + tree.Contains(12));

            Console.Write("\n");
			//********************** ACTUAL ASSIGNMENT ***************************

			BSTTree<T> tree2 = new BSTTree<T>();

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
            tree2.InOrder();
            Console.Write("PreOrder(Root, Left, Right): ");
            tree2.PreOrder();
            Console.Write("PostOrder(Left, Right, Root): ");
            tree2.PostOrder();

            Console.WriteLine("\n[!] Tree Info: \n");

            Console.WriteLine("Nodes: {0}", tree2.Count());
            Console.WriteLine("Levels: {0}", tree2.Levels());
            Console.WriteLine("Min Height given # of Nodes: {0}", tree2.MinHeight()); //log2(n)+1

            //im done c:
        }
    }



}
