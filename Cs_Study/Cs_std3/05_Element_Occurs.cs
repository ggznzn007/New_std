using System;

namespace Arr_Occurs
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 0, 1, 1, 1, 1, 1, 1, 2, 3, 4, 5, 6, 7 };
            int element = 1;

            int count = 0;

            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == element)
                    count++;

            Console.WriteLine("Element " + element + " occurs " + count + " times.");
        }
    }
}