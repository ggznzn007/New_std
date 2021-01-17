using System;

namespace _Add_m
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\t<< 두 숫자 사이의 모든 정수 값을 더하는 메소드 >>\t\n");
                Add();
            }
        }

        private static int Add()
        {
            Console.Write("\t\t원하는 두 숫자를 입력해주세요 >> : ");
            int v1 = int.Parse(Console.ReadLine());
            int v2 = int.Parse(Console.ReadLine());
            int sum = 0;
            for (int i = v1; i <= v2; i++)
                sum += i;
            Console.WriteLine("Num1 ~ Num2까지의 합: {0,8}", sum);
            Console.WriteLine();
            return sum;
        }
    }
}