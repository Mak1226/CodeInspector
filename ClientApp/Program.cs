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
        static void Main()
        {
            string clientId = "hee";
            string moduleName = "EgModule";
            HashSet<string> clients=new ();
            ICommunicator client = CommunicationFactory.GetClient();
            client.Start( "10.128.2.85" , 12399, clientId,moduleName);
            client.Subscribe( new ExampleEventHandler(clients), Id.GetNetworkingBroadcastId());
            client.Subscribe( new ExampleEventHandler(clients) , moduleName );
            Console.ReadKey();
            client.Stop();
            Data data = new ( "omg1" , EventType.ChatMessage() );
            client.Send( Serializer.Serialize<Data>( data ) , Id.GetBroadcastId());
            //Data data1 = new Data("omg2", EventType.ChatMessage());
            //client.Send(Serializer.Serialize<Data>(data1), Id.GetNetworkingBroadcastId(), Id.GetServerId());

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
            

        }
    }
}
