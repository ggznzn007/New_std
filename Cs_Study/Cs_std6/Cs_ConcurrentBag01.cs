using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentBag
{
    class Program
    {
        static void Main(string[] args)
        {
            var bag = new ConcurrentBag<int>();

            //데이터를 Bag에 넣는 쓰레드
            Task t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    bag.Add(i);
                    Thread.Sleep(100);
                }
            });

            // Bag에서 데이터를 읽는 쓰레드
            Task t2 = Task.Factory.StartNew(() =>
            {
                int n = 1;
                // Bag 데이터 내용 10번 출력
                while (n <= 10)
                {
                    Console.WriteLine("{0} iteration", n);
                    int cnt = 0;

                    // Bag에서 데이터 읽기
                    foreach (int i in bag)
                    {
                        Console.WriteLine(i);
                        cnt++;
                    }
                    Console.WriteLine("Cnt={0}", cnt);

                    Thread.Sleep(1000);
                    n++;
                }
            });

            // 두 쓰레드가 끝날 때까지 대기
            Task.WaitAll(t1, t2);            
        }
    }
}