using System.Net.Sockets;

namespace MessageChannel.Abstractions
{
    public interface IApplicationSender
    {
        string Send(string message);
        void SetNetworkStream(NetworkStream ns);
    }
}
