using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main()
        {

            ICommunicator server = CommunicationFactory.GetServer();
            server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() );


            //Data data = new Data( "omg" , EventType.ChatMessage() );
            //server.Send( Serializer.Serialize<Data>( data ) , "client1" , "hee" );
            /*ICommunicator client = CommunicationFactory.GetClient();
            string[] address = addr.Split(':');
            client.Start(address[0], int.Parse(address[1]), "clientA");
            client.Send("hello", EventType.ChatMessage(), "clientA");*/
            //Console.ReadKey();
            /*server.Send("omg_Server", EventType.ChatMessage(), "hee");*/
            /*client.Stop();*/
            Console.ReadKey();
            server.Stop();



        }

    }
}
