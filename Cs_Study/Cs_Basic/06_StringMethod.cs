using System;

namespace _StringMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "Hello, World!";
            string t;

            Console.WriteLine(s.Length);
            Console.WriteLine(s[8]);
            Console.WriteLine(s.Insert(8, "C# "));

        }
    }
}