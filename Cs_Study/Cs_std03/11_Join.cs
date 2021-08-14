using System;

namespace Join_arr
{
    class Pro
    {
        static void Main()
        {
            int[] arr1 = { 0, 1, 2, 3, 4 };
            int[] arr2 = { 5, 6, 7, 8, 9, 10 };

            int[] temArr = new int[arr1.Length];
            arr1.CopyTo(temArr, 0);

            int newLength = arr1.Length + arr2.Length;
            arr1 = new int[newLength];

            for (int i = 0; i < temArr.Length; i++)
                arr1[i] = temArr[i];

            for (int i = 0, index = temArr.Length; i < arr2.Length;i++,index++)
                arr1[index] = arr2[i];

            Console.WriteLine("New arr: ");
            for (int i = 0; i < arr1.Length; i++)
                Console.Write(" " + arr1[i]);
        }
    }
}