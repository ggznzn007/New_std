using System;
using System.IO.Ports;  // 데이터 쓰기부분 공부해야함 

/*시리얼 포트에서 데이터 읽기 210728에 공부함

시리얼 포트에서 데이터를 읽어오는 코드는 위의 기본 절차들을 따르면 된다.
일반적으로 실무에서 시리얼포트 통신을 위해서는 비동기 방식을 사용하지만, 
여기서는 가장 간단한 예제를 들기 위해 시리얼 포트를 열고 동기적으로 데이터를 읽어 오는 코드를 살펴본다.
아래 예제는 COM5 포트를 열고, 한 라인의 데이터를 읽어 오는 코드이다.
이어 코드는 시리얼포트에서 읽은 한 라인을 출력하고 COM5 포트를 닫게 된다.

시리얼포트에서 데이터를 읽는 메서드는 여러 가지가 있다. 
즉, 한 바이트만 읽어오는 ReadByte(), 한 글자를 읽어오는 ReadChar(), 
여러 바이트들을 읽어오는 Read(), 한 라인을 읽어오는 ReadLine() 등이 있다.*/

namespace SerialApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. SerialPort 클래스 객체 생성  (COM5 포트를 사용)
            SerialPort sp = new SerialPort("COM5");

            // 2. 시리얼포트 오픈
            sp.Open();

            // 3. 시리얼포트에서 한 라인 읽기
            string data = sp.ReadLine();

            Console.WriteLine(data);

            // 4. 시리얼포트 닫기
            sp.Close();

            Console.WriteLine("Press Enter to Quit");
            Console.ReadLine();
        }
    }
}

/*시리얼 포트 기초

시리얼 포트 (Serial Port, 직렬 포트)는 한 번에 하나의 비트 단위로 정보를 주고 받을 수 있는
직렬 통신의 물리적 인터페이스로서 다양한 주변 기기와의 통신에 사용한다.
시리얼 포트는 일반적으로 RS-232 표준을 따르는 하드웨어로서 모뎀이나 직렬 마우스,
바코드 리더, 디지털 측정 장비 등 다양한 주변 기기를 시리얼 포트에 연결하여 사용할 수 있다.

.NET 에서 시리얼포트를 사용하기 위해서는 System.IO.Ports 네임스페이스 안의
SerialPort 클래스를 이용하면 된다. SerialPort 클래스를 사용하는 절차는 크게 다음과 같이 5단계로 나눌 수 있다.
(주: 아래의 1~2단계는 SerialPort 객체를 생성하면서 동시에 포트 셋팅을 지정할 수도
있기 때문에 하나로 볼 수도 있다.)

1) SerialPort 클래스 객체 생성
2) (Optional)SerialPort 포트 셋팅
3) 시리얼포트 오픈
4) 시리얼포트에서 데이타 읽거나 쓰기
5) 시리얼포트 닫기

아래 예제처럼 SerialPort 객체를 아무 파라미터 없이 생성하면 디폴트 COM 포트 셋팅을 사용한다.
(디폴트 포트 셋팅은 COM1 포트, 9600 BaudRate, 8 DataBits, None Parity, 1 StopBits 를 사용한다)*/

/*// 1. SerialPort 클래스 객체 생성 (디폴트로 포트 셋팅 사용)
SerialPort sp = new SerialPort();

// 2. 시리얼포트 오픈
sp.Open();

// 3. 시리얼포트에서 한 라인 읽기
string data = sp.ReadLine();

// 4. 시리얼포트 닫기
sp.Close();*/

