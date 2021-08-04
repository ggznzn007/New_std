using System;
using System.Threading;

/*C# Thread 클래스 파라미터 전달

Thread 클래스는 파라미터를 전달하지 않는 ThreadStart 델리게이트와 파라미터를 직접 전달하는
ParameterizedThreadStart 델리게이트를 사용할 수 있다.
ThreadStart 델리게이트는 public delegate void ThreadStart(); 프로토타입에서 알 수 있듯이,
파라미터를 직접 전달 받지 않는다.(물론 파라미터를 전달하는 방식은 있다. 아래 참조)
ParameterizedThreadStart 델리게이트는 public delegate void ParameterizedThreadStart(object obj);
로 정의되어 있는데, 하나의 object 파라미터를 전달하고 리턴 값이 없는 형식이다.
하나의 파라미터를 object 형식으로 전달하기 때문에, 여러 개의 파라미터를 전달하기 위해서는
클래스나 구조체를 만들어 객체를 생성해서 전달할 수 있다. 
파라미터의 전달은 Thread.Start() 메서드를 호출할 때 파라미터를 전달한다.
ThreadStart를 이용해 파라미터를 전달하는 방법은 일단 델리게이트 메서드는 파라미터를 받아들이지 않으므로 
그 메서드 안에서 다른 메서드를 호출하면서 파라미터를 전달하는 방식을 사용할 수 있다.
이렇게 하면 파라미터를 아래 t3의 예처럼 여러 개 전달할 수도 있다.*/

namespace Thread03
{
    class Program
    {
        static void Main(string[] args)
        {
            // 파라미터 없는 ThreadStart 사용
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            // ParameterizedThreadStart 파라미터 전달
            // Start()의 파라미터로 radius 전달
            Thread t2 = new Thread(new ParameterizedThreadStart(Calc));
            t2.Start(10.00);

            // ThreadStart에서 파라미터 전달
            Thread t3 = new Thread(() => Sum(10, 20, 30));
            t3.Start();
        }

        static void Run()
        {
            Console.WriteLine("Run");
        }

        // radius라는 파라미터를 object 타입으로 받아들임
        static void Calc(object radius)
        {
            double r = (double)radius;
            double area = r * r * 3.14;
            Console.WriteLine("r={0},area={1}", r, area);
        }

        static void Sum(int d1, int d2, int d3)
        {
            int sum = d1 + d2 + d3;
            Console.WriteLine(sum);
        }
    }
}

/*Background 쓰레드 vs Foreground 쓰레드

Thread 클래스 객체를 생성한 후 Start()를 실행하기 전에 IsBackground 속성을 true/false로 지정할 수 있는데,
만약 true로 지정하면 이 쓰레드는 백그라운드 쓰레드가 된다. 디폴트 값은 false 즉 Foreground 쓰레드이다.
백그라운드와 Foreground 쓰레드의 기본적인 차이점은 Foreground 쓰레드는 메인 쓰레드가 종료되더라도
Foreground 쓰레드가 살아 있는 한, 프로세스가 종료되지 않고 계속 실행되고,
백그라운드 쓰레드는 메인 쓰레드가 종료되면 바로 프로세스를 종료한다는 점이다.*/
