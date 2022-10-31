using System.Net;
using System.Net.Sockets;
using System.Text;


namespace AsyncClient
{
    public class Client
    {
        private readonly static string SEPARATOR = "::";

        public static void Main()
        {
            try
            {
                new Client().Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press any key to exit the program.");
            Console.ReadKey();
        }


        private Socket ClientSocket;

        private readonly IPEndPoint EndPoint = new(IPAddress.Parse("127.0.0.1"), 11_000);

        public Client()  // 1. 소켓 생성
        {
            ClientSocket = new(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        void Init()
        {
            ClientSocket.Connect(EndPoint); // 2. 서버 연결
            Console.WriteLine($"Server connected.");

            //

            Send();   // 3-1. 전송
        }


        void Received(object? sender, SocketAsyncEventArgs e)
        {
            byte[] data = new byte[1024];
            Socket server = (Socket)sender!;
            try
            {
                server.Receive(data);

                string str = Encoding.Unicode.GetString(data);
                str = str.Replace("\0", "");
                string[] msg = str.Split(SEPARATOR);
                Console.WriteLine($"{msg[0]}: {msg[1]}");

                //

            }
            catch (Exception)
            {
                Console.WriteLine($"Server disconnected.");
                server.Close();
            }
        }

        void Send()  
        {
            do     // 3-1.읽어서 전송 무한 반복
            {
                try
                {
                    string message = Console.ReadLine()!;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    ClientSocket.Send(data);
                }
                catch (Exception) { }
            } while (true);
        }
    }
}
