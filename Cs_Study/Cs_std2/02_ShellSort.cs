using System;

namespace ShellSort01
{
    class Pro
    {
        public static void ShellSort(int[] array)
        {
            int step = array.Length / 2;

            while(step>0)
            {
                for(int i = 0; i<(array.Length-step);i++)
                {
                    int j = i;
                    while(j>=0&&array[j]>array[j+step])
                    {
                        int temp = array[j];
                        array[j] = array[j + step];
                        array[j + step] = temp;
                        j--;
                    }
                }
                step = step / 2;
            }
        }

        static void Main(string[] args)
        {
            int[] array = { 0, 5, 1, 6, 8, 2, 3 };
            foreach (int num in array)
                Console.Write(" " + num);
            Console.WriteLine();

            ShellSort(array);
            foreach (int num in array)
                Console.Write("  " + num);
        }
    }
}