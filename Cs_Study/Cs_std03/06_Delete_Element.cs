using System;

namespace Delete_Element
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7 };
            int idx = 3;

            int[] copyArr = new int[arr.Length];
            arr.CopyTo(copyArr, 0);

            arr = new int[copyArr.Length - 1];

            for(int i = 0, j=0;i<copyArr.Length;i++)
                if(i!=idx)
                {
                    arr[j] = copyArr[i];
                    j++;
                }

            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
        }
    }
}