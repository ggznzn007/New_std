using System;

namespace Day_Week
{
    class Pro
    {
        static void Main()
        {
            DateTime date = new DateTime(2021, 3, 13);

            string day = WeekDay(date.Day, date.Month, date.Year);
            Console.WriteLine(day);
        }

        public static string WeekDay(int day, int month, int year)
        {
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday","Sunday" };
            int a = (14 - month) / 12, y = year - a, m = month + 12 * a - 2;
            return days[(7000 + (day + y + y / 4 - y / 100 + y / 100 + y / 400 + (31 * m) / 12)) % 7];
        }
    }
}