using System;

namespace _LeapYear
{
    class Program
    {
        static void Main(string[] args)
        {
            int year = int.Parse(Console.ReadLine());

            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                Console.WriteLine("{0}는 윤년입니다.", year);
            else
                Console.WriteLine("{0}는 평년입니다.", year);

            if (DateTime.IsLeapYear(year))
                Console.WriteLine("{0}은 윤년", year);
            else
                Console.WriteLine("{0}은 평년", year);
        }
    }
}