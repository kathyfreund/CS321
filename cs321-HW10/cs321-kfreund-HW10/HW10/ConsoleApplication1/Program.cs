using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace MergeSort
{
    //**************INITIAL NOTE - i first used int[] to hold the lists, but that took up way to much memory and last case broke my program. 
    //switched over to lists and that seemed to fix it

    public class Compare
    {
        static void Main(string[] args)
        {
            int[] length = new int[] { 8, 64, 256, 1024 };
            int i = 0;
            long s, e;

            List<int> easy_test = new List<int> { 4, 7, 2, 1, 10, 6, 3, 9, 8, 5 };
            List<int> easy_test2 = easy_test;

            Random rnd = new Random();
            int max = rnd.Next(1, 101); //100 seems like a good max int size

            Console.WriteLine("Starting Tests - Classic MergeSort vs. Threaded MergeSort\n");

            //***********************EASY TEST************** (just for me to see if sorts are working properly
            Console.Write("Easy Test = [");
            for (int z = 0; z < 10; z++)
            {
                Console.Write(" " + easy_test[z]);
            }
            Console.Write(" ]\n");
            //CLASSIC
            s = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //set target framework to 4.6 (project properties)
            Classic.sort(easy_test, 0, (easy_test.Count - 1));
            e = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("\tClassic Sorting Time: " + (e - s));
            Console.Write("\tClassic Sorted = [");
            for (int z = 0; z < 10; z++)
            {
                Console.Write(" " + easy_test[z]);
            }
            Console.Write(" ]\n\n");
            //THREADED
            s = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Threaded.sort(easy_test2, 0, (easy_test2.Count - 1));
            e = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("\tThreaded Sorting Time: " + (e - s));
            Console.Write("\tThreaded Sorted = [");
            for (int z = 0; z < 10; z++)
            {
                Console.Write(" " + easy_test2[z]);
            }
            Console.Write(" ]\n\n");


            foreach (int x in length)
            {
                List<int> l1 = new List<int>(x);
                List<int> l2 = new List<int>(x);

                while (i < x) //print as we go
                {
                    l1.Add(rnd.Next(0, Int32.MaxValue));
                    //Console.Write(" " + l1[i]);
                    i++;
                }
                //Console.Write(" ]");
                l2 = l1; //copy it to compare

                Console.WriteLine("Sorting list size " + x);

                //*******CLASSIC MERGE***************
                s = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //set target framework to 4.6 (project properties)
                Classic.sort(l1, 0, (l1.Count - 1));
                e = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Console.WriteLine("\tClassic Sorting Time: " + (e - s));


                //*******THREADED MERGE*************
                s = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Threaded.sort(l1, 0, (l1.Count - 1));
                e = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Console.WriteLine("\tThreaded Sorting Time: " + (e - s));

            }

            Console.WriteLine("Testing Complete!\n");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }


    //************************************************Algorithms

    public class Classic
    {

        static public void sort(List<int> arr, int l, int r)
        {
            if (l < r)
            {
                // Same as (l+r)/2, but avoids overflow for
                // large l and h
                int m = l + (r - l) / 2;

                // Sort first and second halves
                sort(arr, l, m);
                sort(arr, m + 1, r);

                merge(arr, l, m, r);
            }
        }

        static public void merge(List<int> arr, int l, int m, int r) //https://www.geeksforgeeks.org/merge-sort/
        {
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;

            List<int> L = new List<int>(n1), R = new List<int>(n1);

            for (i = 0; i < n1; i++)
                L.Add(arr[l + i]);
            for (j = 0; j < n2; j++)
                R.Add(arr[m + 1 + j]);

            i = 0;
            j = 0;
            k = l;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }

    }















    class Threaded //https://www.geeksforgeeks.org/merge-sort-using-multi-threading/
    {

        static public void sort(List<int> numbers, int left, int right) //should be same logic as classic version, expect now we're working with threads
        {
            int mid;

            if(right > left)
            {
                mid = (left + right) / 2;
                Thread tleft = new Thread(() => sort(numbers, left, mid)); //https://stackoverflow.com/questions/811224/how-to-create-a-thread
                tleft.Start();
                Thread tright = new Thread(() => sort(numbers, (mid + 1), right));
                tright.Start();

                tleft.Join();
                tright.Join();

                merge(numbers, left, mid, right);

                //tleft.Abort();
                //tright.Abort();
            }
        }

        static public void merge(List<int> arr, int l, int m, int r) //https://www.geeksforgeeks.org/merge-sort/
        {
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;

            List<int> L = new List<int>(n1), R = new List<int>(n1);

            for (i = 0; i < n1; i++)
                L.Add(arr[l + i]);
            for (j = 0; j < n2; j++)
                R.Add(arr[m + 1 + j]);

            i = 0; 
            j = 0; 
            k = l; 
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }

    }
}
