using System;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] data = new int[] { 14, 24, 34, 52, 5, 13, 54, 100 };
            int[] data2 = new int[] { 14, 24, 34, 52, 5, 13, 54, 100 };

            int s = Sum(data);
            int avg = Avg(data);
            Console.WriteLine("Sum={0}, Avg={1}", s, avg);

            s = Sum(data2);
            avg = Avg(data2);
            Console.WriteLine("Sum={0}, Avg={1}", s, avg);
        }

        static int Sum(int[] data)
        {
            int s = 0;
            for (int i = 0; i < data.Length; i++)
            {
                s += data[i];
            }
            return s;
        }

        static int Avg(int[] data)
        {
            if (data.Length == 0) return 0;

            int sum = Sum(data);
            return sum / data.Length;
        }

    }
}