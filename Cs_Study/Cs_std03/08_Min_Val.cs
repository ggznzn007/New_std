using System;
using System.Linq;

namespace Min_Val
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 0, 12, 34, 3, 5, 6, 1 };

            int minVal = arr.Min();

            int min = arr[0];

            for (int i = 1; i < arr.Length; i++)
                if (arr[i] < min)
                    min = arr[i];

            Console.WriteLine("Min val: " + minVal);
            Console.WriteLine("Min val: " + min);
        }
    }
}