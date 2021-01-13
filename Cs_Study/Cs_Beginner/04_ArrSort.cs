using System;

namespace _ArrSort
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        private static void PrintArray(string s, string[] name)
        {
            Console.WriteLine(s);
            foreach (var n in name)
                Console.Write("{0} ", n);
            Console.WriteLine();
        }
    }
}