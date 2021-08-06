/*C# ThreadPool 사용
.NET의 Thread 클래스를 이용하여 쓰레드를 하나씩 만들어 사용하는 것이 아니라,
이미 존재하는 쓰레드풀에서 사용가능한 작업 쓰레드를 할당 받아 사용하는 방식이 있는데,
이는 다수의 쓰레드를 계속 만들어 사용하는 것보다 효율적이다. 이렇게 시스템에 존재하는 
쓰레드풀에 있는 쓰레드를 사용하기 위해서는 (1) ThreadPool 클래스, 
(2) 비동기 델리게이트(Asynchronous delegate), (3).NET 4 Task 클래스,
(4) .NET 4 Task<T> 클래스, (5) BackgroundWorker 클래스 등을 사용할 수 있다.
이 중 ThreadPool 클래스의 경우, ThreadPool.QueueUserWorkItem() 를 사용하여 실행하고자 하는
메서드 델리게이트를 지정하면 시스템풀에서 쓰레드를 할당하여 실행하게 된다. 
이 방식은 실행되는 메서드로부터 리턴 값을 돌려받을 필요가 없는 곳에 주로 사용된다.
리턴값이 필요한 경우는 비동기 델리게이트(Asynchronous delegate)를 사용한다.*/

using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // 쓰레드풀에 있는 쓰레드를 이용하여
        // Calc() 메서드 실행.
        // 리턴값 없을 경우 사용.
        ThreadPool.QueueUserWorkItem(Calc); // radius=null
        ThreadPool.QueueUserWorkItem(Calc, 10.0); // radius=10
        ThreadPool.QueueUserWorkItem(Calc, 20.0);

        Console.ReadLine();
    }

    static void Calc(object radius)
    {
        if (radius == null) return;

        double r = (double)radius;
        double area = r * r * 3.14;
        Console.WriteLine("r={0}, area={1}", r, area);
    }
}