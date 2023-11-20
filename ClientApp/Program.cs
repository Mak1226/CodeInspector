using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Utils;
using System;
using Networking.Events;
using Networking.Models;
using Networking.Serialization;

namespace ClientApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {

            ICommunicator client = CommunicationFactory.GetClient();
            string addr = client.Start("127.0.0.1", 12399, "hee","client1");
            client.Subscribe(new ExampleEventHandler(), "client1");
            Console.ReadKey();
            Data data = new Data("omg1", EventType.ChatMessage());
            client.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), "hee");
            Data data1 = new Data("omg2", EventType.ChatMessage());
            client.Send(Serializer.Serialize<Data>(data1), ID.GetNetworkingID(), ID.GetServerID());

            //client.Send("omg2", EventType.ChatMessage(), "hee");
            //client.Send("omg3", EventType.ChatMessage(), "hee");
            //client.Send("omg4", EventType.ChatMessage(), "hee");
            //client.Send("omg5", EventType.ChatMessage(), "hee");
            //client.Send("omg6", EventType.ChatMessage(), "hee");
            /*ICommunicator client = CommunicationFactory.GetClient();
            string[] address = addr.Split(':');
            client.Start(address[0], int.Parse(address[1]), "clientA");
            client.Send("hello", EventType.ChatMessage(), "clientA");*/
            Console.ReadKey();
            /* server.Send("omg", EventType.ChatMessage(), "A");*/

            /*client.Stop();*/
            client.Stop();

        }
    }
}
