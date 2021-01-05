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
            //정수 리스트 data를 정의하고 초기화
            Print("data : ", data);//Print메소드를 이용해서 data 내용 출력

            var lstEven = from item in data
                          where (item > 20 && item % 2 == 0)
                          orderby item descending
                          select item;
            //data 요소 중 20보다 크고 짝수이면 lstEven에 추가하고 출력
            Print("20보다 큰 짝수 검색결과: ", lstEven);

            var lstSorted = from item in lstEven
                            orderby item ascending
                            select item * 2;
            //lstEven 요소에 2를 곱하고 오름차순으로 정렬하여 lstSorted에 추가하고 출력
            Print("2를 곱해서 오름차순 정렬: ", lstSorted);
        }

        private static void Print(string s, IEnumerable<int> data)
        {//Print 메소드 정의하고 string과 IEnumerable<int>를 매개변수로 사용
            Console.WriteLine(s);
            foreach (var i in data)
                Console.Write(" " + i);
            Console.WriteLine();
        }//문자열을 출력하고 data내용에서 해당내용 출력
    }
}