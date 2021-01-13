using System;

namespace _LoopPyramid
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1) 
            for(int i = 1;i<=10;i++)
            {
                for (int j = 1; j <= i; j++)
                    Console.Write("*");
                Console.WriteLine();
            }
            Console.WriteLine();

            // 2)

        }
    }
}