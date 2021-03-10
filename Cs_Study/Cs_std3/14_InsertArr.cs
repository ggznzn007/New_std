using System;

namespace InsertArr
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 0, 1, 2, 3, 4 };
            int element = 5;
            int idx = 0;

            if (idx < 0 || idx > arr.Length)
                throw new ArgumentOutOfRangeException();

            int[] tempArr = new int[arr.Length];
            arr.CopyTo(tempArr, 0);

            arr = new int[tempArr.Length + 1];

            for(int i =0, j=-1;i<arr.Length;i++)
            {
                if (i == idx)
                    arr[i] = element;
                else
                    arr[i] = tempArr[++j];
            }

            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
        }
    }
}