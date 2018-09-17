using System;
using System.Collections.Generic;
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

        private class op : Node
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
            double val;
            for(int i = expression.Length - 1; i >= 0; i--)
            {
                switch(expression[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        op newbie = new op(expression[i]);
                        if(root == null)
                        {
                            root = newbie;
                        }
                        newbie.left = BuildTree(expression.Substring(0, i));
                        newbie.right = BuildTree(expression.Substring(i + 1));
                        return newbie;
                }
            }
            if(Double.TryParse(expression, out val))
            {
                return new value_node(val);
            }
            else
            {
                return new variable_node(expression);
            }
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

        public double Eval() //gonna end up overriding it anyway
        {
            return root.Eval();
        }
    }
}
