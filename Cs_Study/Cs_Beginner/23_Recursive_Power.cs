using System;

namespace _Recursive_Power
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n\t\t<<< Power 재귀 메소드 >>>\t\t\n");
                Console.WriteLine("Power(x,y)를 계산합니다.\n");
                Console.Write(" x를 입력하세요: ");
                double x = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write(" y를 입력하세요: ");
                double y = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine(" {0}^{1} = {2}", x, y, Power(x, y));
            }
        }

        private static double Power(double x, double y)
        {
            if (y == 0)
                return 1;
            else
                return x * Power(x, y - 1);
        }
    }
}