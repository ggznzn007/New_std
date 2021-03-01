using System;

namespace CreateArr
{
    class Pro
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4, 5, };

            int arrLength = 5;
            int[] arr1 = new int[arrLength];

            for (int i = 0; i < arrLength; i++)
                arr1[i] = i;

            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
            Console.WriteLine();
            for (int i = 0; i < arr1.Length; i++)
                Console.Write(" " + arr1[i]);
        }
    }
}