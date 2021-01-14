using System;

namespace _Array_Random
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        private static void PrintArray(int[] v)
        {
            for (int i = 0; i < v.Length; i++)
                Console.Write("{0,5}{1}", v[i], (i % 10 == 9) ? "\n" : "");
        }
    }
}