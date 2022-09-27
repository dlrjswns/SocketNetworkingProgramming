using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream, 
                ProtocolType.Tcp
            );

            EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 5000); // EndPoint가 상위객체이고 IPEndPoint가 하위객체
            //IPAddress.Loopback 대신에127.0.0.1을 넣어줘도됨

            try
            {
                server.Connect(serverEP);
                // server.Connect(IPAddress.Loopback, 5000)
                // server.Connect("127.0.0.1", 5000)
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            while(true)
            {
                try
                {
                    Console.WriteLine("서버로 전송할 메세지를 입력하세요(종료하려면 quit를 입력하세요)");
                    string sendingText = Console.ReadLine();
                    Console.WriteLine("전송할 메세지: " + sendingText);

                    if (sendingText.Equals("quit"))
                    {
                        server.Shutdown(SocketShutdown.Both);
                        server.Close(); // server쪽 receive 음수로 바뀜
                        break;
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(sendingText);
                    server.Send(buffer);

                    byte[] recvByte = new byte[1024];
                    int nRecv = server.Receive(recvByte);
                    string text = Encoding.UTF8.GetString(recvByte, 0, nRecv);
                    Console.WriteLine("서버가 보낸 메세지: " + text);
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("TCP is connected ");
            }
        }
    }
}
