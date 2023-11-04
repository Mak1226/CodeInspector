using Networking.Communicator; // Assuming this is the namespace of your class library
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start(null, null);
            Console.ReadKey();
            server.Send("omg", "1123", "A");
        }
    }
}
