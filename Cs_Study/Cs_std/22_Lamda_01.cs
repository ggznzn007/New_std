using System;

namespace Lamda_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = new[] { 3, 34, 6, 34, 7, 8, 24, 3, 675, 8, 23 };

            int n = Count(arr, x => x % 2 == 0);
            Console.WriteLine("짝수의 개수: " + n);

            n = Count(arr, x => x % 2 == 1);
            Console.WriteLine("홀수의 개수: " + n);
        }


    }
}