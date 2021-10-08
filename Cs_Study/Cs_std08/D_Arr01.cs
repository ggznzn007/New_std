using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/*동적 배열(Dynamic Array)

배열은 고정된 크기의 연속된 배열요소들의 집합이므로 배열을 초기화 할 때 총 배열 요소의 수를 미리 지정해야 한다.
하지만 경우에 따라 배열요소가 몇 개나 필요한 지 미리 알 수 없는 경우가 있으며, 중간에 필요에 따라 배열을 확장해야 하는 경우도 있다.
.NET에는 이러한 동적 배열을 지원하는 클래스로 ArrayList와 List<T>이 있다. 이들 동적 배열 클래스들은 배열 확장이 필요한 경우,
내부적으로 배열 크기가 2배인 새로운 배열을 생성하고 모든 기존 배열 요소들을 새로운 배열에 복사한 후 기존 배열을 해제한다.
동적 배열의 Time Complexity는 배열과 같이 인덱스를 통할 경우 O(1), 값으로 검색할 경우 O(n)을 갖는다.*/


namespace Cs_std08
{
    class D_Arr01
    {
        static void Main(string[] args)
        {
            var bag = new ConcurrentBag<int>();

            ArrayList myList = new ArrayList();
            myList.Add(90);
            myList.Add(88);
            myList.Add(75);

            // int로 casting
            int val = (int)myList[1];


            List<int> myList2 = new List<int>(); myList.Add(90); myList.Add(88); myList.Add(75); int val2 = myList2[1];

            SortedList<int, string> list = new SortedList<int, string>();
            list.Add(1001, "Tim");
            list.Add(1020, "Ted");
            list.Add(1010, "Kim");

            string name = list[1001];

            foreach (KeyValuePair<int, string> kv in list)
            {
                Console.WriteLine("{0}:{1}", kv.Key, kv.Value);
            }

            // 데이타를 Bag에 넣는 쓰레드
            Task t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    bag.Add(i);
                    Thread.Sleep(100);
                }
            });

            // Bag에서 데이타를 읽는 쓰레드
            Task t2 = Task.Factory.StartNew(() =>
            {
                int n = 1;
                // Bag 데이타 내용을 10번 출력함
                while (n <= 10)
                {
                    Console.WriteLine("{0} iteration", n);
                    int count = 0;

                    // Bag에서 데이타 읽기
                    foreach (int i in bag)
                    {
                        Console.WriteLine(i);
                        count++;
                    }
                    Console.WriteLine("Count={0}", count);

                    Thread.Sleep(1000);
                    n++;
                }
            });

            // 두 쓰레드가 끝날 때까지 대기
            Task.WaitAll(t1, t2);
        }
    }
}
