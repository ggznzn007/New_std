using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _13_GenericMethod
{
    class Program
    {
        static void Swap<T>(ref T a, ref T b)
        {
            T t;
            t = a; a = b; b = t;
        }

        static void Main(string[] args)
        {
            int i1 = 3, i2 = 4;
            Console.WriteLine("i1 = {0}, i2 = {1}",
                i1, i2);
            Swap<int>(ref i1, ref i2);
            Console.WriteLine("i1 = {0}, i2 = {1}",
                i1, i2);

            string s1 = "멍멍", s2 = "꼬꼬댁";
            Console.WriteLine("s1 = {0}, s2 = {1}",
                s1, s2);
            Swap<string>(ref s1, ref s2);
            Console.WriteLine("s1 = {0}, s2 = {1}",
                s1, s2);
        }
    }
}
