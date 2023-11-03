/******************************************************************************
 * Filename    = UdpCommunicator.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = GuiAndDistributedDemo
 * 
 * Project     = Networking
 *
 * Description = Defines a UDP communicator.
 *****************************************************************************/

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    /// <summary>
    /// Communicator that can send and listen for messages over the network using UDP.
    /// </summary>
    internal class UdpCommunicator : ICommunicator
    {
        private IPEndPoint? _endPoint;
        private readonly UdpClient _listener;
        private readonly Thread _listenThread;      // Thread that listens for messages on the UDP port.
        private readonly Dictionary<string, IMessageListener> _subscribers; // List of subscribers.

        /// <summary>
        /// Creates an instance of the UDP Communicator.
        /// </summary>
        /// <param name="listenPort">UDP port to listen on.</param>
        public UdpCommunicator(int listenPort)
        {
            _subscribers = new Dictionary<string, IMessageListener>();

            // Create and start the thread that listens for messages.
            ListenPort = listenPort;
            _listener = new(ListenPort);
            _listenThread = new(new ThreadStart(ListenerThreadProc))
            {
                IsBackground = true // Stop the thread when the main thread stops.
            };
            _listenThread.Start();
        }

        /// <inheritdoc />
        public int ListenPort { get; private set; }

        /// <inheritdoc />
        public void AddSubscriber(string id, IMessageListener subscriber)
        {
            Debug.Assert(!string.IsNullOrEmpty(id));
            Debug.Assert(subscriber != null);

            lock (this)
            {
                if (_subscribers.ContainsKey(id))
                {
                    _subscribers[id] = subscriber;
                }
                else
                {
                    _subscribers.Add(id, subscriber);
                }
            }
        }

        /// <inheritdoc />
        public void RemoveSubscriber(string id)
        {
            Debug.Assert(!string.IsNullOrEmpty(id));

            lock (this)
            {
                if (_subscribers.ContainsKey(id))
                {
                    _subscribers.Remove(id);
                }
            }
        }

        /// <inheritdoc/>
        public void SendMessage(string ipAddress, int port, string senderId, string message)
        {
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress broadcastAddress = IPAddress.Parse(ipAddress);
            byte[] sendBuffer = Encoding.ASCII.GetBytes($"{senderId}:{message}");
            IPEndPoint endPoint = new(broadcastAddress, port);
            int bytesSent = socket.SendTo(sendBuffer, endPoint);
            Debug.Assert(bytesSent == sendBuffer.Length);
        }

        /// <summary>
        /// Listens for messages on the listening port.
        /// </summary>
        private void ListenerThreadProc()
        {
            Debug.WriteLine($"Listener Thread Id = {Environment.CurrentManagedThreadId}.");

            while (true)
            {
                try
                {
                    // Listen for message on the listening port, and receive it when it comes along.
                    byte[] bytes = _listener.Receive(ref _endPoint);
                    string payload = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    Debug.WriteLine($"Received payload: {payload}");

                    // The received payload is expected to be in the format <Identity>:<Message>
                    string[] tokens = payload.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 2)
                    {
                        string id = tokens[0];
                        string message = tokens[1];
                        lock (this)
                        {
                            if (_subscribers.ContainsKey(id))
                            {
                                _subscribers[id].OnMessageReceived(message);
                            }
                            else
                            {
                                Debug.WriteLine($"Received message for unknown subscriber: {id}");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
