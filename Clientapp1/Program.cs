using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Utils;
using System;
using Networking.Events;
using Networking.Models;
using Networking.Serialization;
using System.Text;

namespace Clientapp1
{
    class Program
    {
        static void Main()
        {
            string clientId = "hee";
            string moduleName = "EgModule";
            HashSet<string> clients = new();
            ICommunicator client = CommunicationFactory.GetClient();
            client.Start( "192.168.0.101" , 12399 , clientId , moduleName );
            client.Subscribe( new ExampleEventHandler( clients ) , Id.GetNetworkingBroadcastId() );
            client.Subscribe( new ExampleEventHandler( clients ) , moduleName );
            Console.ReadKey();

            string data1 = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;
            //data1 += data1;

            Console.WriteLine( Encoding.UTF8.GetByteCount( data1 ) );
            Data data = new( data1 , EventType.ChatMessage() );
            client.Send( Serializer.Serialize<Data>( data ) , Id.GetBroadcastId() );
            Console.ReadKey();
            client.Stop();


        }
    }
}
