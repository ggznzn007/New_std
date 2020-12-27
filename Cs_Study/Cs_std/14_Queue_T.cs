using System;
using System.Collections.Generic;

namespace Queue_T
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> que = new Queue<string>();
            que.Enqueue("Tiger");
            que.Enqueue("Lion");
            que.Enqueue("Zebra");
            que.Enqueue("Cow");
            que.Enqueue("Rabbit");
            PrintQueue("que: ", que);

            Console.WriteLine(" Dequeuing '{0}'", que.Dequeue());
            Console.WriteLine(" Peek: '{0}'", que.Peek());

            Queue<string> que2 = new Queue<string>(que.ToArray());
            PrintQueue("que2:", que2);

            string[] array = new string[que.Count];
            que.CopyTo(array, 0);
            Queue<string> que3 = new Queue<string>(array);
            PrintQueue("que3:", que3);

            Console.WriteLine("que.Contains(Lion) = {0}", que.Contains("Lion"));
            que3.Clear();
            Console.WriteLine("Count = {0}, {1}, {2}", que.Count, que2.Count, que3.Count);

        }
    }
}