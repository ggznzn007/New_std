using System;

namespace _Pyramid
{
    class Pyramid
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\t\t@@@ 피라미드 만들기 @@@\t\t\n");
            Console.Write("피라미드의 범위를 입력해주세요(1 ~ n) >>  ");
            int n = int.Parse(Console.ReadLine());
            DrawPyramid(n);
        }

        static void DrawPyramid(int n)
        {
            for (int i = 1; i < n; i++)
            {
                for (int j = i; j < n; j++)
                    Console.Write(" ");
                for (int k = 1; k <= 2 * i - 1; k++)
                    Console.Write("*");
                Console.WriteLine();
            }
        }
    }
}