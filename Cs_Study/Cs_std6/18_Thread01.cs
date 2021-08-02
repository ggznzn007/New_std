using System;
using System.Threading;

/*C# 쓰레드의 생성 (Thread 클래스)
C#에서 쓰레드를 만드는 기본적인 클래스로 System.Threading.Thread라는 클래스가 있다.
이 클래스의 생성자(Constructor)에 실행하고자 하는 메서드를 델리게이트로 지정한 후,
Thread클래스 객체에서 Start() 메서드를 호출하면 새로운 쓰레드가 생성되어 실행되게 된다.
아래 예는 동일 클래스 안의 Run() 메서드를 실행하는 쓰레드를 하나 생성한 후 실행시키는 예제이다.
예제에서는 기본적으로 생성된 메인 쓰레드에서도 동일하게 Run()메서드를 호출하고 있으므로,
Begin/End문장이 2번 출력되고 있는데, 이는 2개의 쓰레드가 동시에 한 메서드를 실행하고 있기 때문이다.*/

namespace MultiThrdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().DoTest();
        }

        void DoTest()
        {
            // 새로운 쓰레드에서 Run() 실행
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            // 메인쓰레드에서 Run() 실행
            Run();
        }

        // 출력
        // Thread#1: Begin
        // Thread#3: Begin
        // Thread#1: End
        // Thread#3: End

        void Run()
        {
            Console.WriteLine("Thread#{0}: Begin", Thread.CurrentThread.ManagedThreadId);
            // Do Something
            Thread.Sleep(3000);
            Console.WriteLine("Thread#{0}: End", Thread.CurrentThread.ManagedThreadId);
        }
    }
}