using System;

namespace _ArrayClass
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        private static void PrintArray(int[] a)
        {
            foreach (var i in a)
                Console.Write("{0,5}", i);
            Console.WriteLine();
        }
    }
}