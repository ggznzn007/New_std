using System;

namespace _IsLeapYear
{
    class IsLeap
    {
        static void Main(string[] args)
        {
            // 윤년을 찾는 프로그램
            while (true)
            {
                Console.WriteLine("\n\t\t윤년을 찾아라!!!\t\t");
                Console.WriteLine("윤년을 찾을 범위를 입력해주세요 :  ");
                int y = int.Parse(Console.ReadLine());
                for (int year = 1; year < y; year++)
                    if (IsLeapYear(year))
                        Console.Write("{0} ", year);
                Console.WriteLine();
            }
        }

        private static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }
    }
}