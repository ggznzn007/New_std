using System;
using System.Linq;

namespace LINQ01_Where
{
    class Program
    {
        static void Main()
        {
            int[] array = { 1, 2, 3, 4, 5, 15, 23, 33, 56, 444, 333, 66, 345, 356, 1, 4, 22, 65 };
            var answer = from num in array
                         where num < 100
                         select num;

            foreach (int num in answer)
                Console.Write("  " + num);
        }
    }
}