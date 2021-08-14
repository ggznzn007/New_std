using System;
using System.Globalization;

namespace MonthNum
{
    class Pro
    {
        static void Main()
        {
            Console.Write("Enter the month: ");
            string month = Console.ReadLine();

            Calendar cal = new GregorianCalendar();
            string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for(int i = 0; i < months.Length;i++)
                if(months[i].Equals(month))
                {
                    Console.WriteLine(i + 1);
                    break;
                }
        }
    }
}