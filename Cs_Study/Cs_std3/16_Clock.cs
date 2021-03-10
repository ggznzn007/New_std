using System;

namespace DateClock
{
    class Pro
    {
        static void Main()
        {
            DateTime time = DateTime.Now;

            int h = time.Hour;
            int m = time.Minute;
            int s = time.Second;

            Console.WriteLine($"Time from midnight {h}:{m}:{s}");
        }
    }
}