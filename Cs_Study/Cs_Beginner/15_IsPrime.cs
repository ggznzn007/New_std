using System;

namespace _IsPrm
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2 ~ n 까지의 소수를 찾는 프로그램
            Console.WriteLine("\t\t<<<< 소수 탐색 프로그램 >>>>\t\t");
            Console.WriteLine("원하는 정수n의 범위를 입력하세요: " + "(ex: 2 ~ n)");
            int n = int.Parse(Console.ReadLine());
            int count = 0;
            for (int i = 2; i <= n; i++)
                if (IsPrime(i))
                {
                    Console.Write("{0} ", i);
                    count++;
                }
            Console.WriteLine("\n2 ~ n까지 소수는 모두 {0}개 있습니다.", count);
        }

        private static bool IsPrime(int x)
        {
            for (int i = 2; i < x; i++)
            {
                if (x % i == 0)
                    return false;
            }
            return true;
        }
    }
}