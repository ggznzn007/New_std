using System;

namespace ReflecArr
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            int temp = 0;

            for(int i = 0; i< arr.Length/2;i++)
            {
                temp = arr[i];
                arr[i] = arr[arr.Length - (i + 1)];
                arr[arr.Length - (i + 1)] = temp;
            }

            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
        }
    }
}