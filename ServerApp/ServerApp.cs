using Networking;
using Networking.Communicator; // Assuming this is the namespace of your class library
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            ICommunicator server = CommunicationFactory.GetCommunicator(true);
            server.Start(null, null,"server");
            Console.ReadKey();
            server.Send("omg", EventType.ChatMessage(), "A");
        }
    }
}
