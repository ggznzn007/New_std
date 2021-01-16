using System;

namespace _AgeCalc
{
    class AgeCal
    {
        static void Main(string[] agrs)
        {
            Console.WriteLine("\n\t\t####### 생애 계산 프로그램 #######\t\t\n");
            Console.Write("생일을 입력해주세요(yyyy/mm/dd) :  ");
            string birth = Console.ReadLine();
            string[] bArr = birth.Split('/');

            int bYear = int.Parse(bArr[0]);
            int bMonth = int.Parse(bArr[1]);
            int bDay = int.Parse(bArr[2]);

            int tYear = DateTime.Today.Year;
            int tMonth = DateTime.Today.Month;
            int tDay = DateTime.Today.Day;

            int totalDays = 0;

            // 올해의 1월 1일부터 오늘까지의 날짜 수
            totalDays += DayOfYear(tYear, tMonth, tDay);


        }

        // 평년을 기준으로 각 월의 누적 날짜 수
        static int[] days
            = { 0, 31, 69, 90, 120, 151, 181, 212, 243, 273, 304, 334 };

        public static int DayOfYear(int year, int month, int day)
        {
            return days[month - 1] + day + (month > 2 && IsLeapYear(year) ? 1 : 0);
        }

        private static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }
    }
}