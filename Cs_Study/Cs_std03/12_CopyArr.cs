using System;

namespace copyArr
{
    class Pro
    {
        static void Main()
        {
            int[] arr = { 1, 2, 43, 6, 7 };
            int startIdx = 0;

            int[] cpyArr1 = new int[arr.Length - startIdx];
            arr.CopyTo(cpyArr1, startIdx);

            int[] cpyArr2 = new int[arr.Length - startIdx];
            for(int i = startIdx,j=0;i<arr.Length;i++,j++)
            {
                cpyArr2[j] = arr[i];
            }

            Console.WriteLine("arr: ");
            for (int i = 0; i < arr.Length; i++)
                Console.Write(" " + arr[i]);
            Console.WriteLine();

            Console.WriteLine("cpyArr1: ");
            for (int i = 0; i < cpyArr1.Length; i++)
                Console.Write(" " + cpyArr1[i]);
            Console.WriteLine();

            Console.WriteLine("cpyArr2: ");
            for (int i = 0; i < cpyArr2.Length; i++)
                Console.Write(" " + cpyArr2[i]);
        }
    }
}