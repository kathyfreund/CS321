using System;
using CptS321;


namespace ExpTreeTesting
{
    class TestingTree
    {
        static void Main(string[] args)
        {
            ExpTree et = new ExpTree("A1-12-C1");
            int choice = 0;
            while (choice != 4)
            {
                Console.WriteLine("Menu (current expression: {0})", et.expression);
                Console.WriteLine("\t1 = Enter a new expression");
                Console.WriteLine("\t2 = Set a variable value");
                Console.WriteLine("\t3 = Evaluate tree");
                Console.WriteLine("\t4 = Quit");
                var input = Console.ReadLine();

                switch (Convert.ToInt32(input))
                {
                    case 1:
                        Console.Write("Enter new expression: ");
                        var temp = Console.ReadLine();
                        et = new ExpTree(temp); //not good, but cant figure out another way as of now
                        break;
                    case 2:
                        Console.Write("Enter variable name: ");
                        var old = Console.ReadLine();
                        Console.Write("Enter variable value: ");
                        var newv = Console.ReadLine();
                        et.SetVar(old, Convert.ToDouble(newv));
                        break;
                    case 3:
                        Console.WriteLine("{0}", et.Eval());
                        break;
                    case 4:
                        choice = 4;
                        break;
                    default:
                        Console.WriteLine("// This is a command that the app will ignore");
                        break;

                }
            }
            Console.WriteLine("Done");
        }
    }
}
