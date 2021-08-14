using System;
using System.Collections.Generic;

namespace Stack01
{
    class Program
    {
        static Stack<int> GetStack()
        {
            Stack<int> stack = new Stack<int>();
            for (int i = 1; i <= 2000; i += i)
                stack.Push(i);
            return stack;
        }

        static void Main()
        {
            var stack = GetStack();
            Console.WriteLine("---- Contents In Stack ----");
            foreach (int i in stack)
            {
                Console.WriteLine("result : " + i);
            }
        }
    }
}