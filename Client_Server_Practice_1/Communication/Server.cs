using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server_Practice_1.Communication
{
    class Server
    {
        private int port;
        private Action<string> updateGuiNewMessage;

        Socket serverSocket;
        List<ClientHandler> ClientList = new List<ClientHandler>();
        

        public Server(int port, Action<string> updateGuiNewMessage)
        {
            this.port = port;
            this.updateGuiNewMessage = updateGuiNewMessage;

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, port));
            serverSocket.Listen(5);
            Task.Factory.StartNew(Accept);
        }

        private void Accept()
        {
            while (true)
            {
                ClientList.Add(new ClientHandler(serverSocket.Accept(), new Action<string>(UpdateGuiNewMessage)));
            }
        }

        private void UpdateGuiNewMessage(string message)
        {
            updateGuiNewMessage(message);
        }

        public void Send(string message)
        {
            //updateGuiNewMessage(message);
            foreach (var client in ClientList)
            {
                client.Send("Server: " + message);
            }
        }
    }
}
