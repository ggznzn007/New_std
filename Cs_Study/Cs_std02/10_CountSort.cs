using System;
using System.Linq;

namespace CountSort01
{
    class Pro
    {
         static int[] CountingSort(int[] array, int min, int max)
        {
            int[] count = new int[max - min + 1];
            int z = 0;

            for(int i = 0; i < count.Length; i++) { count[i] = 0; }
            for(int i = 0; i < array.Length; i++) { count[array[i] - min]++; }

            for(int i = min; i <=max;i++)
            {
                while(count[i-min]-->0)
                {
                    array[z] = i;
                    z++;
                }
            }
            return array;
        }

        static void Main(string[] args)
        {
            int[] array = { 0, 5, 1, 6, 8, 2, 3 };
            foreach (int num in array)
                Console.Write(" " + num);
            Console.WriteLine();

            CountingSort(array, array.Min(), array.Max());
            foreach (int num in array)
                Console.Write(" " + num);
        }
    }

}