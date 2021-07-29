using System;
using System.IO.Ports;

/*시리얼 포트에 데이타 쓰기

시리얼 포트에 데이타를 쓰는 코드는 위의 데이타를 읽는 코드와 거의 유사한데,
차이점은 읽기 메서드 대신 쓰기 메서드를 사용한다는 점이다.
여기서는 가장 간단한 예제로 시리얼 포트를 열고 동기적으로 데이타를 쓰는 코드를 살펴본다.
아래 예제는 COM6 포트를 열고, 한 라인의 데이타를 쓰고 COM 포트를 닫는 코드이다.
시리얼포트에서 데이타를 쓰는 메서드는 문자열을 Newline 없이 쓰는 Write() 메서드와
한 라인을 쓰는 WriteLine() 등이 있다.*/

/*시리얼 포트 셋팅 옵션

SerialPort 객체를 생성할 때, 혹은 객체를 생성하고 포트를 오픈하기 전에 해당 포트에 대한 셋팅을 설정할 수 있다.
자주 사용하는 포트 셋팅 옵션으로는 포트명(PortName), BaudRate, 데이타비트(DataBits), 패러티(Parity),
스톱비트(StopBits) 등이 있는데, BaudRate, DateBits, Parity, StopBits 등의 셋팅들은
시리얼 통신을 하는 양쪽 포트에서 동일하게 설정해야 한다.
아래 예제는 SerialPort 객체를 생성한 후, 여러 포트 셋팅 옵션들을 지정하는 예이다.
많은 경우 디폴트 포트 셋팅 옵션을 많이 사용하므로 이러한 옵션들은 필요할 때만 설정하면 된다.*/
namespace SerialWrt
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. SerialPort 클래스 객체 생성  (COM6 포트를 사용)
            SerialPort sp = new SerialPort("COM6");

            // 2-1. SerialPort 포트 셋팅 설정
            sp.PortName = "COM6";
            sp.BaudRate = 9600;
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;

            // 2-2. 시리얼포트 오픈
            sp.Open();

            // 3. 시리얼포트에서 한 라인 쓰기
            sp.WriteLine("Hello World");

            // 4. 시리얼포트 닫기
            sp.Close();       
                       
        }
    }
}


