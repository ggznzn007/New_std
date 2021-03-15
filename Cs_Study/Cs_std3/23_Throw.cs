using System;

namespace Throw01
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (int i = 0; i < arr.Length; i++)
            {
                try
                {
                    if (arr[i] > 5)
                        throw new Exception("The number > 5.");
                    else
                        Console.WriteLine(arr[i]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}