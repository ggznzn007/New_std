using System;

namespace _8_EnumInt
{
    class Program
    {
        enum SEASON { Spring, Summer, Fall, Winter}
        static void Main(string[] args)
        {
            SEASON season;
            season = SEASON.Fall;
            Console.WriteLine(season);
            int Value = (int)season;
            Console.WriteLine(Value);
        }
    }
}