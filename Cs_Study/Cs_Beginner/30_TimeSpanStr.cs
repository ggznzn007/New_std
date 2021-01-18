using System;

namespace _TimeSpanStr
{
    class Pro
    {
        static void Main(string[] args)
        {
            DateTime christmas = new DateTime(2021, 12, 25);
            DateTime newYearDay = new DateTime(2022, 1, 1);

            TimeSpan span = newYearDay - christmas;
            //TimeSpan span = newYearDay.Subtract(christmas);
            Console.WriteLine("크리스마스와 1월 1일의 시간 간격");
            Console.WriteLine("{0,14}", span);
            Console.WriteLine("{0,14} days", span.Days);
            Console.WriteLine("{0,14} hours", span.Hours);
            Console.WriteLine("{0,14} minutes", span.Minutes);
            Console.WriteLine("{0,14} seconds", span.Seconds);
            Console.WriteLine("{0,14} milliseconds", span.Milliseconds);

            Console.WriteLine("또는");
            Console.WriteLine("{0,14}", span);
            Console.WriteLine("{0,14} days", span.TotalDays);
            Console.WriteLine("{0,14} hours", span.TotalHours);
            Console.WriteLine("{0,14} minutes", span.TotalMinutes);
            Console.WriteLine("{0,14} seconds", span.TotalSeconds);
            Console.WriteLine("{0,14} milliseconds", span.TotalMilliseconds);
            Console.WriteLine("{0,14} ticks", span.Ticks);
        }
    }
}