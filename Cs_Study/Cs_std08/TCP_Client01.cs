using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;


/*TCP 클라이언트
TcpClient 클래스
.NET Framework에서 TCP 클라이언트 프로그램을 개발하기 위해서는 System.Net.Sockets.TcpClient 클래스를 사용할 수 있다.
TcpClient 클래스는 내부적으로 System.Net.Sockets.Socket 클래스 기능들을 사용하여 TCP 기능을 구현하고 있다.
TCP는 기본적으로 IP와 포트를 필요로 하는데, IP가 호스트까지 연결하는데 비해 TCP는 호스트내 포트까지 연결하여 해당 포트에서 기다리고 응용프로그램까지 도달한다.
*/

/*namespace Cs_std08
{
    class TCP_Client01
    {
        static void Main(string[] args)
        {
            // (1) IP 주소와 포트를 지정하고 TCP 연결 
            TcpClient tc = new TcpClient("192.168.43.149", 7000);
            //TcpClient tc = new TcpClient("localhost", 7000);

            string msg = "Hello World";
            byte[] buff = Encoding.ASCII.GetBytes(msg);

            // (2) NetworkStream을 얻어옴 
            NetworkStream stream = tc.GetStream();

            // (3) 스트림에 바이트 데이타 전송
            stream.Write(buff, 0, buff.Length);

            // (4) 스트림으로부터 바이트 데이타 읽기
            byte[] outbuf = new byte[1024];
            int nbytes = stream.Read(outbuf, 0, outbuf.Length);
            string output = Encoding.ASCII.GetString(outbuf, 0, nbytes);

            // (5) 스트림과 TcpClient 객체 닫기
            stream.Close();
            tc.Close();

            Console.WriteLine($"{nbytes} bytes: {output}");
        }
    }
}*/

/*TCP 데이타 수신
일반적으로 데이타를 송신할 때는 몇 바이트를 보내는지 정확히 알 수 있지만, 데이타를 수신할 때는 몇 바이트가 올지 알 수가 없다.
또한, 데이타를 읽을 때 상대편이 비록 한번 송신을 하더라도 실제 데이타는 여러 조각으로 나뉘어져 올 수도 있다.
NetworkStream의 Read() 메서드는 데이타가 도착할 때까지 기다렸다가 최대 버퍼 크기만큼 데이타를 읽어 들이는데,
만약 읽어 올 데이타가 버퍼보다 크거나 버퍼보다 작아도 데이타가 잘게 쪼개져 올 경우 루프를 돌며 Read() 하게 된다.
그러면 어떻게 상대가 보낸 데이타가 마지막 데이타인지 알 수 있을까? 이는 크게 2가지로 분류할 수 있는데,
만약 수신할 데이타의 사이즈를 미리 알고 있는 경우와 이를 모를 경우이다.만약 TCP 통신에 있어 상대방과 주고 받는 데이타의 크기
및 구조 등에 대해 규칙(프로토콜) 을 가지고 있다면 수신할 데이타의 크기를 미리 알고 그만큼만 읽어 들이면 된다.
예를 들어, 헤더에 차후 보낼 바이트의 크기를 보낸다던지, 메시지의 마지막에 End of Message 마크를 찍는다던지, 일정 시간 아무 데이타가 없으면
수신 종료 한다던지 하는 규칙을 가질 수 있다.만약 이러한 프로토콜이 없고 수신 데이타 크기를 미리 알 수 없다면,
상대방에서 TCP Connection을 종료했을 때 Read() 메서드가 0 을 리턴하므로 이를 체크함으로써 데이타 읽기를 종료할 수 있다.
일반적으로 서버가 Connection을 먼저 닫는지, 클라이언트가 Connection을 먼저 닫는지는 프로토콜마다 다르다. 
예를 들어, HTTP 프로토콜의 경우 서버가 먼저 TCP Connection을 닫고, 브라우저 클라이언트가 뒤따라 Connection을 닫는다.
아래 예제는 서버가 TCP Connection을 닫을 때까지 계속 데이타를 읽어 들이는 예이다.*/

namespace Cs_std08
{
    class TCP_Client01
    {
        static void Main(string[] args)
        {
            // (1) IP 주소와 포트를 지정하고 TCP 연결             
            TcpClient tc = new TcpClient("localhost", 7000);

            string msg = "Hello World";
            byte[] buff = Encoding.ASCII.GetBytes(msg);

            // (2) NetworkStream을 얻어옴 
            NetworkStream stream = tc.GetStream();

            // (3) 스트림에 바이트 데이타 전송
            stream.Write(buff, 0, buff.Length);

            // (4) 서버가 Connection을 닫을 때가지 읽는 경우
            byte[] outbuf = new byte[1024];
            int nbytes;
            MemoryStream mem = new MemoryStream();
            while ((nbytes = stream.Read(outbuf, 0, outbuf.Length)) > 0)
            {
                mem.Write(outbuf, 0, nbytes);
            }
            byte[] outbytes = mem.ToArray();
            mem.Close();

            // (5) 스트림과 TcpClient 객체 닫기
            stream.Close();
            tc.Close();

            Console.WriteLine(Encoding.ASCII.GetString(outbytes));
        }
    }
}