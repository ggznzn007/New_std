/*C# Task<T> 클래스

Non-Generic 타입인 Task 클래스는 ThreadPool.QueueUserWorkItem()과 같이 리턴값을 쉽게 돌려 받지 못한다.
비동기 델리게이트(Asynchronous Delegate)와 같이 리턴값을 돌려 받기 위해서는 Task<T> 클래스를 사용한다.
Task<T> 클래스의 T는 리턴 타입을 가리키는 것으로 리턴값은 Task객체 생성 후 Result 속성을 참조해서 얻게 된다.
Result 속성을 참조할 때 만약 작업 쓰레드가 계속 실행 중이면, 결과가 나올 때까지 해당 쓰레드를 기다리게 된다.*/

namespace MultiThrdApp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            // Task<T>를 이용하여 쓰레드 생성과 시작
            Task<int> task = Task.Factory.StartNew<int>(() => CalcSize("Hello World"));

            // 메인쓰레드에서 다른 작업 실행
            Thread.Sleep(1000);

            // 쓰레드 결과 리턴. 쓰레드가 계속 실행중이면
            // 이곳에서 끝날 때까지 대기함
            int result = task.Result;

            Console.WriteLine("Result={0}", result);
        }

        static int CalcSize(string data)
        {
            string s = data == null ? "" : data.ToString();
            // 복잡한 계산 가정

            return s.Length;
        }
    }
}