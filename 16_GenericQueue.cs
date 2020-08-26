using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _16_GenericQueue
{
    class Program
    {// 통신, 버퍼에서 많이 사용
        static void Main(string[] args)
        {
            Queue<String> queue = new Queue<String>();
            
            queue.Enqueue("A");
            queue.Enqueue("P");
            queue.Enqueue("P");
            queue.Enqueue("L");
            queue.Enqueue("E");

            while (queue.Count > 0)
                Console.WriteLine(queue.Dequeue());
        }
    }
}
