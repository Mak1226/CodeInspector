using Networking;
using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Utils;
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            ICommunicator server = CommunicationFactory.GetCommunicator(true);

            //Console.ReadKey();
            //server.Send("omg", EventType.ChatMessage(), "A");

            server.Start(null, null,ID.GetServerID());

            Console.ReadKey();
            server.Stop();
        }
    }
}
