using System;
using System.Collections.Generic;// Queue<T>를 사용 할때 제네릭 네임스페이스가 필요함

namespace Queue_T
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> que = new Queue<string>();//큐 객체 생성
            que.Enqueue("Tiger");
            que.Enqueue("Lion");
            que.Enqueue("Zebra");
            que.Enqueue("Cow");
            que.Enqueue("Rabbit");
            PrintQueue("que: ", que);//5개의 값을 저장하고 출력

            Console.WriteLine(" Dequeuing '{0}'", que.Dequeue());
            //디큐를 사용하여 값을 가져오고 맨 앞의 값의 타이거가 리턴 후 없어짐
            Console.WriteLine(" Peek: '{0}'", que.Peek());
            //픽을 사용하여 맨 앞의 값을 출력하지만 값은 없어지지않음

            Queue<string> que2 = new Queue<string>(que.ToArray()); //2번째 큐 객체 생성
            PrintQueue("que2:", que2);

            string[] array = new string[que.Count];// 큐의 크기로 배열을 생성
            que.CopyTo(array, 0);//큐의 값을 배열에 복사
            Queue<string> que3 = new Queue<string>(array);// 복사한 배열로 3번째 큐 객체 생성
            PrintQueue("que3:", que3);

            Console.WriteLine("que.Contains(Lion) = {0}", que.Contains("Lion"));
            que3.Clear();
            Console.WriteLine("Count = {0}, {1}, {2}", que.Count, que2.Count, que3.Count);

        }

        private static void PrintQueue(string s, Queue<string> q)
        {
            Console.Write("{0,8}", s);
            foreach (var item in q)
                Console.Write("{0,-8}", item);
            Console.WriteLine();
        }
    }
}