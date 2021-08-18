using System;

namespace _6_Boxing
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1234;
            object box = i;
            Console.WriteLine(box);
            int j = (int)box;
            Console.WriteLine(j);

            double d = 3.14;
            object box1 = d;
            Console.WriteLine(box1);
            double d1 = (double)box1;
            Console.WriteLine(d1);
        }
    }
}