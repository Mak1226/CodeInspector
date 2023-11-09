using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Utils;
using System;
using Networking.Events;
namespace ClientApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {

            ICommunicator server = CommunicationFactory.GetClient();
            string addr = server.Start("192.168.0.108", 12345, "hee");
            
            server.Subscribe(new ClientEvent(), "blah");
            Console.ReadKey();
            server.Send("omg", EventType.ChatMessage(), "hee");
            /*ICommunicator client = CommunicationFactory.GetClient();
            string[] address = addr.Split(':');
            client.Start(address[0], int.Parse(address[1]), "clientA");
            client.Subscribe(new Events(), "asdfasf");
            client.Send("hello", EventType.ChatMessage(), "clientA");*/
            Console.ReadKey();
            /* server.Send("omg", EventType.ChatMessage(), "A");*/

            /*client.Stop();*/
            server.Stop();

        }
    }
}
