using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server_Practice_1.Communication
{
    class ClientHandler
    {
        private Socket clientSocket;
        private Action<string> action;
        byte[] buffer = new byte[512];
        string endMessage = "quit";

        public ClientHandler(Socket socket, Action<string> action)
        {
            this.clientSocket = socket;
            this.action = action;

            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = "";

            while (!message.Equals(endMessage))
            {
                int length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                action(message);
            }

            Close();
        }

        private void Close()
        {
            clientSocket.Close(1);
        }

        public void Send(string message)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
