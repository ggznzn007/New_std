using System;
using System.Collections.Generic;
using System.Linq;// Linq 추가

namespace LinqBasic
{
    class Program
    {
        static void Main(string[] args)
        {//정수 리스트 data를 정의하고 초기화
            List<int> data = new List<int> { 123, 45, 12, 89, 456, 1, 4, 74, 46 };
            List<int> lstSortedEven = new List<int>();//정렬된 짝수의 리스트를 정의

            foreach(var item in data)
            {//data리스트에 각 요소가 짝수이면 lstSortedEven에 추가한다
                if (item % 2 == 0)
                    lstSortedEven.Add(item);
            }
            lstSortedEven.Sort();//lstSortedEven을 Sort메소드를 사용하여 정렬

            Console.WriteLine("Using foreach: ");
            foreach (var item in lstSortedEven)
                Console.Write(item + " ");
            Console.WriteLine();

            var sortedEven = from item in data
                             where item % 2 == 0
                             orderby item
                             select item;

            Console.WriteLine("\nUsing Linq: ");
            foreach (var item in sortedEven)
                Console.Write(item + " ");
            Console.WriteLine();
        }
    }
}