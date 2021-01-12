using System;

namespace _Loop
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01) 1 ~ 100까지 합
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            { sum += i; }
            Console.WriteLine("1부터 100까지 숫자의 합은 {0}", sum);

            // 02) 1 ~ 100까지 홀수의 합
            int sumOdd = 0;
            for (int x = 1; x <= 100; x++)
            { if (x % 2 == 1) sumOdd += x; }
            Console.WriteLine("1부터 100까지 홀수의 합은 {0}", sumOdd);

            // 03) 1 ~ 100까지 짝수의 합
            int sumEven = 0;
            for (int y = 1; y<=100;y++)
            { if (y % 2 == 0) sumEven += y; }
            Console.WriteLine("1부터 100까지 짝수의 합은 {0}", sumEven);

            // 04) 1 ~ 100까지 실수의 합
            float sumFloat = 0;
            for (int z = 1; z<=100;z++)
            { sumFloat += z; }
            Console.WriteLine("1부터 100까지 실수의 합은 {0}", sumFloat);

            // 05) 1 ~ 100까지 역수의 합
            double sumDouble = 0;
            for(int j = 1;j<=100;j++)
            { sumDouble += 1.0 / j; }
            Console.WriteLine("1부터 100까지 역수의 합은 {0}", sumDouble);
        }
    }
}