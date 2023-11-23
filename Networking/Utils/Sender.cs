/******************************************************************************
 * Filename    = Utils/Sender.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = The sending functionality of the communicators is implemented
 *               here.
 *****************************************************************************/

using System.Net.Sockets;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;

namespace Networking.Utils
{
    /// <summary>
    /// The class responsible for the sending functionality of the communicators
    /// </summary>
    public class Sender
    {
        /// <summary>
        /// The priority queue for the messages to be sent.
        /// </summary>
        private readonly Queue _sendQueue = new();

        /// <summary>
        /// Thread that sends messages from <see cref="_sendQueue"/>.
        /// </summary>
        private readonly Thread _sendThread;

        /// <summary>
        /// Denotes whether the communicator using this <see cref="Sender"/> is a client or not.
        /// </summary>
        private readonly bool _isClient;

        /// <summary>
        /// Dictionary mapping client Ids to network streams.
        /// </summary>
        private readonly Dictionary<string, NetworkStream> _clientIDToStream;

        /// <summary>
        /// Dictionary mapping Id of the communicator to client Id.
        /// </summary>
        readonly Dictionary<string, string> _senderIDToClientID;

        /// <summary>
        /// Flag to signal the <see cref="_sendThread"/> to stop.
        /// </summary>
        private bool _stopThread;

        /// <summary>
        /// Constructor for the sender. Spawns thread that polls from <see cref="_sendQueue"/> and sends it
        /// </summary>
        /// <param name="clientIDToStream">The mapping of Client Id (internal) to Networksteam of the client</param>
        /// <param name="senderIDToClientID">The mapping of the Id of the communicator to the Client Id (internal)</param>
        /// <param name="isClient">Whether the communicator is Client</param>
        public Sender(Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, string> senderIDToClientID, bool isClient)
        {
            _stopThread = false;
            _senderIDToClientID=senderIDToClientID;
            _isClient = isClient;
            Console.WriteLine("[Sender] Init");
            _clientIDToStream = clientIDToStream;
            _sendThread = new Thread(SendLoop)
            {
                IsBackground = true                 // if all foreground threads have terminated, then all the background threads are automatically stopped when the application quits
            };
            _sendThread.Start();
        }

        /// <summary>
        /// Stops the sender thread
        /// </summary>
        public void Stop()
        {

            Console.WriteLine("[Sender] Stop");
            _stopThread = true;
            _sendThread.Join();
        }

        /// <summary>
        /// Enqueues the <paramref name="message"/> to <see cref="_sendQueue"/> for it to be sent
        /// </summary>
        /// <param name="message">The message to be sent</param>
        public void Send(Message message)
        {
            _sendQueue.Enqueue(message, Priority.GetPriority(message.ModuleName));
        }

        /// <summary>
        /// Whether the next-to-be-dequeued message is directed to `networking` or `networkingBroadcast`.
        /// Used to temporatily delay Stoping the <see cref="Sender"/>.
        /// </summary>
        /// <returns>true if and only if <see cref="_sendQueue"/> can be dequeued and the next-to-be-dequeued message is directed to `networking` or `networkingBroadcast`.</returns>
        private bool IfNetworkingMessage()
        {
            Message? message = _sendQueue.Peek();
            if (message == null)
            {
                return false;
            }
            else
            {
                return message.ModuleName == ID.GetNetworkingID() || message.ModuleName == ID.GetNetworkingBroadcastID();
            }
        }

        /// <summary>
        /// Writes the <paramref name="message"/> to the <paramref name="stream"/> following the sending protocol.
        /// </summary>
        /// <param name="stream">The network stream to write to</param>
        /// <param name="message">The bytes of the message to send</param>
        /// <param name="messageSize">The length of the bytes of the message</param>
        private static void SendToDest(NetworkStream stream, byte[] message, int messageSize)
        {
            stream.Write( BitConverter.GetBytes( messageSize ) , 0 , sizeof( int ) );   // first sizeof( int ) bytes of a message sent will be the size of message payload
            stream.Write( message );                                                    // sending the message itself
        }

        /// <summary>
        /// The function that polls from the <see cref="_sendQueue"/> and sends the message to appropriate destination
        /// </summary>
        public void SendLoop()
        {
            // Continue sending messages, if to be sent, until the thread is signaled to stop
            while (IfNetworkingMessage() || (!_stopThread))             // Temporaily halt stopping when the next-to-be-dequeued message is directed to `networking` or `networkingBroadcast`.
            {
                if (!_sendQueue.canDequeue())
                {
                    // wait for some time to avoid busy-waiting
                    Thread.Sleep(500);
                    continue;
                }

                // Get the next message to send
                Message message = _sendQueue.Dequeue();

                // Serialize the message; convert to Bytes; get its length
                string serStr = Serializer.Serialize(message);
                byte[] messagebytes = System.Text.Encoding.ASCII.GetBytes(serStr);
                int messageSize = messagebytes.Length;

                // Try to send the message
                try
                {
                    if (_isClient == true)                              // All messages from the client is sent to the Server. If the destination is not Server, the message will be sent to the right recipient from the Server
                    {
                        SendToDest( _clientIDToStream[ID.GetServerID()] , messagebytes , messageSize );
                    }
                    else
                    {
                        if (message.DestID == ID.GetBroadcastID())      // Broadcast the message to all clients
                        {
                            foreach (KeyValuePair<string, NetworkStream> pair in _clientIDToStream)
                            {
                                SendToDest( pair.Value , messagebytes , messageSize );
                            }
                        }
                        else                                            // Send the message to the appropriate recipient
                        {
                            SendToDest( _clientIDToStream[_senderIDToClientID[message.DestID]] , messagebytes , messageSize );
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot send message to "+ message.DestID);
                    Console.WriteLine(e.Message);    
                }
            }
        }
    }
}

