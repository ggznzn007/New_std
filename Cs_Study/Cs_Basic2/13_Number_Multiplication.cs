using System;

namespace _NumberMultiTable
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01 NumberSystem
            Console.WriteLine("{0,5} {1,8} {2,3} {3,4}", "10진수", "2진수", "8진수", "16진수");

            for (int i = 1; i <= 128; i++)
            {
                Console.WriteLine("{0,7} {1,10} {2,5} {3,6}", i,
                    Convert.ToString(i, 2).PadLeft(8, '0'),
                    Convert.ToString(i, 8),
                    Convert.ToString(i, 16));
            }

            // 02 MultiplicationTable (구구단)
            Console.WriteLine("구구단의 출력할 단수를 입력하세요 : ");
            int n = int.Parse(Console.ReadLine());
            for (int j = 1; j <= 9; j++)
            { Console.WriteLine("{0} X {1} = {2}", n, j, n * j); }
        }
    }
}