using System;

namespace _Area_Cir
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n\t\t@@@ 원의 면적을 계산하는 메소드 @@@\t\t\n");
                Console.Write("원하는 면적을 정수로 입력해주세요 >> :  ");
                int n = int.Parse(Console.ReadLine());
                for (double r = 1; r <= n; r++)
                    Console.WriteLine("{0}까지의 면적은 = {1,7:F2}", r, AreaOfCircle(r));

            }
        }

        private static double AreaOfCircle(double r)
        {
            return Math.PI * r * r;
        }
    }
}