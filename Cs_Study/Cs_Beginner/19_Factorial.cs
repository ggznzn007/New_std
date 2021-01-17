using System;

namespace _Facto
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("\n\t\t<< 팩토리얼 계산 메소드 >>\t\t\n");
                Console.Write("원하는 숫자를 입력해주세요 >>> :  ");
                int n = int.Parse(Console.ReadLine());
                Console.WriteLine();
                int sum = 0;
                for (int i = 1; i <= n; i++)
                {
                    sum += Factorial(i);
                    Console.WriteLine("{0,2}! : {1,10:N0}", i, Factorial(i));
                }
                Console.WriteLine();
                Console.WriteLine("1! ~ n!의 합 = {0,8:N0}", sum);
            }
        }

        private static int Factorial(int n)
        {
            int fact = 1;
            for (int i = 1; i <= n; i++)
                fact *= i;
            return fact;
        }
    }
}