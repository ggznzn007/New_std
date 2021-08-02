using System;
using System.Threading;

/*C# 쓰레스 생성의 다양한 예제
이 섹션은 .NET의 Thread 클래스를 이용해 쓰레드를 만드는 다양한 예를 들고 있다.
Thread클래스의 생성자가 받아들이는 파라미터는 ThreadStart 델리게이트와
ParameterizedThreadStart 델리게이트가 있는데, 이 섹션은 파라미터를 직접 전달하지 않는 메서드들에
사용하는 ThreadStart 델리게이트 사용 예제를 보여준다.
ThreadStart 델리게이트는 public delegate void ThreadStart(); 와 같이 정의되어 있는데,
리턴값과 파라미터 모두 void임을 알 수 있다. 따라서 파라미터와 리턴값이 없는 메서드는
델리게이트 객체로 생성될 수 있다. 아래 예에서 보이듯이, ThreadStart 델리게이트를 만족하는 다른 방식들
즉, 익명 메서드, 람다식 등도 모두 사용할 수 있다.*/

namespace OtherThdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Run 메서드를 입력받아
            // ThreadStart 델리게이트 타입 객체를 생성한 후
            // Thread 클래스 생성자에 전달
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            // 컴파일러가 Run() 메서드의 함수 프로토타입으로부터
            // ThreadStart Delegate객체를 추론하여 생성함
            Thread t2 = new Thread(Run);
            t2.Start();

            // 익명메서드(Anonymous Method)를 사용하여
            // 쓰레드 생성
            Thread t3 = new Thread(delegate ()
            {
                Run();
            });
            t3.Start();

            // 람다식 (Lambda Expression)을 사용하여
            // 쓰레드 생성
            Thread t4 = new Thread(() => Run());
            t4.Start();

            // 간략한 표현
            new Thread(() => Run()).Start();
        }

        static void Run()
        {
            Console.WriteLine("Run");
        }
    }
}