using System;

namespace _FactPrimePI
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {// 01 Factorial
                Console.WriteLine("n!을 계산합니다.");
                Console.Write("정수 n을 입력하세요: ");
                int n = int.Parse(Console.ReadLine());
                int fact = 1;
                for (int i = 2; i <= n; i++)
                    fact *= i;
                Console.WriteLine("{0}! = {1}", n, fact);
                Console.WriteLine();

                // 02 PrimeNumber
                Console.Write("숫자를 입력하세요: ");
                int num = int.Parse(Console.ReadLine());
                int index;
                for (index = 2; index < num; index++)
                {
                    if (num % index == 0)
                    { Console.WriteLine("{0}는 소수가 아닙니다", num); break; }
                }
                if (index == num)
                    Console.WriteLine("{0}는 소수입니다", num);
                Console.WriteLine();
            }
            /*// 03 PI
            bool sign = false;
            double pi = 0;
            for (int j =1;j<=10000;j+=2)
            {
                if(sign==false)
                { pi += 1.0 / j; sign = true; }
                else
                { pi -= 1.0 / j; sign = false; }
                Console.WriteLine("j = {0}, PI = {1}", j, 4 * pi);
            }
            Console.WriteLine();*/

        }
    }
}