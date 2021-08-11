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

/*Task 작업 취소
비동기 작업을 취소하기 위해서는 Cancellation Token을 사용하는데, 작업 취소와 관련된 타입은
CancellationTokenSource 클래스와 CancellationToken 구조체이다.
CancellationTokenSource 클래스는 Cancellation Token을 생성하고 Cancel 요청을
Cancellation Token들에게 보내는 일을 담당하고, CancellationToken은 현재 Cancel 상태를
모니터링하는 여러 Listener들에 의해 사용되는 구조체이다.
작업을 취소하는 일반적인 절차는 (1) 먼저 CancellationTokenSource 필드를 선언하고,
(2) CancellationTokenSource 객체를 생성하며, (3) 비동기 작업 메서드 안에서 작업이 취소되었는지를
체크하는 코드를 넣으며, (4) 취소 버튼이 눌러지면 CancellationTokenSource의 Cancel() 메서드를
호출해 작업 취소를 요청한다. 아래 예제는 시작버튼을 누르면 작업을 비동기 Task를 100초 동안 실행하다가
그 이전에 취소 버튼을 누르면 작업을 중간에 취소하는 코드이다.
*/

