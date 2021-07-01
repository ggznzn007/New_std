using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Created SortedList with three keys and values.
            SortedList<string, int> sorted = new SortedList<string, int>();
            sorted.Add("XXXX", 3);
            sorted.Add("dot", 1);
            sorted.Add("net", 2);

            // ContainsKey 키 체크해보기.
            bool contains1 = sorted.ContainsKey("java");
            Console.WriteLine("contains java = " + contains1);

            // XXXX 값 가져오기
            int value;
            if (sorted.TryGetValue("XXXX", out value))
            {
                Console.WriteLine("XXXX key is = " + value);
            }

            // dot값 가져오기. "인덱스" 사용
            Console.WriteLine("dot key is = " + sorted["dot"]);

            // 반복문 사용 예제
            foreach (System.Collections.Generic.KeyValuePair<string, int> pair in sorted)
            {
                Console.WriteLine(pair);
            }

            // 키를 사용해서 인덱스 가져오기
            int index1 = sorted.IndexOfKey("net");
            Console.WriteLine("index of net (key) = " + index1);

            //값을 이용해서 인덱스 가져오기
            int index2 = sorted.IndexOfValue(3);
            Console.WriteLine("index of 3 (value) = " + index2);

            // 전체 갯수 보기
            Console.WriteLine("count is = " + sorted.Count);

            //temp
            string temp = Console.ReadLine();
        }
    }
}