using MessageChannel.Abstractions;
using MessageChannel.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace MessageChannel.Implementations
{
    public class ApplicationConfigurator : IApplicationConfigurator
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 5000;
        private readonly TcpClient client = new TcpClient();
        private string userName = string.Empty;

        IPEndPoint epUDP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 100);

        private readonly UdpClient server = new UdpClient();
        public MessageResult RegisterUser(string name)
        {
            try
            {
                userName = name;

                server.Connect(epUDP);

                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();

                binFormatter.Serialize(mStream, name);
                var text = mStream.ToArray();

                server.Send(text, text.Length);

                var result = server.Receive(ref epUDP);

                var stream1 = new MemoryStream();
                var binaryFormatter1 = new BinaryFormatter();

                stream1.Write(result, 0, result.Length);
                stream1.Position = 0;

                var result1 = binaryFormatter1.Deserialize(stream1) as object;

                var messageResult = new MessageResult();

                if ((bool)result1)
                {
                    messageResult.Text = "Successfully added";
                    messageResult.Type = MessageType.Info;
                    messageResult.isSuccess = (bool)result1;
                }
                else
                {
                    messageResult.Text = "Error! Name already exists.";
                    messageResult.Type = MessageType.Info;
                    messageResult.isSuccess = (bool)result1;
                }

                return messageResult;
            }
            catch (Exception ex)
            {
                return new MessageResult
                {
                    Text = ex.Message,
                    Type = MessageType.Error,
                    isSuccess = false

                };
            }
        }

        public MessageResult ConnectToServer()
        {
            try
            {
                client.Connect(ip, port);

                
                return new MessageResult
                {
                    isSuccess = true,
                    Text = "Connected",
                    Type = MessageType.Info
                };
            }
            catch (Exception ex)
            {
                return new MessageResult
                {
                    isSuccess = false,
                    Text = ex.Message,
                    Type = MessageType.Error
                };
            }
        }
       

        public TcpClient GetClient()
        {
            return this.client;
            }
    }
}
