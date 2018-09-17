using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ExpTree
    {
        private Node root;
        public static Dictionary<string, double> var = new Dictionary<string, double>(); //easier to keep track of I think
        public string expression { get; private set; }

        public ExpTree(string expression)
        {
            var.Clear();
            this.expression = expression;
            BuildTree(expression);
        }

        private abstract class Node
        {
            public abstract double Eval();
        }
        private class variable_node : Node //*******************************variable node
        {
            string name;
            double val;
            public variable_node(string name)
            {
                this.name = name;
                val = 0.0;
            }
            public override double Eval()
            {
                val = var[name]; //told ya
                return val;
            }
        }

        private class op : Node //**************************************** operator node
        {
            public Node left, right;
            char operand;
            public op(char x)
            {
                operand = x;
                left = null;
                right = null;
            }
            public override double Eval()
            {
                if(operand == '+')
                {
                    return this.left.Eval() + this.right.Eval();
                }
                else if (operand == '-')
                {
                    return this.left.Eval() - this.right.Eval();
                }
                else if (operand == '*')
                {
                    return this.left.Eval() * this.right.Eval();
                }
                else if (operand == '/')
                {
                    return this.left.Eval() / this.right.Eval();
                }
                else
                {
                    return 0.0; //nothing
                }
            }
        }

        private class value_node : Node //*********************************value node
        {
            double val;
            public value_node(double val)
            {
                this.val = val;
            }
            public override double Eval()
            {
                return val;
            }
        }

        Node BuildTree(string expression)
        {
            Stack<Node> nstack = new Stack<Node>();

            string pattern = @"([-/\+\*\(\)])";
            var tokens = Regex.Split(expression, pattern).Where(s => s != String.Empty).ToList<string>();
            var list = InfixtoPrefix(tokens);

            foreach (var tok in list)
            {
                Console.Write("{0}", tok);
                if (Char.IsLetter(tok[0]))
                {
                    if (!var.ContainsKey(tok)) //add to dict if it doesn't already exist
                    {
                        SetVar(tok, 0); //added to dictionary
                    }
                    nstack.Push(new variable_node(tok));
                }
                else if (Char.IsDigit(tok[0]))
                {
                    nstack.Push(new value_node(Double.Parse(tok)));
                }
                else
                {
                    var z = new op(tok[0]);
                    z.right = nstack.Pop();
                    z.left = nstack.Pop();
                    nstack.Push(z);
                }
            }
            root = nstack.Pop(); //[!] ERROR - stack empty (fixed)
            return root;
        }

        private static int precendence(string token)
        {
            switch (token)
            {
                case "*":
                case "/":
                    return 2;
                case "+":
                case "-":
                    return 1;
                case "(":
                    return 0; //took care of this in InfixtoPrefix
                case ")":
                    return 3; 
                default:
                    return -1;                  
            }

        }
        //spits out formatted list of chars
        private List<string> InfixtoPrefix(List<string> tokens) //pseudo code -> https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        {
            var opstack = new Stack<string>();
            var output = new List<string>();

            foreach(var tok in tokens)
            {
                switch (tok)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        while (opstack.Count > 0) 
                        {
                            if(precendence(opstack.Peek()) < precendence(tok))
                            {
                                break;
                            }
                            output.Add(opstack.Pop());
                        }
                        opstack.Push(tok);
                        break;
                    case "(":
                        opstack.Push(tok);
                        break;
                    case ")":
                        while (opstack.Count > 0 && opstack.Peek() != "(")
                        {
                            output.Add(opstack.Pop());
                        }
                        opstack.Pop();
                        break;
                    default: //number
                        output.Add(tok);
                        break;
                }
            }         
            while (opstack.Count > 0)
            {
                output.Add(opstack.Pop());
            }
            return output;
        }

        public void SetVar(string varName, double varValue)
        {
            try
            {
                var.Add(varName, varValue);
            }
            catch
            {
                var[varName] = varValue;
            }
        }

        public string[] getVariables()
        {
            string[] vars = new string[var.Count];
            int i = 0;
            foreach(KeyValuePair<string, double> p in var)
            {
                vars[i] = p.Key;
                i++;
            }
            return vars;
        }

        public double Eval() //gonna end up overriding it anyway
        {
            return root.Eval();
        }
    }
}
