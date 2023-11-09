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
            string addr = server.Start(null, null, ID.GetServerID());
            server.Subscribe(new Events(), "asdfasf");
            Console.ReadKey();
            //Console.ReadKey();
            //server.Send("omg", EventType.ChatMessage(), "A");
            /*ICommunicator client = CommunicationFactory.GetClient();
            string[] address = addr.Split(':');
            client.Start(address[0], int.Parse(address[1]), "clientA");
            client.Send("hello", EventType.ChatMessage(), "clientA");*/
            //Console.ReadKey();
            server.Send("omg_Server", EventType.ChatMessage(), "hee");
            Console.ReadKey();

            /*client.Stop();*/
            server.Stop();

        }
    }
}
