using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Program
    {
        private static Server _server;
        static void Main()
        {
            Console.WriteLine("Starting SERVER...");
            _server = new Server(64532);
            _server.Start();
            Console.WriteLine("The SERVER has started. Listening for clients...");

            
        }
    }
}