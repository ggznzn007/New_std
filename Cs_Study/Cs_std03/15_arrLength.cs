using System;

namespace arrLength
{
    class Pro
    {
        static void Main()
        {
            int[] arrA = new int[10];
            int lengthA = arrA.Length;

            int[] arrB = { 0, 1, 2, 3, 4 };
            int lengthB = arrB.Length;

            Console.WriteLine("First arr's length: " + lengthA);
            Console.WriteLine("Second arr's length: " + lengthB);
        }
    }
}