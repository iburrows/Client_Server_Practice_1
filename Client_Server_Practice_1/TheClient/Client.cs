using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server_Practice_1.TheClient
{
    class Client
    {
        private int port;
        private Action<string> updateGuiNewMessage;

        Socket clientSocket;

        byte[] buffer = new byte[512];

        string endMessage = "quit";

        public Client(int port, Action<string> updateGuiNewMessage)
        {
            this.port = port;
            this.updateGuiNewMessage = updateGuiNewMessage;

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Loopback, port);
            clientSocket = client.Client;
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = "";

            while (!message.Equals(endMessage))
            {
                int length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                updateGuiNewMessage(message);
            }

            Close();
        }

        private void Close()
        {
            clientSocket.Close(1);
        }

        public void Send(string message)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes("Client: " + message));
        }
    }
}
