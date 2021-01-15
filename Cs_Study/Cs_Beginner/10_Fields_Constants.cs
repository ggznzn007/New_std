using System;

namespace _Fields_Constants
{
    class Product
    {
        public string name;
        public int price;
    }

    class MyMath
    { public static double PI = 3.14;}

    class MyCalender
    {
        public const int months = 12;
        public const int weeks = 52;
        public const int days = 365;

        public const double daysPerWeek = (double)days / (double)weeks;
        public const double daysPerMonth = (double)days / (double)months;
    }

    class MemberVariables
    {
        static void Main(string[] args)
        {

        }
    }
}