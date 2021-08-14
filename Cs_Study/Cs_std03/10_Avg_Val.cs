using System;
using System.Linq;

namespace Avg_Val
{
    class Pro
    {
        static void Main()
        {
            double[] arr = { 1, 3, 5, 67, 8, 76, 88, 98 };

            double avgVal = arr.Average();

            double sum = 0;

            for (int i = 0; i < arr.Length; i++)
                sum += arr[i];

            double avg = sum / arr.Length;

            Console.WriteLine("Avg counted by arr.Average: " + avgVal);
            Console.WriteLine("Avg counted manally: " + avg);
        }
    }
}