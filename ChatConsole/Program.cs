using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using MessageChannel;
using System.Linq;

namespace ChatConsole
{
    class Program
    {
        static readonly object _lock = new object();
        //static readonly List<TcpUsers> clientList = new List<TcpUsers>();
        static readonly Dictionary<int, TcpClient> clientList = new Dictionary<int, TcpClient>();

        static void Main(string[] args)
        {
            int count = 1;

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 5000);
            ServerSocket.Start();

            while (true)
            {
                using(var context = new MessageChannelContext())
                {
                    TcpClient client = ServerSocket.AcceptTcpClient();

                    lock (_lock) clientList.Add(count, client);

                    var user = context.Users.FirstOrDefault(x => x.ChatOrder == count);

                    Console.WriteLine(!string.IsNullOrEmpty(user.UserName) ? $"{user.UserName} is connected" : "someone connected!!");

                    Thread t = new Thread(Handle_clients);
                    t.Start(count);
                    count++;
                }
            }
        }

        public static void Handle_clients(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = clientList[id];

            using (var context = new MessageChannelContext())
            {
                while (true)
                {

                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    Broadcast(data);
                    var user = context.Users.FirstOrDefault(x => x.ChatOrder == id);
                    Console.WriteLine(!string.IsNullOrEmpty(user.UserName) ? $"{user.UserName}: {data}" : data);
                }
            }

            lock (_lock) clientList.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static void Broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in clientList.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

    }
}
