using System;

namespace _Recursive_Reci
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(" 1 ~ n 까지의 역수의 합을 구합니다. n 을 입력하세요: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine(" 1 ~ {0} 까지의 역수의 합: {1}", n, SumOfReci(n));
        }

        private static double SumOfReci(int n)
        {
            if (n == 1)
                return 1;
            else
                return 1.0 / n + SumOfReci(n - 1);
        }
    }
}