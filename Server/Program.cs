using MessageChannel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    class Program
    {
        public static List<User> ActualUsers = new List<User>();
        public Unit Unit = new Unit();
        static void Main(string[] args)
        {

            while (true)
            {
                int recv;
                byte[] data = new byte[1024 * 4];

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 100);
                Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                newSocket.Bind(endPoint);
                Console.WriteLine("Waiting for client...");
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 100);
                EndPoint tmpRemote = (EndPoint)sender;

                recv = newSocket.ReceiveFrom(data, ref tmpRemote);
                Console.WriteLine($"Message received from {tmpRemote.ToString()}");


                var mStream = new MemoryStream();
                var binFormatter = new BinaryFormatter();

                mStream.Write(data, 0, data.Length);
                mStream.Position = 0;

                var result = binFormatter.Deserialize(mStream) as dynamic;

                byte[] byteArr = Unit.Execute(result);


                newSocket.Connect(tmpRemote);
                if (newSocket.Connected)
                {
                    newSocket.Send(byteArr);

                }
                newSocket.Close();
                continue;
            }
        }
    }
}
