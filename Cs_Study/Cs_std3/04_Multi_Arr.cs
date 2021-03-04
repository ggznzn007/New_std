using System;

namespace multiArr
{
    class Pro
    {
        static void Main()
        {
            int n = 3;
            int m = 5;

            int[,] arr = new int[n, m];

            int lengthN = arr.GetLength(0);
            int lengthM = arr.GetLength(1);


            Console.WriteLine($"Arr is { lengthN}*{lengthM} size");
        }
    }
}