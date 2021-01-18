using System;

namespace _LifeTimeCalc
{
    class Pro
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\t\t<<< 생애 계산 프로그램 >>>\t\t\n");
            while (true)
            {
                Console.WriteLine();
                Console.Write("\n\t생년월일 시분초를 입력하세요 : ");
                DateTime birthday = DateTime.Parse(Console.ReadLine());
                DateTime now = DateTime.Now;

                TimeSpan interval = now - birthday;
                Console.WriteLine("\n\t탄생 시간: {0}", birthday);
                Console.WriteLine("\n\t현재 시간: {0}", now);
                Console.WriteLine("\n\t생존 시간: {0}", interval.ToString());
                Console.WriteLine("\n\t당신은 지금 이 순간까지 {0}일 {1}시간"
                    + " {2}분 {3}초를 살았습니다. 앞으로도 건강하고 즐겁게 살아보자구욧!!!",
                    interval.Days, interval.Hours, interval.Minutes, interval.Seconds);
            }
        }
    }
}