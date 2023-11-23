/******************************************************************************
 * Filename    = Communicator/Client.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = The functionality of the client (student) side of the Analyzer
 *               is implemented here.
 *****************************************************************************/

using System.Net;
using System.Net.Sockets;
using Networking.Events;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace Networking.Communicator
{
    /// <summary>
    /// The class that implements the functionality of the client (student) side
    /// of the Analyzer
    /// </summary>
    public class Client : ICommunicator
    {
        private string _moduleName;
        private Sender _sender;
        private Receiver _receiver;
        public Dictionary<string, NetworkStream> _IdToStream = new();
        readonly Dictionary<string, string> _senderIdToClientId = new();

        private string _senderId;
        private NetworkStream _networkStream;
        private readonly Dictionary<string, IEventHandler> _eventHandlersMap = new();

        private bool _isStarted = false;
        

        /// <summary>
        /// Sends serialized data to destination. 
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent</param>
        /// <param name="moduleName">The module where the data is to be delivered</param>
        /// <param name="destId">The Id of destination communicator where the data is to be delivered</param>
        public void Send(string serializedData, string moduleName, string destId) 
        {
            if (!_isStarted)
            {
                throw new Exception("Start client first");
            }

            // NOTE: destID SHOULD be ID.GetServerID() to send to the server.
            Console.WriteLine("[Client] Send" + serializedData + " " + moduleName + " " + destId);
            Message message = new(
                serializedData, moduleName, destId, _senderId
            );
            _sender.Send(message);
        }

        /// <summary>
        /// Sends serialized data to the module (passed in the Start function) in the destination
        /// </summary>
        /// <param name="serializedData">Serialized data to be sent</param>
        /// <param name="destId">The Id of destination communicator where the data is to be delivered</param>
        public void Send(string serializedData, string destId)
        {
            if (!_isStarted)
            {
                throw new Exception("Start client first");
            }

            Console.WriteLine("[Client] Send" + serializedData + " " + _moduleName + " " + destId);
            Message message = new(
                serializedData, _moduleName, destId, _senderId
            );
            _sender.Send(message);
        }

        /// <summary>
        /// Starts the client. 
        /// </summary>
        /// <param name="destIP">IP address of the server</param>
        /// <param name="destPort">Port number of the server</param>
        /// <param name="destIP">Serialized data to be sent</param>
        /// <param name="senderId">The unique Id of the client. This is the Id referred to in Send functions.</param>
        /// <param name="moduleName">The module where data is to be delivered by default</param>
        public string Start(string? destIP, int? destPort, string senderId,string moduleName)
        {
            if (destIP == null || destPort == null)
            {
                throw new Exception("Illegal arguments");
            }
            if (_isStarted)
            {
                return "already started";
            }
            
            _moduleName = moduleName;
            _senderId = senderId;

            Console.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();


            try
            {
                tcpClient.Connect(destIP, destPort.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Client] Cannot connect to server at "+ destIP + ":"+ destPort.ToString());
                Console.WriteLine(e.Message);
                return "failed";
            }
            _isStarted = true;              // mark as started only when the connection is successful


            IPEndPoint localEndPoint = (IPEndPoint)tcpClient.Client.LocalEndPoint;
            Console.WriteLine("[Client] IP Address: " + localEndPoint.Address.MapToIPv4());
            Console.WriteLine("[Client] Port: " + localEndPoint.Port);

            // send message to Networking module of the server to register this client with this Id
            Data data = new(_senderId,EventType.ClientRegister());
            Message message = new(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetServerID(), _senderId);

            _networkStream = tcpClient.GetStream();
            lock (_IdToStream) { _IdToStream[ID.GetServerID()] = _networkStream; }

            // starting the sender and receiver threads
            Console.WriteLine("[Client] Starting sender");
            _sender = new(_IdToStream,_senderIdToClientId, true);
            Console.WriteLine("[Client] Starting receiver");
            _receiver = new(_IdToStream, this);
            _sender.Send(message);

            // subscribing to the Networking module's event handler.
            Subscribe(new NetworkingEventHandler(this), ID.GetNetworkingID());
            Console.WriteLine("[Client] Started");
            return localEndPoint.Address.MapToIPv4()+":"+localEndPoint.Port;
        }

        /// <summary>
        /// Sends message to server that the client is stopping, stops listening to the server, and stops all threads
        /// </summary>
        public void Stop()
        {
            if (!_isStarted)
            {
                throw new Exception("Start client first");
            }

            Console.WriteLine("[Client] Stop");
            Data data = new(EventType.ClientDeregister());
            _sender.Send(new Message(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetServerID(), _senderId));
            _sender.Stop();
            _receiver.Stop();

            _networkStream.Close();
            _isStarted = false;
            Console.WriteLine("[Client] Stopped");
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
                throw new Exception("Start client first");
            }

            Console.WriteLine("[Client] Subscribe "+ moduleName);

            if (_eventHandlersMap.ContainsKey(moduleName))
            {
                Console.WriteLine("[Client] "+moduleName+" already subscribed");// already subs
            }
            else
            {
                _eventHandlersMap[moduleName] = eventHandler;
            }
        }

        /// <summary>
        /// Handles incoming message to this destination client. This function is responsible for demultiplexing to the required module.
        /// </summary>
        /// <param name="message">The received message</param>
        public void HandleMessage(Message message)
        {
            if (_eventHandlersMap.ContainsKey( message.ModuleName ))
            {
                _eventHandlersMap[message.ModuleName].HandleMessageRecv(message);
            }
            else
            {
                 Console.WriteLine("[Client] " + message.ModuleName + " not subscribed");
            }
        }

    }
}




