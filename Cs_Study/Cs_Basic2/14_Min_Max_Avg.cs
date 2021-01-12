using System;

namespace _MinMaxAvg_Power
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01 Min Max Avg
            while (true)
            {
                double max = double.MinValue;
                double min = double.MaxValue;
                double sum = 0;

                for (int i = 0; i < 5; i++)
                {
                    Console.Write("학생들의 키를 입력하세요(단위: cm) : ");
                    double h = double.Parse(Console.ReadLine());
                    if (h > max)
                        max = h;
                    if (h < min)
                        min = h;
                    sum += h;
                }
                Console.WriteLine("평균: {0}cm, 최대: {1}cm, 최소: {2}cm", sum / 5, max, min);
                Console.WriteLine();

                // 02 Power x의 y승 구하기
                Console.WriteLine("x의 y승을 계산합니다.");
                Console.Write(" x를 입력하세요: ");
                int x = int.Parse(Console.ReadLine());
                Console.Write(" y를 입력하세요: ");
                int y = int.Parse(Console.ReadLine());

                int pow = 1;
                for (int j = 0; j < y; j++)
                    pow *= x;
                Console.WriteLine("{0}의 {1}승은 {2}입니다", x, y, pow);
                Console.WriteLine();
            }
        }
    }
}