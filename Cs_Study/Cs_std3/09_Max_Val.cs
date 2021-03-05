using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Max_Val
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 98 };

            int maxVal = arr.Max();

            int max = arr[0];
            for (int i = 1; i < arr.Length; i++)
                if (arr[i] > max)
                    max = arr[i];

            Console.WriteLine("Max val: " + maxVal);
            Console.WriteLine("Max val: " + max);
        }
    }
}