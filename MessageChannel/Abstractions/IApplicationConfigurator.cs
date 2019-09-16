using System.Net.Sockets;
using MessageChannel.Models;

namespace MessageChannel.Abstractions
{
    public interface IApplicationConfigurator
    {
        MessageResult RegisterUser(string name);
        MessageResult ConnectToServer();
        TcpClient GetClient();
    }
}
