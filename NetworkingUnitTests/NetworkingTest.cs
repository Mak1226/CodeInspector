using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
using Networking.Utils;

namespace NetworkingUnitTests;

[TestClass]
public class NetworkingTest
{
    [TestMethod]
    public void OneClient()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = CommunicationFactory.GetServer();
        ICommunicator client = CommunicationFactory.GetClient();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        server.Subscribe( new ServerEventHandler( messageQueue: messages ) , "unitTestServer" );
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        Message message = new( "testMessage" , "unitTestServer" , ID.GetServerID() , "testClient1" );
        client.Send( message.Data , message.ModuleName , message.DestID );
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if(cnt == 10)
            {
                Assert.Fail( "Did not receive message" );
            }
        }
        Message receivedMessage = messages.Dequeue();
        client.Stop();
        server.Stop();
        Assert.IsTrue( CompareMessages(receivedMessage,message));
    }

    private bool CompareMessages(Message message1, Message message2)
    {
        if( (message1.Data != message2.Data) ||
            (message1.DestID != message2.DestID) ||
            (message1.SenderID != message2.SenderID) ||
            (message1.ModuleName != message2.ModuleName))
        {
            return false;
        }
        return true;
    }
}
