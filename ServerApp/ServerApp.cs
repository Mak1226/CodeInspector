using Networking.Communicator; // Assuming this is the namespace of your class library
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;
using System;

namespace ServerApp
{
    class ServerApp
    {
        static void Main(string[] args)
        {
            ICommunicator communicator ;
            for(int i=0;i<1;i++)
            {
                communicator = new Server();
            }
            communicator.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() );
            
            ////return;
            //ICommunicator server = CommunicationFactory.GetServer();
            //string addr = server.Start(null, null, ID.GetServerID(),ID.GetNetworkingID());
            ///*server.Subscribe(new Events(), "asdfasf");*/
            //Console.ReadKey();
            ////Console.ReadKey();
            //Data data= new Data("omg",EventType.ChatMessage());
            //server.Send(Serializer.Serialize<Data>(data), "client1", "hee");
            ///*ICommunicator client = CommunicationFactory.GetClient();
            //string[] address = addr.Split(':');
            //client.Start(address[0], int.Parse(address[1]), "clientA");
            //client.Send("hello", EventType.ChatMessage(), "clientA");*/
            ////Console.ReadKey();
            ///*server.Send("omg_Server", EventType.ChatMessage(), "hee");*/
            //Console.ReadKey();            /*client.Stop();*/


            //server.Stop();

        }

    }
}
