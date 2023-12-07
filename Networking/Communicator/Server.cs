/******************************************************************************
 * Filename    = Communicator/Server.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Implementation of the server-side module for the Analyzer.
 *****************************************************************************/

using System.Net.Sockets;
using System.Net;
using Networking.Utils;
using Networking.Models;
using Networking.Events;
using Networking.Serialization;
using Logging;

namespace Networking.Communicator
{
    /// <summary>
    /// Represents the server-side module for the Analyzer.
    /// </summary>
    public class Server : ICommunicator
    {
        private bool _stopThread = false;
        private string _moduleName;
        private Sender _sender;
        private Thread _listenThread;
        private Receiver _receiver;
        private TcpListener _serverListener;
        public Dictionary<string, NetworkStream> _clientIdToStream { get; set; } = new();
        public Dictionary<string, string> _senderIdToClientId { get; set; } = new();
        private readonly Dictionary<string, IEventHandler> _eventHandlersMap = new();
        private string _senderId;
        private bool _isStarted = false;
        private string _ipPort = "";

        /// <summary>
        /// Gets the local IPv4 address of the server.
        /// </summary>

        private string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            // prioritizing the returning of private IPv4 in the subnet 10.*.*.*
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork &&
                    ip.ToString().Length>3 && ip.ToString()[..3] == "10.")
                {
                    return ip.ToString();
                }
            }

            // otherwise return any valid IPv4
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// Sends serialized data to a destination with specified module name and destination ID.
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent.</param>
        /// <param name="moduleName">The module where the data is to be delivered.</param>
        /// <param name="destId">The Id of the destination communicator where the data is to be delivered.</param>
        public void Send(string serializedData, string moduleName, string destId)
        {
            if (!_isStarted)
            {
                throw new Exception("Start server first");
            }

            Logger.Log("[Server] Send" + serializedData + " " + moduleName + " " + destId,LogLevel.INFO);
            Message message = new(
                serializedData, moduleName, destId, _senderId
            );
            _sender.Send(message);
        }

        /// <summary>
        /// Sends serialized data to a destination with the default module name.
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent.</param>
        /// <param name="destId">The Id of the destination communicator where the data is to be delivered.</param>
        public void Send(string serializedData, string destId)
        {
            if (!_isStarted)
            {
                throw new Exception("Start server first");
            }
            Logger.Log("[Server] Send" + serializedData + " " + _moduleName + " " + destId , LogLevel.INFO );
            Message message = new(
                serializedData, _moduleName, destId, _senderId
            );
            _sender.Send(message);
        }

        /// <summary>
        /// Sends serialized data to a destination with specified module name, destination ID, and sender ID.
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent.</param>
        /// <param name="moduleName">The module where the data is to be delivered.</param>
        /// <param name="destId">The Id of the destination communicator where the data is to be delivered.</param>
        /// <param name="senderId">The Id of the sender communicator.</param>
        public void Send(string serializedData, string moduleName, string destId, string senderId)
        {
            if (!_isStarted)
            {
                throw new Exception("Start server first");
            }

            Logger.Log("[Server] Send" + serializedData + " " + moduleName + " " + destId , LogLevel.INFO );
            Message message = new(
                serializedData, moduleName, destId, senderId
            );
            _sender.Send(message);
        }

        /// <summary>
        /// Starts the server and returns it's IP and port number. 
        /// </summary>
        /// <param name="destIP">ignored and not used</param>
        /// <param name="destPort">ignored and not used</param>
        /// <param name="senderId">The unique Id of the server. This is the Id referred to in Send functions.</param>
        /// <param name="moduleName">The module where data is to be delivered by default</param>
        public string Start(string? destIP, int? destPort, string senderId, string moduleName)
        {
            if (_isStarted)
            {
                Logger.Log("[Server] Already started, returning same IP:Port" , LogLevel.ERROR );
                return _ipPort;
            }
            Logger.Log("[Server] Start" + destIP + " " + destPort , LogLevel.INFO );
            _moduleName = moduleName;
            _senderId = senderId;
            _sender = new(_clientIdToStream, _senderIdToClientId, false);
            _receiver = new(_clientIdToStream, this);

            int port = 12399;
            Random random = new();
            while (true)
            {
                try
                {
                    _serverListener = new TcpListener(IPAddress.Any, port);
                    _serverListener.Start();
                    break;
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        port = random.Next(1, 65534);
                    }
                    else
                    {
                        Logger.Log("Socket error: " + ex.SocketErrorCode , LogLevel.ERROR );
                    }
                }
            }
            IPEndPoint localEndPoint = (IPEndPoint)_serverListener.LocalEndpoint;
            Logger.Log("[Server] Server is listening on:" , LogLevel.INFO );
            Logger.Log("[Server] IP Address: " + GetLocalIPAddress() , LogLevel.INFO );
            Logger.Log("[Server] Port: " + localEndPoint.Port , LogLevel.INFO );
            _listenThread = new Thread(AcceptConnection)
            {
                IsBackground = true
            };
            _listenThread.Start();
            _isStarted = true;
            Subscribe(new NetworkingEventHandler(this), Id.GetNetworkingId());
            _ipPort = GetLocalIPAddress() + ":" + localEndPoint.Port;
            return _ipPort;
        }

        /// <summary>
        /// boradcasts message to clients that the server is stopping,
        /// stops listening to new connections,
        /// closes all streams
        /// and stops all threads
        /// </summary>
        public void Stop()
        {
            if (!_isStarted)
            {
                throw new Exception("Start server first");
            }

            Logger.Log("[Server] Stop" , LogLevel.INFO );
            _stopThread = true;
            Data data = new (EventType.ServerLeft());
            Send(Serializer.Serialize<Data>(data), Id.GetNetworkingBroadcastId(), Id.GetBroadcastId());
            Send(Serializer.Serialize<Data>(data),Id.GetNetworkingId(),Id.GetBroadcastId());
            _sender.Stop();
            _receiver.Stop();
            foreach (NetworkStream stream in _clientIdToStream.Values)
            {
                stream.Close(); // Close the network stream
            }

            Logger.Log("[Server] Stopped _sender and _receiver" , LogLevel.INFO );
            _serverListener.Stop();
            //_listenThread.Interrupt();
            _listenThread.Join();
            _isStarted = false;
            Logger.Log("[Server] Stopped" , LogLevel.INFO );
        }

        /// <summary>
        /// Subscribe a handler <paramref name="eventHandler"/> to a module <paramref name="moduleName"/>
        /// </summary>
        /// <param name="eventHandler">The implemented class of the event handler </param>
        /// <param name="moduleName">The name of module to subscribe</param>
        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            if (!_isStarted)
            {
                throw new Exception("Start server first");
            }

            Logger.Log("[Server] Subscribe " + moduleName , LogLevel.INFO );

            if (_eventHandlersMap.ContainsKey(moduleName))
            {
                Logger.Log("[Server] "+moduleName+" already subscribed!" , LogLevel.WARNING );// already subs
            }
            else
            {
                _eventHandlersMap[moduleName] = eventHandler;
            }
        }

        /// <summary>
        /// Accepts incoming client connections and adds them to the dictionary.
        /// </summary>
        void AcceptConnection()
        {
            string clientId = "A";

            while (!_stopThread)
            {
                Logger.Log( "[Server] waiting for connection" , LogLevel.INFO );
                TcpClient client = new();
                try
                {
                    client = _serverListener.AcceptTcpClient();
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        Logger.Log("[Server] Listener stopped" , LogLevel.INFO );
                        break;
                    }
                    //handle other exceptions
                        
                }
                try
                {
                    NetworkStream stream = client.GetStream();
                    lock (_clientIdToStream) { _clientIdToStream.Add(clientId, stream); }
                }
                catch (Exception)
                {
                    Logger.Log("[Server] Failed to get stream!" , LogLevel.ERROR );
                    continue;
                }
                clientId += 'A';
                Logger.Log("New client connected" , LogLevel.INFO );
            }
        }

        /// <summary>
        /// Handles incoming messages by demultiplexing to the required module.
        /// </summary>
        /// <param name="message">The received message.</param>
        public void HandleMessage(Message message)
        {
            if (message.DestId == Id.GetServerId())
            {
                try
                {
                    _eventHandlersMap[message.ModuleName].HandleMessageRecv(message);
                }
                catch (Exception e)
                {
                    if (_eventHandlersMap.ContainsKey( message.ModuleName ))
                    {
                        Logger.Log( "[Server] " + message.ModuleName + " not subscribed" , LogLevel.WARNING );
                    }
                    else
                    {
                        Logger.Log( "[Server] Error in handling message: " + e.Message , LogLevel.ERROR );
                    }
                }
            }
            else
            {
                Send(message.Data, message.ModuleName, message.DestId, message.SenderId);
            }
        }
    }
}
