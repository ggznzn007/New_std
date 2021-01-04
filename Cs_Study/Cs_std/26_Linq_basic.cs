/*
LINQ는 Language-INtegrated Query의 약자로 C#에 통합된 질의 기능
SQL과 같은 Query언어를 C#에 도입한 것
from, where, orderby, select 등의 연산자가 사용
=> data컬렉션에 있는 데이터 item으로 부터 item이 짝수이면 item값으로 정렬하여 선택한다는 의미
LINQ를 사용하려면 원본 데이터가 IEnumerable, IEnumerable<T> 인터페이스를 상속하는 형식이어야 한다
이에 해당하는 컬렉션은 배열과 리스트 등이 있다
*/
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

            Console.WriteLine("Using foreach: ");//lstSortedEven 출력
            foreach (var item in lstSortedEven)
                Console.Write(item + " ");
            Console.WriteLine();

            var sortedEven = from item in data//LINQ로 data에서 짝수를 찾아 정렬하여
                             where item % 2 == 0//sortedEven에 추가하고 이때 sortedEven은
                             orderby item//IEnumerable<int>형이 됩니다
                             select item;//var 대신 IEnumerable<int>로 명시

            Console.WriteLine("\nUsing Linq: ");
            foreach (var item in sortedEven)
                Console.Write(item + " ");
            Console.WriteLine();
        }
    }
}
