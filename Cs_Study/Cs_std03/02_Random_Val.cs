using System;

namespace RandomVal01
{
    class Pro
    {
        static void Main(string[] args)
        {
            Random r = new Random();

            int[] arr = new int[10];
            int minVal = 0;
            int maxVal = 100;

            for (int i = 0; i < arr.Length; i++)
                arr[i] = r.Next(minVal, maxVal + 1);
            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
        }
    }
}