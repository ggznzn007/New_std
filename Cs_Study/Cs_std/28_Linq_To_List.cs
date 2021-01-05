using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqToList
{
    class Program
    {
        static void Main(string[] args)
        {// 정수리스트 lstData를 정의하고 초기화
            List<int> lstData = new List<int> { 123, 456, 132, 96, 13, 465, 321 };
            Print("Data: ", lstData);//Print메소드를 이용하여 lstData내용 출력

            List<int> lstOdd = new List<int>();// 정수리스트 lstOdd 정의
            lstOdd = SelectOddAndSort(lstData);// SelectOddAndSort메소드 호출하여 결과값 할당
            Print("Ordered Odd: ", lstOdd);//Print메소드를 이용하여 lstOdd내용 출력

            int[] arrEven;//정수배열 arrEven 정의
            arrEven = SelectEvenAndSort(lstData);// SelectEvenAndSort메소드 호출하여 결과값 할당
            Print("Ordered Even: ", arrEven);//Print메소드를 이용하여 arrEven내용 출력
        }

        private static List<int> SelectOddAndSort(List<int> lstData)
        {//SelectOddAndSort메소드 정의
            return (from item in lstData
                    where item % 2 == 1
                    orderby item
                    select item).ToList<int>();
        }//List<int>를 매개변수로 받아서 홀수인 자료를 정렬 후 ToList메소드를 통해 리스트로 변환

        private static int[] SelectEvenAndSort(List<int> lstData)
        {//SelectEvenAndSort메소드 정의
            return (from item in lstData
                    where item % 2 == 0
                    orderby item
                    select item).ToArray<int>();
        }//List<int>를 매개변수로 받아서 짝수인 자료를 정렬 후 ToArray메소드를 통해 배열로 변환

        private static void Print(string s, IEnumerable<int> data)//Print 메소드 정의
        {
            Console.WriteLine(s);
            foreach (var i in data)
                Console.Write(" " + i);
            Console.WriteLine();
        }
    }
}