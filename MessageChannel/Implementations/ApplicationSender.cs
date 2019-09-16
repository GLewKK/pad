using MessageChannel.Abstractions;
using System.Net.Sockets;
using System.Text;

namespace MessageChannel.Implementations
{
    public class ApplicationSender : IApplicationSender
    {
        private static NetworkStream networkStream;

        public string Send(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            networkStream.Write(buffer, 0, buffer.Length);

            return message;
        }

        public void SetNetworkStream(NetworkStream ns)
        {
            networkStream = ns;
        }
    }
}
