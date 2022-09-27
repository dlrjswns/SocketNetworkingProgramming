﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleServer
{
    class Program
    {
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
                SocketType.Stream, ProtocolType.Tcp
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
                Console.WriteLine("Server: client accepted");

                byte[] recvBytes = new byte[1024];
                int nRecv = client.Receive(recvBytes);
                if(nRecv <= 0)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    break;
                }
            }
        }
    }
}