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
            Data data = new ( "what's up cuuuuh" , EventType.ChatMessage() );
            client.Send( Serializer.Serialize<Data>( data ) , Id.GetBroadcastId());
            Console.ReadKey();
            client.Stop();
            

        }
    }
}
