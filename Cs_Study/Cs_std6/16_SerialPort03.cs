using System;
using System.IO.Ports;

/*시리얼포트 비동기 처리
시리얼포트 통신을 통해 데이타를 처리하기 위해서는 동기적인 방식보다 비동기적인 방식이 효율적인 경우가 대부분이다.
즉 시리얼포트 읽기나 쓰기를 별도의 작업쓰레드에서 실행하게 하여 메인쓰레드가 
주변기기 통신에 묶여 있지 않게하는 것이다.
시리얼포트에서 비동기적으로 데이타를 읽기 위해서 간단하게 사용하는 방법의 하나로
DataReceived 이벤트 핸들러를 사용하는 방법이 있다.
즉, 데이타가 시리얼포트에 도착했을 때, DataReceived 이벤트핸들러에 지정된 코드가 실행되게 된다.
아래 예제는 DataReceived 이벤트핸들러를 사용하여 COM5 포트로부터 데이타가 도착하면
이를 콘솔에 출력해 주는 간단한 샘플이다.*/

namespace SerialDataRec
{
    class Program
    {
        static void Main(string[] args)
        {
            // COM5 포트 사용
            SerialPort sp = new SerialPort("COM5");

            // DataReceived 이벤트 핸들러 지정
            // 시리얼포트에 데이타가 도착하면 실행
            sp.DataReceived += (sender, e) =>
            {
                SerialPort port = (SerialPort)sender;
                // 현재까지 도착한 데이타 모두 읽기
                string data = port.ReadExisting();

                Console.WriteLine(data);
            };

            // 시리얼 포트 열기
            sp.Open();

            // Enter 누를 때까지 프로그램 계속 실행
            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();

            // 시리얼 포트 닫기
            sp.Close();
        }
    }
}