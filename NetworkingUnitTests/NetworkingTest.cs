using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;
using Networking.Utils;

namespace NetworkingUnitTests;

[TestClass]
public class NetworkingTest
{
    [TestMethod]
    public void TwoServersTwoPortNumbers()
    {
        ICommunicator server1 = new Server();
        string[] ipPort = server1.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        int port1 = int.Parse(ipPort[1]);
        ICommunicator server2 = new Server();
        ipPort = server2.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        int port2 = int.Parse(ipPort[1]);

        Assert.AreNotEqual(port1, port2);
    }

    [TestMethod]
    public void ServerSendBeforeStart()
    {
        ICommunicator server = new Server();
        try
        {
            server.Send("123", "456");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start server first", exception.Message);
        }
        try
        {
            server.Send("123", "456","e4e4");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start server first", exception.Message);
        }
    }

    [TestMethod]
    public void ServerSubscribeBeforeStart()
    {
        ICommunicator server = new Server();
        try
        {
            server.Subscribe(new GenericEventHandler(new()), "modName");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start server first", exception.Message);
        }
    }

    [TestMethod]
    public void ServerStopBeforeStart()
    {
        ICommunicator server = new Server();
        try
        {
            server.Stop();
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start server first", exception.Message);
        }
    }
    [TestMethod]
    public void ServerDoubleStart()
    {
        ICommunicator server = new Server();
        string ipPort = server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() );
        string ipPort1= server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() );
        Assert.AreEqual( ipPort,ipPort1);
        server.Stop();
    }

    [TestMethod]
    public void ClientSendBeforeStart()
    {
        ICommunicator client = new Client();
        try
        {
            client.Send("123", "456");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start client first", exception.Message);
        }

        try
        {
            client.Send("123", "456","789");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start client first", exception.Message);
        }
    }

    [TestMethod]
    public void ClientSubscribeBeforeStart()
    {
        ICommunicator client = new Client();
        try
        {
            client.Subscribe(new GenericEventHandler(new()), "modName");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start client first", exception.Message);
        }
    }

    [TestMethod]
    public void ClientStopBeforeStart()
    {
        ICommunicator client = new Client();
        try
        {
            client.Stop();
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start client first", exception.Message);
        }
    }

    [TestMethod]
    public void ClientStartStopWithoutServer()
    {
        ICommunicator client = new Client();
        string ret = client.Start("127.0.0.1", 99, "Client1", "UnitTestClient");
        Assert.AreEqual(ret, "failed");

        // verify if client has not started
        try
        {
            client.Stop();
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Start client first", exception.Message);
        }
    }

    [TestMethod]
    public void ClientIllegalStart()
    {
        ICommunicator client = new Client();
        try
        {
            client.Start(null, null, "testClient1", "unitTestClient");
        }
        catch (Exception exception)
        {
            Assert.AreEqual("Illegal arguments", exception.Message);
        }
    }

    [TestMethod]
    public void ClientDoubleSubscribe()
    {
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        //TODO: Verify dict
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        //TODO: Verify dict
        client.Stop();
        server.Stop();
    }

    [TestMethod]
    public void ClientDoubleStart()
    {
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        string ret = client.Start(ip, port, "testClient1", "unitTestClient");
        Assert.AreNotEqual(ret, "already started");
        ret = client.Start(ip, port, "testClient1", "unitTestClient");
        Assert.AreEqual(ret, "already started");

        client.Stop();
        server.Stop();
    }

    [TestMethod]
    public void OneClientToServer()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        server.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestServer");
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        Networking.Models.Message message = new("testMessage", "unitTestServer", ID.GetServerID(), "testClient1");
        client.Send(message.Data, "unitTestServer", message.DestID);
        //client.Send(message.Data, ID.GetNetworkingID(), ID.GetServerID());
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        client.Stop();
        server.Stop();
        Assert.IsTrue(CompareMessages(receivedMessage, message));
    }

    [TestMethod]
    public void OneClientToUnsubscribedServer()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        //server.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestServer");
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        Networking.Models.Message message = new("testMessage", "unitTestServer", ID.GetServerID(), "testClient1");
        client.Send(message.Data, message.ModuleName, message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }

        if (messages.canDequeue())
        {
            Assert.Fail("Message recieved to unsubscribed server");
        }

        client.Stop();
        server.Stop();
    }

    [TestMethod]
    public void ServerToUnsubscribedClient()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        //client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        Networking.Models.Message message = new("testMessage", "unitTestClient", "testClient1", ID.GetServerID());
        server.Send(message.Data, message.ModuleName, message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }

        if (messages.canDequeue())
        {
            Assert.Fail("Message recieved to unsubscribed client");
        }

        client.Stop();
        server.Stop();
    }
    [TestMethod]
    public void ServerToClient()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), "unitTestClient").Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        
        client.Start(ip, port, "testClient1", "unitTestClient");
        Thread.Sleep(4000);
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        Networking.Models.Message message = new("testMessage", "unitTestClient", "testClient1", ID.GetServerID());
        server.Send(message.Data,message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        client.Stop();
        server.Stop();
        Assert.IsTrue(CompareMessages(receivedMessage, message));
    }

    [TestMethod]
    public void ServerLeftBeforeClient()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        client.Subscribe(new GenericEventHandler(messageQueue: messages), ID.GetNetworkingBroadcastID());
        //Message message = new("testMessage", "unitTestServer", ID.GetServerID(), "testClient1");
        Data data = new(EventType.ServerLeft());

        Networking.Models.Message message = new(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID(), ID.GetServerID());
        //client.Send(message.Data, "unitTestServer", message.DestID);
        server.Stop();
        client.Stop();
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        
        Assert.IsTrue(CompareMessages(receivedMessage, message));
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
        Data data=new("testMessage",EventType.ChatMessage());
        Networking.Models.Message message = new(Serializer.Serialize(data), "unitTestClient", "testClient1", "testClient1");
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        client.Send(message.Data, message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        client.Stop();
        server.Stop();
        Assert.IsTrue(CompareMessages(receivedMessage, message));
    }

    [TestMethod]
    public void OneClientToItselfUnsubscribed()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        string ip = ipPort[0];
        int port = int.Parse(ipPort[1]);
        client.Start(ip, port, "testClient1", "unitTestClient");
        Networking.Models.Message message = new("testMessage", "unitTestClient", "testClient1", "testClient1");
        //client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        client.Send(message.Data, message.ModuleName, message.DestID);
        while (!messages.canDequeue())
        {
            Thread.Sleep(300);
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }
        if (messages.canDequeue())
        {
            Assert.Fail("Message received on unsubscribed client");
        }

        client.Stop();
        server.Stop();
    }

    [TestMethod]
    public void DequeingEmptyQueue()
    {
        Queue queue = new();
        queue.Dequeue();
        Assert.IsTrue(true);
        
    }

    [TestMethod]
    public void MultipleClientsSendingMessages()
    {
        const int numClients = 10;
        Queue[] messages = new Queue[numClients];
        Networking.Models.Message[] sentMessages = new Networking.Models.Message[numClients];
        ICommunicator server = new Server();
        ICommunicator[] clients = new ICommunicator[numClients];

        string[] ipPort = server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        for (int i = 0; i < numClients; i++)
        {
            clients[i] = new Client();
            clients[i].Start( ip , port , $"testClient{i + 1}" , "unitTestClient" );
            messages[i] = new Queue();
            // Subscribe each client to its own message queue
            clients[i].Subscribe( new GenericEventHandler( messageQueue: messages[i] ) , "unitTestClient" );
        }
            for (int i = 0; i < numClients; i++)
        {
            // Send a message from each client to itself
            Data data = new( "testMessage" , EventType.ChatMessage() );
            sentMessages[i] = new( Serializer.Serialize( data ) , "unitTestClient" , $"testClient{i + 1}" , $"testClient{i + 1}" );
            clients[i].Send( sentMessages[i].Data , sentMessages[i].DestID );
        }


        // Check if each client received its own message
        for (int i = 0; i < numClients; i++)
        {
            int cnt = 0;
            while (!messages[i].canDequeue())
            {
                Thread.Sleep( 300 );
                cnt++;
                if (cnt == 10)
                {
                    Assert.Fail( "Did not receive message" );
                }
            }
            Networking.Models.Message receivedMessage = messages[i].Dequeue();
            Assert.IsTrue( CompareMessages( receivedMessage , sentMessages[i] ) );
            clients[i].Stop();
        }
        server.Stop();
    }




    private bool CompareMessages(Networking.Models.Message message1, Networking.Models.Message message2)
    {
        return !((message1.Data != message2.Data) ||
            (message1.DestID != message2.DestID) ||
            (message1.SenderID != message2.SenderID) ||
            (message1.ModuleName != message2.ModuleName));
    }

    private Client[] getClientsAndStart(int n, string destIP, int destPort, string moduleName)
    {
        List<Client> clientsList = new();

        for (int i = 0; i < n; i++)
        {
            Client client = new();
            client.Start(destIP, destPort, "client_" + i.ToString(), moduleName);
            clientsList.Add(client);
        }

        return clientsList.ToArray();
    }
}
