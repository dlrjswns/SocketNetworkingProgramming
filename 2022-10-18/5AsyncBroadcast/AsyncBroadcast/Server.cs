using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncServer
{
    public class Server
    {
        private readonly static string SEPARATOR = "::";

        public static void Main()
        {
            try
            {
                new Server().Init();  // 서버 시작
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        private List<Socket> connectedClients = new();

        public List<Socket> ConnectedClients
        {
            get => connectedClients;
            set => connectedClients = value;
        }

        private Socket ServerSocket;

        private readonly IPEndPoint EndPoint = new(IPAddress.Parse("127.0.0.1"), 5000);

        Server()  // 1 소켓 생성 @ 생성자
        {
            ServerSocket = new(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        void Init() 
        {
            ServerSocket.Bind(EndPoint);   // 2. Bind
            ServerSocket.Listen(100);      // 3. Listen
            Console.WriteLine("Waiting connection request.");

            Accept(ServerSocket);         // 4-1. Accept 호출
        }

        void Accept(Socket server)
        {
            do
            { 
                Socket client = server.Accept();  //4-2. Accept() 클라이언트와 연결  
                ConnectedClients.Add(client);
                Console.WriteLine($"Client accepted: {client.RemoteEndPoint}.");

                //


            } while(true);   // 4-3. 무한 반복
        }

        void Disconnected(Socket client)   // 6. 소켓종료
        {
            ConnectedClients.Remove(client);
            Console.WriteLine($"Client disconnected: {client.RemoteEndPoint}.");
        }

        void Received(object? sender, SocketAsyncEventArgs e)
        {
            Socket client = (Socket)sender!;

            try
            {
                byte[] data = new byte[1024];

                Socket server = (Socket)sender!;
                server.Receive(data);   // 5-1 데이터 받기

                string str = Encoding.Unicode.GetString(data);
                str = str.Replace("\0", "");
                Console.WriteLine($"Data received from {client.RemoteEndPoint}: {str}");
                Broadcast($"{client.RemoteEndPoint}{SEPARATOR}{str}");  // 5-2 데이터 보내기

                //

            }
            catch (Exception)
            {
                Disconnected(client);   // 6. 전송 오류 나면 종료
            }
        }

        void Broadcast(string msg)  // 5-2ㅡ모든 클라이언트에게 Send
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);

            foreach (Socket client in ConnectedClients.ToArray())
            {
                try
                {
                    client.Send(data);   //5-2 send
                }
                catch (Exception)
                {
                    Disconnected(client);
                }
            }
        }
    }
}