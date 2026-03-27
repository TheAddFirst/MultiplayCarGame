using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CKGameServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(ServerFunc);
            serverThread.IsBackground = true;
            serverThread.Start();
            Thread.Sleep(500);

            Console.WriteLine(" *** 자동차 게임을 위한 멋진 서버 입니다. ***");
            Console.WriteLine(" => 서버를 종료하기 위해서는 아무키나 누르세요.");
            Console.ReadLine();
            serverThread.Abort();
        }

        private static void ServerFunc(object obj)
        {
            Socket srySocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any,12345);
            srySocket.Bind(endPoint);

            Console.WriteLine(" *** 게임 서버가 잘 동작중입니다. *** ");
        
            byte [] recvBytes = new byte[1024];
            EndPoint clientEP = new IPEndPoint(IPAddress.None, 0);

            while(true)
            {
                int nRecv = srySocket.ReceiveFrom(recvBytes, ref  clientEP);
                string text = Encoding.UTF8.GetString(recvBytes, 0 , nRecv);
                Console.WriteLine(" = 클라이언트에서 보내준 내용 : " + text);



                byte[] sendBytes = Encoding.UTF8.GetBytes("Success : " + text);
                srySocket.SendTo(sendBytes, clientEP);
            }
        }
    }
}
