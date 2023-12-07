/******************************************************************************
 * Filename    = Utils/Receiver.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = The receive functionality of the communicators is implemented
 *               here.
 *****************************************************************************/

using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using Networking.Communicator;
using Networking.Models;
using Networking.Queues;
using Networking.Serialization;
using Logging;

namespace Networking.Utils
{
    /// <summary>
    /// Handles receiving messages from network streams and enqueues them for further processing.
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Priority queue for received messages
        /// </summary>
        private readonly Queue _recvQueue = new();

        /// <summary>
        /// Thread for receiving messages
        /// </summary>
        private readonly Thread _recvThread;

        /// <summary>
        /// Thread for processing received messages
        /// </summary>
        private readonly Thread _recvQueueThread;

        /// <summary>
        /// Dictionary mapping client IDs to network streams
        /// </summary>
        private readonly Dictionary<string , NetworkStream> _clientIdToStream;

        /// <summary>
        /// Flag to signal the threads to stop
        /// </summary>
        private bool _stopThread = false;

        /// <summary>
        /// Reference to the <see cref="ICommunicator"/> interface
        /// </summary>
        private readonly ICommunicator _comm;

        /// <summary>
        /// Initializes a new instance of the Receiver class.
        /// </summary>
        /// <param name="clientIdToStream">Dictionary mapping client IDs to network streams.</param>
        /// <param name="comm">Communicator interface for handling received messages.</param>
        public Receiver( Dictionary<string , NetworkStream> clientIdToStream , ICommunicator comm )
        {
            Logger.Log( "[Receiver] Init" , LogLevel.INFO );
            _clientIdToStream = clientIdToStream;
            _recvThread = new Thread( Receive )
            {
                IsBackground = true
            };
            _recvQueueThread = new Thread( RecvLoop )
            {
                IsBackground = true
            };
            _recvThread.Start();
            _recvQueueThread.Start();
            _comm = comm;
        }

        /// <summary>
        /// Stops the receiver by signaling threads to stop and waiting for them to join.
        /// </summary>
        public void Stop()
        {
            Logger.Log( "[Receiver] Stop" , LogLevel.INFO );

            _stopThread = true;

            // Wait for the threads to terminate
            _recvThread.Join();
            _recvQueueThread.Join();
        }

        /// <summary>
        /// Thread function for receiving messages from network streams following the protocol.
        /// </summary>
        private void Receive()
        {
            Logger.Log( "[Receiver] Receive starts" , LogLevel.INFO );


            // Continue receiving messages until the thread is signaled to stop
            while (!_stopThread)
            {
                bool ifAval = false;

                // Iterate over each client's network stream
                foreach (KeyValuePair<string , NetworkStream> item in _clientIdToStream)
                {
                    try
                    {
                        if (item.Value.DataAvailable == true)
                        {
                            ifAval = true;

                            // Read the size of the incoming message as per the protocol
                            byte[] sizeBytes = new byte[sizeof( int )];
                            int sizeBytesRead = item.Value.Read( sizeBytes , 0 , sizeof( int ) );
                            System.Diagnostics.Trace.Assert( sizeBytesRead == sizeof( int ) );
                            int messageSize = BitConverter.ToInt32( sizeBytes , 0 );

                            // Now read the actual message
                            byte[] receiveData = new byte[messageSize];
                            int totalBytesRead = 0;

                            // Continue reading until the entire message is received
                            while (totalBytesRead < messageSize)
                            {
                                sizeBytesRead = item.Value.Read( receiveData , totalBytesRead , messageSize - totalBytesRead );
                                totalBytesRead += sizeBytesRead;
                            }

                            System.Diagnostics.Trace.Assert( totalBytesRead == messageSize );

                            // Convert received byte array to a string; deserialize the string into a Message object
                            string receivedMessage = Encoding.ASCII.GetString( receiveData );
                            Message message = Serializer.Deserialize<Message>( receivedMessage );

                            // If the message belongs to the Networking module, attach the client ID to the payload and update the message data
                            if (message.ModuleName == Id.GetNetworkingId())
                            {
                                Data data = Serializer.Deserialize<Data>( message.Data );

                                if (data.EventType == EventType.ClientRegister())
                                {
                                    data.Payload = item.Key;
                                    message.Data = Serializer.Serialize( data );
                                }
                            }

                            // Enqueue the received message with its priority
                            _recvQueue.Enqueue( message , Priority.GetPriority( message.ModuleName ) );
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log( "Exception in receiver: " + ex.Message , LogLevel.ERROR );

                    }
                }

                // If no data is available, sleep for a short duration to avoid busy-waiting
                if (ifAval == false)
                {
                    Thread.Sleep( 200 );
                }
            }

            Logger.Log( "[Receiver] Receive stops" , LogLevel.INFO );

        }


        /// <summary>
        /// Thread function for processing received messages from the queue.
        /// </summary>
        private void RecvLoop()
        {
            while (!_stopThread)
            {
                if (!_recvQueue.canDequeue())
                {
                    // Wait for some time if the queue is empty to avoid busy-waiting
                    Thread.Sleep( 500 );
                    continue;
                }

                // Get the next message to process and process it based on the type of communicator
                Message message = _recvQueue.Dequeue();
                if (_comm is Client client)
                {
                    client.HandleMessage( message );
                }
                else if (_comm is Server server)
                {
                    server.HandleMessage( message );
                }
            }
        }
    }
}
