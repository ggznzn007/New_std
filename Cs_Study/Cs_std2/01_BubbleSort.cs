using System;

namespace BubbleSort01
{
    class Pro
    {
        static void BubbleSort(int[] array)
        {
            int count = 0;
            for(int i = array.Length - 1;i > 0;i--)
            {
                for( int j =0; j<i;j++)
                    if(array[j]>array[j+1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        count++;
                    }
            }
        }

        static void Main(string[] args)
        {
            int[] array = { 0, 5, 1, 6, 8, 2, 3 };
            foreach (int num in array)
                Console.Write(" " + num);
            Console.WriteLine();

            BubbleSort(array);
            foreach (int num in array)
                Console.Write(" " + num);
        }
    }
}