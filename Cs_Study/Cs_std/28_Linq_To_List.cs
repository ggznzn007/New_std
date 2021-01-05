using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqToList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> lstData = new List<int> { 123, 456, 132, 96, 13, 465, 321 };
            Print("Data: ", lstData);

            List<int> lstOdd = new List<int>();
            lstOdd = SelectOddAndSort(lstData);
            Print("Ordered Odd: ", lstOdd);

            int[] arrEven;
            arrEven = SelectOddAndSort(lstData);
            Print("Ordered Even: ", arrEven);
        }

        private static List<int> SelectOddAndSort(List<int> lstData)
        {
            return (from item in lstData
                    where item % 2 == 1
                    orderby item
                    select item).ToList<int>();
        }
    }
}