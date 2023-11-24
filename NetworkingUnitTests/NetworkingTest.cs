/******************************************************************************
* Filename    = NetworkingTest.cs
*
* Author      = VM Sreeram & Shubhang Kedia
*
* Product     = Analyzer
* 
* Project     = NetworkingUnitTests
*
* Description = Unit test for Networking Module
*****************************************************************************/
using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;
using Networking.Utils;

namespace NetworkingUnitTests;

/// <summary>
/// Unit tests for networking-related functionalities.
/// </summary>
[TestClass]
public class NetworkingTest
{
    /// <summary>
    /// Test for deserialization failure with invalid JSON.
    /// </summary>
    [TestMethod]
    public void DeserializationFailure()
    {
        string invalidJson = "invalid_json_string";
        object result = Serializer.Deserialize<object>( invalidJson );
        Assert.AreEqual( default , result );
    }

    /// <summary>
    /// Test to ensure two servers get different port numbers.
    /// </summary>
    [TestMethod]
    public void TwoServersTwoPortNumbers()
    {
        ICommunicator server1 = new Server();
        string[] ipPort = server1.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        int port1 = int.Parse(ipPort[1]);
        ICommunicator server2 = new Server();
        ipPort = server2.Start(null, null, ID.GetServerID(), ID.GetNetworkingID()).Split(':');
        int port2 = int.Parse(ipPort[1]);
        server1.Stop();
        server2.Stop();
        Assert.AreNotEqual(port1, port2);
    }

    /// <summary>
    /// Test to check server behavior when attempting to send before starting.
    /// </summary>
    [TestMethod]
    public void ServerSendBeforeStart()
    {
        Server server = new Server();
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
        try
        {
            server.Send( "123" , "456" , "e4e4","fsdc" );
        }
        catch (Exception exception)
        {
            Assert.AreEqual( "Start server first" , exception.Message );
        }
    }

    /// <summary>
    /// Test to check server behavior when attempting to subscribe before starting.
    /// </summary>
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

    /// <summary>
    /// Test to check server behavior when attempting to stop before starting.
    /// </summary>
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

    /// <summary>
    /// Test to ensure double start of the server results in the same IP:Port.
    /// </summary>
    [TestMethod]
    public void ServerDoubleStart()
    {
        ICommunicator server = new Server();
        string ipPort = server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() );
        string ipPort1= server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() );
        Assert.AreEqual( ipPort,ipPort1);
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure attempting to send a message from the client before starting it raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure attempting to subscribe to an event before starting the client raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure attempting to stop the client before starting it raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure attempting to start and stop the client without a server raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure attempting to start the client with illegal arguments raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure attempting to double subscribe to a client raises the correct exception.
    /// </summary>
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
        client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        client.Stop();
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that subscribing to the same event on the server twice raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ServerDoubleSubscribe()
    {
        Queue messages = new();
        ICommunicator server = new Server();
        string[] _ = server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() ).Split( ':' );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that attempting to start a client twice raises the correct exception.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that an exception in the message handling on the server is properly received by the client.
    /// </summary>
    [TestMethod]
    public void OneClientToServerWithExceptionInHandle()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();
        string[] ipPort = server.Start( null , null , ID.GetServerID() , ID.GetNetworkingID() ).Split( ':' );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestServer" );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );
        Networking.Models.Message message = new( "Throw" , "unitTestServer" , ID.GetServerID() , "testClient1" );
        client.Send( message.Data , "unitTestServer" , message.DestID );
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail( "Did not receive message" );
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        Assert.IsTrue( CompareMessages( receivedMessage , message ) );
        client.Stop();
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that a client can send a message to the server, and the server properly receives it.
    /// </summary>
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
        client.Send( message.Data , "NOTunitTestServer" , message.DestID );
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
        Assert.IsTrue(CompareMessages(receivedMessage, message));
        client.Stop();
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that a client's message sent to an unsubscribed server isn't recieved.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that a message sent from the server to an unsubscribed client is not received.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that a message sent from the server to a subscribed client is properly received.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that a server leaving before the client results in the client receiving the appropriate message.
    /// </summary>
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

        while (!messages.canDequeue())
        {
            Thread.Sleep(400);
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail("Did not receive message");
            }
        }
        Networking.Models.Message receivedMessage = messages.Dequeue();
        client.Stop();
        Assert.IsTrue(CompareMessages(receivedMessage, message));
    }

    /// <summary>
    /// Test method to ensure that a client can send a message to itself and receive it.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that an unsubscribed client does not receive a message sent to itself.
    /// </summary>
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

    /// <summary>
    /// Test method to ensure that dequeuing an empty queue does not throw an exception.
    /// </summary>
    [TestMethod]
    public void DequeingEmptyQueue()
    {
        Queue queue = new();
        queue.Dequeue();
        Assert.IsTrue(true);
        
    }

    /// <summary>
    /// Test method to ensure that multiple clients can send messages to themselves and receive them.
    /// </summary>
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



    /// <summary>
    /// Compares two messages to check if their data, destination ID, sender ID, and module name are equal.
    /// </summary>
    /// <param name="message1">The first message to compare.</param>
    /// <param name="message2">The second message to compare.</param>
    /// <returns>True if the messages are equal, false otherwise.</returns>
    private bool CompareMessages(Networking.Models.Message message1, Networking.Models.Message message2)
    {
        return !((message1.Data != message2.Data) ||
            (message1.DestID != message2.DestID) ||
            (message1.SenderID != message2.SenderID) ||
            (message1.ModuleName != message2.ModuleName));
    }

}
