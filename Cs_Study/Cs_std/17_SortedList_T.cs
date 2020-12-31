using System;
using System.Collections.Generic;

namespace SortedList_T
{
    class Program
    {
        static void Main(string[] args)
        {// 제네릭 소트리스트 객체 생성 후 4개의 키-값 쌍을 저장
            SortedList<int, string> s1 = new SortedList<int, string>();
            s1.Add(3, "Three");
            s1.Add(4, "Four");
            s1.Add(1, "One");
            s1.Add(2, "Two");

            for (int i = 0; i < s1.Count; i++)//s1에 저장되어있는 각 요소를 출력 **키에 의해 정렬됨
                Console.Write("k: {0}, v: {1} / ", s1.Keys[i], s1.Values[i]);
            Console.WriteLine();

            foreach (var kvp in s1)//s1 모든 요소 출력
                Console.Write("{0, -10} ", kvp);
            Console.WriteLine();

            SortedList<string, int> s2 = new SortedList<string, int>();//s2 객체 생성 후 4개 값 저장
            s2.Add("one", 1);
            s2.Add("two", 2);
            s2.Add("three", 3);
            s2.Add("four", 4);

            Console.WriteLine(s2["two"]);// 다음과 같은 방법으로 키에 해당하는 값 출력가능

            foreach (var kvp in s2)//s2 각 요소 출력 **키가 string타입이므로 사전식으로 정렬되어 출력
                Console.WriteLine("{0, -10} ", kvp);
            Console.WriteLine();

            int val;
           /*s2.TryGetValue("ten", out val)메소드는 s2에서 ten을 키로 하는 값이 있으면 변수 val에 값을 저장하고
                true를 리턴하고, ten을 키로 하는 값이 없으면 false를 리턴*/
            
            if (s2.TryGetValue("ten", out val))
                Console.WriteLine("key: ten, value: {0}", val);
            else
                Console.WriteLine("[ten] : Key is not valid.");

            //one을 키로 하여 값을 val에 저장하고 val는 1이 되고 출력
            if (s2.TryGetValue("one", out val))
                Console.WriteLine("key: one, value: {0}", val);

            //매개변수를 키로 하는 요소가 있으면 return true 아니면 return false
            Console.WriteLine(s2.ContainsKey("one")); 
            Console.WriteLine(s2.ContainsKey("ten"));
            //매개변수를 값으로 하는 요소가 있으면 return true 아니면 return false
            Console.WriteLine(s2.ContainsValue(2)); 
            Console.WriteLine(s2.ContainsValue(6));

            s2.Remove("one"); //키가 'one'인 요소 삭제 
            s2.RemoveAt(0); //첫번째 요소 삭제 

            foreach (KeyValuePair<string, int> kvp in s2)
                Console.Write("{0, -10} ", kvp);
            Console.WriteLine();
        }
    }
}