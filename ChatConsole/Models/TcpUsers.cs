using System.Net.Sockets;

namespace ChatConsole.Models
{
    public class TcpUsers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TcpClient Client { get; set; }
    }
}
