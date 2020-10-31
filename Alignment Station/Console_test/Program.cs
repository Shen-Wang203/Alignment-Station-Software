using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Console_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Start:");
            string str1 = "t 0 ";
            int str2 = 3252;
            string str3 = " 0 0";
            try
            { str1 = string.Concat(str1, str2, str3, " iie"); }
            catch (Exception)
            { }
            Console.WriteLine(str1);
            if (1 < 2)
                Console.WriteLine(string.Concat(str1, "\n"));
            Console.WriteLine("end");
            if (str1[2] == '0')
                Console.WriteLine("ys");
            double a = 2.83252663;
            Console.WriteLine((int)Math.Round(a));
            Console.WriteLine(Math.Abs(-2.33-1.64));
            int[] count = { 1, 2, 3, 4, 2, 2 };
            int[] countNew = { 0, 0, 0, 0, 0, 0 };
            countNew = count;
            Console.WriteLine($"countNew is {countNew}");
            Console.WriteLine($"count is {count}");
            count = new int[] { 2,2,2,2,2,2 };
            Console.WriteLine($"countNew is {countNew}");
        }
    } 
}
