using System;

namespace Remove_Element
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int element = 1;

            int[] copyArr = new int[arr.Length];
            arr.CopyTo(copyArr, 0);

            arr = new int[copyArr.Length - 1];

            bool isDeleted = false;

            for (int i = 0, j = 0; i < copyArr.Length; i++)
                if (isDeleted == false && copyArr[i] == element)
                    isDeleted = true;
                else
                {
                    arr[j] = copyArr[i];
                    j++;
                }

            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
        }
    }
}