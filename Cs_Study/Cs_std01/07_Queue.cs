using System;

namespace Queue
{
    class Node<T>
    {
        internal T value;
        internal Node<T> next;

        public Node(T value)
        {
            this.value = value;
            this.next = null;
        }
    }

    class MyQueue<T>
    {
        internal Node<T> first = null;
        internal Node<T> last = null;

        internal void EnQueue(Node<T> node)
        {
            if (last == null)
                first = last = node;
            else
            {
                last.next = node;
                last = node;
            }

        }

        internal T DeQueue()
        {
            if (first == null)
            {
                Console.WriteLine("Queue is Empty");
                return default(T);
            }
            else
            {
                T value = first.value;
                first = first.next;
                return value;
            }
        }

        internal void Print()
        {
            for (Node<T> t = first; t != null; t = t.next)
                Console.Write(t.value + " -> ");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            MyQueue<float> que = new MyQueue<float>();

            for (int i = 0; i < 5; i++)
                que.EnQueue(new Node<float>(r.Next(100) / 100.0F));
            que.Print();

            for (int i = 0; i < 3; i++)
                Console.WriteLine("DeQueue: {0}", que.DeQueue());
            que.Print();
        }
    }
}