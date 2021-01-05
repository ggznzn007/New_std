using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqBasic2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> data = new List<int> { 123, 45, 12, 89, 456, 1, 4, 74, 46 };

            Print("data : ", data);

            var lstEven = from item in data
                          where (item > 20 && item % 2 == 0)
                          orderby item descending
                          select item;

            Print("20보다 큰 짝수 검색결과: ", lstEven);


        }
    }
}