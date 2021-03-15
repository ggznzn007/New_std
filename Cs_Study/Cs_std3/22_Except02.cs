using System;

namespace Except02_Multi
{
    class Pro
    {
        static void Main(string[] args)
        {
            int[] num = { 2, 4, 8, 16, 32, 64, 128 };
            int[] denom = { 1, 2, 0, 0, 4 };

            for( int i = 0; i<num.Length;i++)
            {
                try
                {
                    Console.WriteLine($"{num[i]}/{denom[i]}={num[i] / denom[i]}");
                }
                catch(DivideByZeroException e)
                {
                    Console.WriteLine("Divide by zero.");
                }
                catch(IndexOutOfRangeException e)
                {
                    Console.WriteLine("Array is out of bounds.");
                }
            }
        }
    }
}