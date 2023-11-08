using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Utils;
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            ICommunicator server = CommunicationFactory.GetServer();
            string serverAddr=server.Start(null, null,ID.GetServerID());
            Console.ReadKey();
            ICommunicator client=CommunicationFactory.GetClient();

            client.Start()
            server.Send("omg", EventType.ChatMessage(), "A");
        }
    }
}
