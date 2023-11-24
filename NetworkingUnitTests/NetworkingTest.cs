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
        string[] ipPort = server1.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        int port1 = int.Parse( ipPort[1] );
        ICommunicator server2 = new Server();
        ipPort = server2.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        int port2 = int.Parse( ipPort[1] );
        server1.Stop();
        server2.Stop();
        Assert.AreNotEqual( port1 , port2 );
    }

    /// <summary>
    /// Test to check server behavior when attempting to send before starting.
    /// </summary>
    [TestMethod]
    public void ServerSendBeforeStart()
    {
        Server server = new();
        try
        {
            server.Send( "123" , "456" );
        }
        catch (Exception exception)
        {
            Assert.AreEqual( "Start server first" , exception.Message );
        }
        try
        {
            server.Send( "123" , "456" , "e4e4" );
        }
        catch (Exception exception)
        {
            Assert.AreEqual( "Start server first" , exception.Message );
        }
        try
        {
            server.Send( "123" , "456" , "e4e4" , "fsdc" );
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
            server.Subscribe( new GenericEventHandler( new() ) , "modName" );
        }
        catch (Exception exception)
        {
            Assert.AreEqual( "Start server first" , exception.Message );
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
            Assert.AreEqual( "Start server first" , exception.Message );
        }
    }

    /// <summary>
    /// Test to ensure double start of the server results in the same IP:Port.
    /// </summary>
    [TestMethod]
    public void ServerDoubleStart()
    {
        ICommunicator server = new Server();
        string ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() );
        string ipPort1 = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() );
        Assert.AreEqual( ipPort , ipPort1 );
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure attempting to send a message from the client before starting it raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientSendBeforeStart()
    {
        // Create a new instance of the client communicator.
        ICommunicator client = new Client();

        try
        {
            // Attempt to send a message without starting the client.
            client.Send( "123" , "456" );
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Start client first" , exception.Message );
        }

        try
        {
            // Attempt to send a message with additional parameters without starting the client.
            client.Send( "123" , "456" , "789" );
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Start client first" , exception.Message );
        }
    }

    /// <summary>
    /// Test method to ensure attempting to subscribe to an event before starting the client raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientSubscribeBeforeStart()
    {
        // Create a new instance of the client communicator.
        ICommunicator client = new Client();

        try
        {
            // Attempt to subscribe to an event without starting the client.
            client.Subscribe( new GenericEventHandler( new() ) , "modName" );
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Start client first" , exception.Message );
        }
    }

    /// <summary>
    /// Test method to ensure attempting to stop the client before starting it raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientStopBeforeStart()
    {
        // Create a new instance of the client communicator.
        ICommunicator client = new Client();

        try
        {
            // Attempt to stop the client without starting it.
            client.Stop();
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Start client first" , exception.Message );
        }
    }

    /// <summary>
    /// Test method to ensure attempting to start and stop the client without a server raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientStartStopWithoutServer()
    {
        // Create a new instance of the client communicator.
        ICommunicator client = new Client();

        // Attempt to start the client without a server and check for failure.
        string ret = client.Start( "127.0.0.1" , 99 , "Client1" , "UnitTestClient" );
        Assert.AreEqual( ret , "failed" );

        // Verify if the client has not started successfully.
        try
        {
            // Attempt to stop the client without starting it.
            client.Stop();
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Start client first" , exception.Message );
        }
    }

    /// <summary>
    /// Test method to ensure attempting to start the client with illegal arguments raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientIllegalStart()
    {
        // Create a new instance of the client communicator.
        ICommunicator client = new Client();

        try
        {
            // Attempt to start the client with illegal arguments.
            client.Start( null , null , "testClient1" , "unitTestClient" );
        }
        catch (Exception exception)
        {
            // Verify that the expected exception is thrown.
            Assert.AreEqual( "Illegal arguments" , exception.Message );
        }
    }

    /// <summary>
    /// Test method to ensure attempting to double subscribe to a client raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientDoubleSubscribe()
    {
        // Create a message queue and instances of the server and client communicators.
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();

        // Start the server and client.
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );

        // Subscribe the client to an event.
        client.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );

        // Attempt to double subscribe the client and check for the expected exception.
        client.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );

        // Stop the client and server.
        client.Stop();
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that subscribing to the same event on the server twice raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ServerDoubleSubscribe()
    {
        // Create a message queue and an instance of the server communicator.
        Queue messages = new();
        ICommunicator server = new Server();

        // Start the server.
        string[] _ = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );

        // Subscribe the server to an event twice and check for the expected exception.
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );

        // Stop the server.
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that attempting to start a client twice raises the correct exception.
    /// </summary>
    [TestMethod]
    public void ClientDoubleStart()
    {
        // Create instances of the server and client communicators.
        ICommunicator server = new Server();
        ICommunicator client = new Client();

        // Start the server.
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );

        // Start the client and check for the expected return value.
        string ret = client.Start( ip , port , "testClient1" , "unitTestClient" );
        Assert.AreNotEqual( ret , "already started" );

        // Attempt to start the client again and check for the expected return value.
        ret = client.Start( ip , port , "testClient1" , "unitTestClient" );
        Assert.AreEqual( ret , "already started" );

        // Stop the client and server.
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

        // Start the server and subscribe it to an event.
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestServer" );

        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );

        // Start the client.
        client.Start( ip , port , "testClient1" , "unitTestClient" );

        // Create and send a message from the client to the server with a specific exception trigger.
        Networking.Models.Message message = new( "Throw" , "unitTestServer" , Id.GetServerId() , "testClient1" );
        client.Send( message.Data , "unitTestServer" , message.DestId );

        // Wait for the message to be received by the server.
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail( "Did not receive message" );
            }
        }

        // Dequeue the received message and verify that it matches the sent message.
        Networking.Models.Message receivedMessage = messages.Dequeue();
        Assert.IsTrue( CompareMessages( receivedMessage , message ) );

        // Stop the client and server.
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

        // Start the server and subscribe it to an event.
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        server.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestServer" );

        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );

        // Start the client.
        client.Start( ip , port , "testClient1" , "unitTestClient" );

        // Create and send a message from the client to the server.
        Networking.Models.Message message = new( "testMessage" , "unitTestServer" , Id.GetServerId() , "testClient1" );
        client.Send( message.Data , "NOTunitTestServer" , message.DestId );
        client.Send( message.Data , "unitTestServer" , message.DestId );

        // Wait for the message to be received by the server.
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail( "Did not receive message" );
            }
        }

        // Dequeue the received message and verify that it matches the sent message.
        Networking.Models.Message receivedMessage = messages.Dequeue();
        Assert.IsTrue( CompareMessages( receivedMessage , message ) );

        // Stop the client and server.
        client.Stop();
        server.Stop();
    }

    /// <summary>
    /// Test method to ensure that a client cannot send a message to an unsubscribed server.
    /// </summary>
    [TestMethod]
    public void OneClientToUnsubscribedServer()
    {
        int cnt = 0;
        Queue messages = new();
        ICommunicator server = new Server();
        ICommunicator client = new Client();

        // Start the server.
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );

        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );

        // Start the client.
        client.Start( ip , port , "testClient1" , "unitTestClient" );

        // Create and send a message from the client to the server.
        Networking.Models.Message message = new( "testMessage" , "unitTestServer" , Id.GetServerId() , "testClient1" );
        client.Send( message.Data , message.ModuleName , message.DestId );

        // Wait for a short time to check if the server receives the message (it shouldn't).
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }

        // If a message is received, fail the test.
        if (messages.canDequeue())
        {
            Assert.Fail( "Message received by unsubscribed server" );
        }

        // Stop the client and server.
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
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );
        Networking.Models.Message message = new( "testMessage" , "unitTestClient" , "testClient1" , Id.GetServerId() );
        server.Send( message.Data , message.ModuleName , message.DestId );
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }

        if (messages.canDequeue())
        {
            Assert.Fail( "Message received by unsubscribed client" );
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
        string[] ipPort = server.Start( null , null , Id.GetServerId() , "unitTestClient" ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );

        client.Start( ip , port , "testClient1" , "unitTestClient" );
        Thread.Sleep( 4000 );
        client.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );
        Networking.Models.Message message = new( "testMessage" , "unitTestClient" , "testClient1" , Id.GetServerId() );
        server.Send( message.Data , message.DestId );
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
        client.Stop();
        server.Stop();
        Assert.IsTrue( CompareMessages( receivedMessage , message ) );
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
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );

        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );
        client.Subscribe( new GenericEventHandler( messageQueue: messages ) , Id.GetNetworkingBroadcastId() );

        Data data = new( EventType.ServerLeft() );
        Networking.Models.Message message = new( Serializer.Serialize<Data>( data ) , Id.GetNetworkingBroadcastId() , Id.GetBroadcastId() , Id.GetServerId() );

        server.Stop();
            Thread.Sleep( 500 );
        while (!messages.canDequeue())
        {
            Thread.Sleep( 1000 );
            cnt++;
            if (cnt == 10)
            {
                Assert.Fail( "Did not receive message" );
            }
        }
        client.Stop();
        Networking.Models.Message receivedMessage = messages.Dequeue();

        Assert.IsTrue( CompareMessages( receivedMessage , message ) );
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
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );
        Data data = new( "testMessage" , EventType.ChatMessage() );
        Networking.Models.Message message = new( Serializer.Serialize( data ) , "unitTestClient" , "testClient1" , "testClient1" );
        client.Subscribe( new GenericEventHandler( messageQueue: messages ) , "unitTestClient" );
        client.Send( message.Data , message.DestId );
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
        client.Stop();
        server.Stop();
        Assert.IsTrue( CompareMessages( receivedMessage , message ) );
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
        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
        string ip = ipPort[0];
        int port = int.Parse( ipPort[1] );
        client.Start( ip , port , "testClient1" , "unitTestClient" );
        Networking.Models.Message message = new( "testMessage" , "unitTestClient" , "testClient1" , "testClient1" );
        //client.Subscribe(new GenericEventHandler(messageQueue: messages), "unitTestClient");
        client.Send( message.Data , message.ModuleName , message.DestId );
        while (!messages.canDequeue())
        {
            Thread.Sleep( 300 );
            cnt++;
            if (cnt == 10)
            {
                break;
            }
        }
        if (messages.canDequeue())
        {
            Assert.Fail( "Message received on unsubscribed client" );
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
        Assert.IsTrue( true );
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

        string[] ipPort = server.Start( null , null , Id.GetServerId() , Id.GetNetworkingId() ).Split( ':' );
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
            clients[i].Send( sentMessages[i].Data , sentMessages[i].DestId );
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
    private bool CompareMessages( Networking.Models.Message message1 , Networking.Models.Message message2 )
    {
        return !((message1.Data != message2.Data) ||
            (message1.DestId != message2.DestId) ||
            (message1.SenderId != message2.SenderId) ||
            (message1.ModuleName != message2.ModuleName));
    }

    /// <summary>
    /// Creates and starts multiple clients and returns them as an array.
    /// </summary>
    /// <param name="n">The number of clients to create.</param>
    /// <param name="destIP">The destination IP for the clients.</param>
    /// <param name="destPort">The destination port for the clients.</param>
    /// <param name="moduleName">The module name for the clients.</param>
    /// <returns>An array of started clients.</returns>
    private Client[] getClientsAndStart( int n , string destIP , int destPort , string moduleName )
    {
        List<Client> clientsList = new();

        for (int i = 0; i < n; i++)
        {
            Client client = new();
            client.Start( destIP , destPort , "client_" + i.ToString() , moduleName );
            clientsList.Add( client );
        }

        return clientsList.ToArray();
    }

}
