using System;
using System.Collections.Generic;

namespace SortedList_T
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList<int, string> s1 = new SortedList<int, string>();
            s1.Add(3, "Three");
            s1.Add(4, "Four");
            s1.Add(1, "One");
            s1.Add(2, "Two");

            for (int i = 0; i < s1.Count; i++)
                Console.Write("k: {0}, v: {1} / ", s1.Keys[i], s1.Values[i]);
            Console.WriteLine();

            foreach (var kvp in s1)
                Console.Write("{0, -10} ", kvp);
            Console.WriteLine();

            SortedList<string, int> s2 = new SortedList<string, int>();
            s2.Add("one", 1);
            s2.Add("two", 2);
            s2.Add("three", 3);
            s2.Add("four", 4);

            Console.WriteLine(s2["two"]);

            foreach (var kvp in s2)
                Console.WriteLine("{0, -10} ", kvp);
            Console.WriteLine();

            int val;
            if (s2.TryGetValue("ten", out val))
                Console.WriteLine("key: ten, value: {0}", val);
            else
                Console.WriteLine("[ten] : Key is not valid.");

            if (s2.TryGetValue("one", out val))
                Console.WriteLine("key: one, value: {0}", val);

            Console.WriteLine(s2.ContainsKey("one")); // return true
            Console.WriteLine(s2.ContainsKey("ten")); // return false
            Console.WriteLine(s2.ContainsKey("one")); // return true
            Console.WriteLine(s2.ContainsKey("one")); // return true
        }
    }
}