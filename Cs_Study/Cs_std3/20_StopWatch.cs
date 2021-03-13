using System;

namespace StopWatch01
{
    class Pro
    {
        static void Main()
        {
            while(true)
            {

            Console.WriteLine("Press enter to start.");

            Console.ReadLine();
            DateTime start = DateTime.Now;

            Console.WriteLine("Press enter to stop.");

            Console.ReadLine();
            DateTime stop = DateTime.Now;

            TimeSpan distance = stop - start;
            double time = distance.TotalSeconds;
            Console.WriteLine("Time distance: " + time + " seconds");
            }
        }
    }
}