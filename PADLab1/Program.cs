using MessageChannel.Abstractions;
using MessageChannel.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PADLab1
{
    class Program
    {
        private static string name = string.Empty;
        static ConsoleEventDelegate handler;
        static TcpClient client;
        static Thread thread;
        static NetworkStream ns;
        static void Main(string[] args)
        {
            var serviceLocator = new ServiceCollection()
                .AddSingleton<IApplicationConfigurator, ApplicationConfigurator>()
                .AddSingleton<IApplicationSender, ApplicationSender>()
                .BuildServiceProvider();

            var configurator = serviceLocator.GetService<IApplicationConfigurator>();
            var sender = serviceLocator.GetService<IApplicationSender>();

            while (true)
            {
                Console.WriteLine("Select a username:");
                name = Console.ReadLine();
                var result = configurator.RegisterUser(name);
                Console.WriteLine(result.Text);
                if(result.isSuccess)
                {
                    break;
                }
            }

            var message = configurator.ConnectToServer();

            Console.WriteLine(message.Text);

            client = configurator.GetClient();

            ns = client.GetStream();
            thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);

            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);


            sender.SetNetworkStream(ns);
            string text;
            while (!string.IsNullOrEmpty((text = Console.ReadLine())))
            {

                Console.WriteLine(sender.Send(text));
            }

        }
        private static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();

            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                var message = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                if (!message.Contains(name))
                {
                    Console.WriteLine(message);
                }
            }
        }
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                client.Client.Shutdown(SocketShutdown.Send);
                thread.Join();
                ns.Close();
                client.Close();
            }
            return false;
        }


        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}
