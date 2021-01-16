using System;

namespace _Constr
{
    class Date
    {
        private int year, month, day;

        public Date()
        {
            year = 1;
            month = 1;
            day = 1;
        }

        public Date(int y, int m, int d)
        {
            year = y;
            month = m;
            day = d;
        }

        public void PrintDay()
        {
            Console.WriteLine("{0}/{1}/{2}", year, month, day);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Date birthday = new Date(2000, 11, 22);
            Date chrismas = new Date(2021, 12, 25);
            Date firstDay = new Date();

            birthday.PrintDay();
            chrismas.PrintDay();
            firstDay.PrintDay();
        }
    }
}