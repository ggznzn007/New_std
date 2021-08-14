using System;
using System.Collections;

namespace Queue01
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue que = new Queue();
            for (int i = 1; i <= 100; i++)
            {
                que.Enqueue(i);
            }
            Console.WriteLine("que.Count: {0}", que.Count);
            Console.WriteLine();

            while (que.Count > 0)
            {
                Console.WriteLine(que.Dequeue());
            }
            Console.WriteLine();
            Console.WriteLine("que.Count: {0}", que.Count);
        }
    }
}