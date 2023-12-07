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
using System.Diagnostics;
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
        private readonly Dictionary<string, NetworkStream> _clientIdToStream;

        /// <summary>
        /// Dictionary mapping Id of the communicator to client Id.
        /// </summary>
        readonly Dictionary<string, string> _senderIdToClientId;

        /// <summary>
        /// Flag to signal the <see cref="_sendThread"/> to stop.
        /// </summary>
        private bool _stopThread;
        private readonly ManualResetEvent _queueEvent = new( false );
        private readonly object _lock = new();

        /// <summary>
        /// Constructor for the sender. Spawns thread that polls from <see cref="_sendQueue"/> and sends it
        /// </summary>
        /// <param name="clientIdToStream">The mapping of Client Id (internal) to Networksteam of the client</param>
        /// <param name="senderIdToClientId">The mapping of the Id of the communicator to the Client Id (internal)</param>
        /// <param name="isClient">Whether the communicator is Client</param>
        public Sender(Dictionary<string, NetworkStream> clientIdToStream, Dictionary<string, string> senderIdToClientId, bool isClient)
        {
            _stopThread = false;
            _senderIdToClientId=senderIdToClientId;
            _isClient = isClient;
            Trace.WriteLine("[Sender] Init");
            _clientIdToStream = clientIdToStream;
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

            Trace.WriteLine("[Sender] Stop");
            _stopThread = true;
            _queueEvent.Set();
            _sendThread.Join();
        }

        /// <summary>
        /// Enqueues the <paramref name="message"/> to <see cref="_sendQueue"/> for it to be sent
        /// </summary>
        /// <param name="message">The message to be sent</param>
        public void Send(Message message)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue( message , Priority.GetPriority( message.ModuleName ) );
                _queueEvent.Set();
            }
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
                return message.ModuleName == Id.GetNetworkingId() || message.ModuleName == Id.GetNetworkingBroadcastId();
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
            try
            {
                stream.Write( BitConverter.GetBytes( messageSize ) , 0 , sizeof( int ) );   // first sizeof( int ) bytes of a message sent will be the size of message payload
                                                                                            //stream.Write( message );                                                    // sending the message itself
                int chunkSize = 1024 * 1024; // 1MB chunk size
                int totalBytesSent = 0;

                while (totalBytesSent < message.Length)
                {
                    int bytesToSend = Math.Min( chunkSize , message.Length - totalBytesSent );
                    stream.Write( message , totalBytesSent , bytesToSend );
                    totalBytesSent += bytesToSend;
                }
            }
            catch(Exception e) {
                Trace.WriteLine( "Exception in Sender:SendToDest " +e.Message);
            }

        }

        /// <summary>
        /// The function that polls from the <see cref="_sendQueue"/> and sends the message to appropriate destination
        /// </summary>
        public void SendLoop()
        {
            // Continue sending messages, if to be sent, until the thread is signaled to stop
            while (IfNetworkingMessage() || (!_stopThread))             // Temporaily halt stopping when the next-to-be-dequeued message is directed to `networking` or `networkingBroadcast`.
            {
                //if (!_sendQueue.canDequeue())
                //{
                //    // wait for some time to avoid busy-waiting
                //    Thread.Sleep(500);
                //    continue;
                //}
                _queueEvent.WaitOne();
                lock (_lock)
                {
                    if (!_sendQueue.canDequeue())
                    {
                        continue;
                        throw new Exception( "Exception in sender, queue is empty" );
                    }

                    // Get the next message to send
                    Message message = _sendQueue.Dequeue();

                    // Serialize the message; convert to Bytes; get its length
                    //string serStr = Serializer.Serialize( message );
                    byte[] messagebytes = System.Text.Encoding.ASCII.GetBytes( Serializer.Serialize( message ) );

                    int messageSize = messagebytes.Length;

                    // Try to send the message
                    try
                    {
                        if (_isClient == true)                              // All messages from the client is sent to the Server. If the destination is not Server, the message will be sent to the right recipient from the Server
                        {
                            SendToDest( _clientIdToStream[Id.GetServerId()] , messagebytes , messageSize );
                        }
                        else
                        {
                            if (message.DestId == Id.GetBroadcastId())      // Broadcast the message to all clients
                            {
                                foreach (KeyValuePair<string , NetworkStream> pair in _clientIdToStream)
                                {
                                    SendToDest( pair.Value , messagebytes , messageSize );
                                }
                            }
                            else                                            // Send the message to the appropriate recipient
                            {
                                SendToDest( _clientIdToStream[_senderIdToClientId[message.DestId]] , messagebytes , messageSize );
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine( "Cannot send message to " + message.DestId );
                        Trace.WriteLine( e.Message );
                    }
                    if (!_sendQueue.canDequeue())
                    {
                        _queueEvent.Reset();
                    }
                }
            }
        }
    }
}

