using System;

namespace _Recursive_Factorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\t\t$$$ 팩토리얼 재귀 메소드 $$$\t\t\n");
            Console.WriteLine("팩토리얼이란 1부터 n까지의 곱을 뜻합니다.\n");
            while (true)
            {
                Console.Write("팩토리얼을 계산합니다. 원하는 숫자를 입력하세요 >>  ");
                double m = double.Parse(Console.ReadLine());
                Console.WriteLine("\n{0}! = {1}\n", m, Fact(m));
            }
        }

        private static double Fact(double x)
        {
            // 1! = 1, n! = n(n-1)!
            if (x == 1)
                return 1;
            else
                return x * Fact(x - 1);
        }
    }
}