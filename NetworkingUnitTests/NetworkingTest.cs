using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
using Networking.Utils;

namespace NetworkingUnitTests;

[TestClass]
public class NetworkingTest
{
    [TestMethod]
    public void ServerSendBeforeStart()
    {
        ICommunicator server = new Server();
        Exception exception = Assert.ThrowsException<Exception>(() => server.Send("123", "456"));
        Assert.AreEqual("Start server first", exception.Message);
    }

    [TestMethod]
    public void ServerSubscribeBeforeStart()
    {
        ICommunicator server = new Server();
        Exception exception = Assert.ThrowsException<Exception>(() => server.Subscribe(new GenericEventHandler(new()),"modName"));
        Assert.AreEqual("Start server first", exception.Message);
    }

    [TestMethod]
    public void ServerStopBeforeStart()
    {
        ICommunicator server = new Server();
        Exception exception = Assert.ThrowsException<Exception>(server.Stop);
        Assert.AreEqual("Start server first", exception.Message);
    }

    [TestMethod]
    public void ClientStopBeforeStart()
    {
        ICommunicator server = new Server();
        _ = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');

        ICommunicator client = new Client();

        Exception exception = Assert.ThrowsException<Exception>(client.Stop);
        Assert.AreEqual("Start client first", exception.Message);
    }

    [TestMethod]
    public void ClientSubscribeBeforeStart()
    {
        ICommunicator server = new Server();
        _ = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');

        ICommunicator client = new Client();

        Exception exception = Assert.ThrowsException<Exception>(() => client.Subscribe(new GenericEventHandler(new()), "modName"));
        Assert.AreEqual("Start client first", exception.Message);
    }

    [TestMethod]
    public void ClientSendBeforeStart()
    {
        ICommunicator server = new Server();
        _ = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');

        ICommunicator client = new Client();

        Exception exception = Assert.ThrowsException<Exception>(() => client.Send("123", "456"));
        Assert.AreEqual("Start client first", exception.Message);
    }

    [TestMethod]
    public void OneClientToServer()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = CommunicationFactory.GetServer();
        ICommunicator client = CommunicationFactory.GetClient();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestServer" );
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

    [TestMethod]
    public void OneClientToItself()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = CommunicationFactory.GetServer();
        ICommunicator client = CommunicationFactory.GetClient();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        Message message = new("testMessage", "unitTestClient", "testClient1", "testClient1");
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        client.Send(message.Data, message.ModuleName, message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Message receivedMessage = messages.Dequeue();
        client.Stop();
        server.Stop();
        Assert.IsTrue(CompareMessages(receivedMessage, message));
    }


    //[TestMethod]
    public void ManyClientsToServer()
    {
        int NUMCLIENTS = 3;
        ICommunicator server = CommunicationFactory.GetServer();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);

        Client[] clients = getClientsAndStart(NUMCLIENTS, ip, port, "unitTestClient");
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

    private Client[] getClientsAndStart(int n, string destIP, int destPort, string moduleName)
    {
        List<Client> clientsList = new();

        for (int i = 0; i < n; i++)
        {
            Client client = new();
            client.Start(destIP, destPort, "client_"+i.ToString(), moduleName);
            clientsList.Add(client);
        }

        return clientsList.ToArray();
    }
}
