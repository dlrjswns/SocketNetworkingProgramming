using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleServer
{
    class Program
    {
        public static List<Socket> clientSockets = new List<Socket>();
        static void Main(string[] args)
        {
            string myComputer = Dns.GetHostName(); // 내 컴퓨터이름을 가져와라
            Console.WriteLine(myComputer);
            IPHostEntry entry = Dns.GetHostEntry(myComputer); // 인자로 넣은 컴퓨터 주소의 ip주소를 찾겠다
            foreach(IPAddress ipAddress in entry.AddressList)
            {
                Console.WriteLine(ipAddress);
            }

            Socket server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream, 
                ProtocolType.Tcp
            );
            IPEndPoint endPoint = new IPEndPoint(
                    IPAddress.Any, 5000
            );

            server.Bind(endPoint);
            server.Listen(10);

            while(true)
            {
                Console.WriteLine("Server: waiting connection request");
                Socket client = server.Accept();
                ClientHandler cHandler = new ClientHandler(client);
                Thread t= new Thread(new ThreadStart(cHandler.chat));
                t.Start();
                Console.WriteLine("Server: client accepted"); 

                //while(true)
                //{
                //    byte[] recvBytes = new byte[1024];
                 //   int nRecv = client.Receive(recvBytes);
                 //   if (nRecv <= 0) // 상대방이 종료함
                 //   {
                 //       client.Shutdown(SocketShutdown.Both);
                 //       client.Close();
                 //       break;
                 //   }
                 //   string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv); // 한글패치, Byte배열 0~nRecv까지 -> txt
                 //   Console.WriteLine(client.RemoteEndPoint.ToString() + ":" + txt);
                 //   byte[] sendByte = Encoding.UTF8.GetBytes("서버:" + txt);
                 //   client.Send(sendByte);
                 // }           
            }
        }
    }
}

class ClientHandler
{
    Socket client = null;
    List<Socket> clientSockets = new List<Socket>();

    public ClientHandler (Socket socket, List<Socket> cList)
    {
        this.client = socket;
        this.clientSockets = cList;
    }

    public void chat()
    {
        while(true)
        {
            byte[] recvBytes = new byte[1024];
            int nRecv = client.Receive(recvBytes);
            if(nRecv <= 0)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                return;
            }
            string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
            Console.WriteLine(client.RemoteEndPoint.ToString() + ":" + txt);
            byte[] sendBytes = Encoding.UTF8.GetBytes("서버:" + txt);
            //client.Send(sendBytes);
            Broadcast(sendByte);
        }
    }
    void Broadcast(byte[] sendB)
    {
        foreach(var client in clientSockets)
        {
            if(client.Connected)
            {
                client.Send(sendB);
            } 
            else
            {
               clientSockets.Remove(client);
            }
        }
    }
}