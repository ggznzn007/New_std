using System;

namespace MulArr01
{
    class Pro
    {
        static void Main()
        {
            int n = 3;
            var m = 5;

            int[,] arr1 = new int[n, m];

            int[,] arr2 = { { 0, 1, 2, 3, 4 }, { 5, 6, 7, 8, 9 } };

            for(int i = 0;i<arr2.GetLength(0);i++)
            {
                for (int j = 0; j < arr2.GetLongLength(1); j++)
                    Console.Write(" " + arr2[i, j]);
                Console.WriteLine();
            }
        }
    }
}