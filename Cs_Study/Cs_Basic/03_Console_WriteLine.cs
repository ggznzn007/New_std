using System;

namespace CS_
{
    class Program
    {
        static void Main(string[] args)
        {
            int i;
            double x;

            i = 5;
            x = 3.141592;
            Console.WriteLine(" i == " + i + ", x == " + x);

            x = i;
            i = (int)x;
            Console.WriteLine(" i == " + i + ", x == " + x);
        }
    }
}