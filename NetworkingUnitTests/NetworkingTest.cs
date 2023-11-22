using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
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
        Message message = new("testMessage", "unitTestServer", ID.GetServerID(), "testClient1");
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
        Message message = new("testMessage", "unitTestServer", ID.GetServerID(), "testClient1");
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
            Assert.Fail("Message recieved to unsubscribed server");

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
        Message message = new("testMessage", "unitTestClient", "testClient1", ID.GetServerID());
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
            Assert.Fail("Message recieved to unsubscribed client");

        client.Stop();
        server.Stop();
    }

    //[TestMethod]
    //public void ServerStopped()
    //{
    //    ICommunicator server = new Server();
    //    ICommunicator client = new Client();
    //    server.Stop();
    //    client.Send("huh", ID.GetServerID());
    //}
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

    [TestMethod]
    public void OneClientToItselfUnsubscribed()
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
            Assert.Fail("Message received on unsubscribed client");
        client.Stop();
        server.Stop();
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
        if ((message1.Data != message2.Data) ||
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
            client.Start(destIP, destPort, "client_" + i.ToString(), moduleName);
            clientsList.Add(client);
        }

        return clientsList.ToArray();
    }
}
