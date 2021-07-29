using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

/*시리얼포트 작업쓰레드 활용
시리얼포트 통신에서 이벤트 핸들러를 사용하지 않고 직접 작업쓰레드를 생성해서 처리할 수도 있다.
아래 예제는 간단한 콘솔 채팅 프로그램으로 한편으로 시리얼포트 읽기를 위한 작업쓰레드를 생성하여
계속 루프를 돌며 읽기 작업을 처리하고, 다른 한편으로 메인쓰레드에서 시리얼포트에 데이타를 쓰는 작업을
할 수 있게 한 것이다. (주: 이 예제는 MSDN에서 제공하는 코드를 간결하게 Refactoring 함과 
동시에 약간의 버그를 수정한 것이다)*/

namespace SerialChat
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: SerialChat {portName}");
                Console.WriteLine("   Ex: C> SerialChat COM3");
                return;
            }
            string portName = args[0];

            // 시리얼포트 열기
            SerialPort sp = new SerialPort(portName);
            sp.Open();

            // 읽기 작업쓰레드: Read from serial port
            var cancel = new CancellationTokenSource();
            var readTask = Task.Factory.StartNew(() =>
            {
                while (!cancel.IsCancellationRequested)
                {
                    string readMsg = sp.ReadLine();
                    if (readMsg.ToLower() == "quit")
                    {
                        Environment.Exit(0);
                    }
                    Console.WriteLine(readMsg);
                }
            }, cancel.Token);

            // 메인쓰레드: Write to serial port
            while (true)
            {
                string sendMsg = Console.ReadLine();
                if (sendMsg.ToLower() == "quit")
                {
                    sp.WriteLine(sendMsg);
                    cancel.Cancel();
                    break;
                }

                sp.WriteLine(sendMsg);
            }

            // 시리얼포트 닫기
            sp.Close();
        }
    }
}
