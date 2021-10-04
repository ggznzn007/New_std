using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

/*TCP 서버
TcpListener 클래스
.NET Framework에서 TCP 서버 프로그램을 개발하기 위해서는 System.Net.Sockets.TcpListener 클래스를 사용한다.
TcpListener 클래스는 내부적으로 System.Net.Sockets.Socket 클래스 기능들을 사용하여 TCP Port Listening 기능을 구현하고 있다.
TCP 서버는 TcpListener 클래스를 통해 포트를 열고 TcpListener.AcceptTcpClient() 메서드를 통해 클라이언트 접속을 대기하고 있다가 
접속 요청이 오면 이를 받아들여 TcpClient 객체를 생성하여 리턴한다. 이후 서버의 TcpClient 객체가 클라이언트와 
직접 네트워크 스트림을 통해 통신하게 된다. (참고로 AcceptTcpClient() 대신 AcceptSocket()을 사용할 수 있는데 
이를 통해 TcpClient 객체 대신 Low Level의 Socket 객체를 사용할 수 있다)*/

namespace Cs_std08
{
    class TCP_Server01
    {
        static void Main(string[] args)
        {
            // (1) 로컬 포트 7000 을 Listen
            TcpListener listener = new TcpListener(IPAddress.Any, 7000);
            listener.Start();

            byte[] buff = new byte[1024];

            while (true)
            {
                // (2) TcpClient Connection 요청을 받아들여
                //     서버에서 새 TcpClient 객체를 생성하여 리턴
                TcpClient tc = listener.AcceptTcpClient();

                // (3) TcpClient 객체에서 NetworkStream을 얻어옴 
                NetworkStream stream = tc.GetStream();

                // (4) 클라이언트가 연결을 끊을 때까지 데이타 수신
                int nbytes;
                while ((nbytes = stream.Read(buff, 0, buff.Length)) > 0)
                {
                    // (5) 데이타 그대로 송신
                    stream.Write(buff, 0, nbytes);
                }

                // (6) 스트림과 TcpClient 객체 
                stream.Close();
                tc.Close();

                // (7) 계속 반복
            }
        }
    }
}
