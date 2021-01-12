using System;

namespace _BreakFactTime_Loop
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01 Break
            int sum = 0;
            for (int i = 1; ;i++)
            {
                sum += i;
                if(sum>=10000)
                {
                    Console.WriteLine("1~{0}의 합 = {1}", i, sum);
                    break;
                }
            }

            sum = 0;
            int index = 1;
            for(;sum<10000;index++)
            { sum += index; } Console.WriteLine("1~{0}의 합 = {1}", index - 1, sum);
            Console.WriteLine();

            /*// 02 Sum of Factorial
            Console.Write("숫자를 입력하세요: ");
            int n = int.Parse(Console.ReadLine());
            int sumF = 0;
            for(int j=1;j <=n;j++)
            {
                int fact = 1;
                for(int k= 2;k<=j;k++)
                { fact *= k; } sumF += fact;
                Console.WriteLine("{0,2}! = {1,10:#,#}", j, fact);
            }
            Console.WriteLine("1! + 2! + ... + {0}! = {1:N0}\n", n, sumF);
            Console.WriteLine();*/

            // 03 TimeTable(구구단 전체 출력) 
            for(int y = 1; y <=9;y++)
            {
                for (int x = 2; x <= 9; x++)
                    Console.Write("{0}x{1}={2,2} ", x, y, x * y);
                Console.WriteLine();
            }
        }
    }
}