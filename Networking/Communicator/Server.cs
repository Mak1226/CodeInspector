/******************************************************************************
 * Filename    = Communicator/Server.cs
 *
 * Author      = 
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = 
 *****************************************************************************/


using System.Net.Sockets;
using System.Net;
using Networking.Utils;
using Networking.Models;
using Networking.Events;
using System.Diagnostics;
using System;
using Networking.Serialization;

namespace Networking.Communicator
{
    public class Server : ICommunicator
    {
        private bool _stopThread = false;
        private string _moduleName;
        private Sender _sender;
        private Thread _listenThread;
        private Receiver _receiver;
        private TcpListener _serverListener;
        public Dictionary<string, NetworkStream> _clientIDToStream { get; set; } = new();
        public Dictionary<string, string> _senderIDToClientID { get; set; } = new();
        private Dictionary<string, IEventHandler> _eventHandlersMap = new();
        private string _senderId;
        private bool _isStarted = false;
        private string _ipPort = "";


        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public void Send(string serializedData, string moduleName, string destId)
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Send" + serializedData + " " + moduleName + " " + destId);
            Message message = new Message(
                serializedData, moduleName, destId, _senderId
            );
            _sender.Send(message);
        }
        public void Send(string serializedData, string destId)
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Send" + serializedData + " " + _moduleName + " " + destId);
            Message message = new Message(
                serializedData, _moduleName, destId, _senderId
            );
            _sender.Send(message);
        }
        public void Send(string serializedData, string moduleName, string destId, string senderId)
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Send" + serializedData + " " + moduleName + " " + destId);
            Message message = new Message(
                serializedData, moduleName, destId, senderId
            );
            _sender.Send(message);
        }

        public string Start(string? destIP, int? destPort, string senderId, string moduleName)
        {
            if (_isStarted)
            {
                Console.WriteLine("[Server] Already started, returning same IP:Port");
                return _ipPort;
            }

            Console.WriteLine("[Server] Start" + destIP + " " + destPort);
            _moduleName = moduleName;
            _senderId = senderId;
            _sender = new(_clientIDToStream, _senderIDToClientID, false);
            _receiver = new(_clientIDToStream, this);

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
                        Console.WriteLine("Socket error: " + ex.SocketErrorCode);
                    }
                }
            }
            IPEndPoint localEndPoint = (IPEndPoint)_serverListener.LocalEndpoint;
            Console.WriteLine("[Server] Server is listening on:");
            Console.WriteLine("[Server] IP Address: " + GetLocalIPAddress());
            Console.WriteLine("[Server] Port: " + localEndPoint.Port);
            _listenThread = new Thread(AcceptConnection)
            {
                IsBackground = true
            };
            _listenThread.Start();
            _isStarted = true;
            Subscribe(new NetworkingEventHandler(this), ID.GetNetworkingID());
            _ipPort = GetLocalIPAddress() + ":" + localEndPoint.Port;
            return _ipPort;
        }

        public void Stop()
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Stop");
            _stopThread = true;
            Data data = new Data(EventType.ServerLeft());
            this.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID());
            this.Send(Serializer.Serialize<Data>(data),ID.GetNetworkingID(),ID.GetBroadcastID());
            _sender.Stop();
            _receiver.Stop();
            foreach (var stream in _clientIDToStream.Values)
            {
                stream.Close(); // Close the network stream
            }

            Console.WriteLine("[Server] Stopped _sender and _receiver");
            _listenThread.Interrupt();
            _serverListener.Stop();
            //_listenThread.Join();
            _isStarted = false;
            Console.WriteLine("[Server] Stopped");
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Subscribe " + moduleName);

            if (_eventHandlersMap.ContainsKey(moduleName))
                Console.WriteLine("[Server] "+moduleName+" already subscribed!");// already subs
            else
                _eventHandlersMap[moduleName] = eventHandler;

        }

        void AcceptConnection()
        {
            string clientID = "A";

            while (!_stopThread)
            {
                Console.WriteLine("waiting for connection");
                TcpClient client = new();
                try
                {
                    client = _serverListener.AcceptTcpClient();
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        Console.WriteLine("[Server] Listener stopped");
                        break;
                    }
                    //handle other exceptions
                        
                }
                try
                {
                    NetworkStream stream = client.GetStream();
                    lock (_clientIDToStream) { _clientIDToStream.Add(clientID, stream); }
                }
                catch (Exception e) {
                    Console.WriteLine("[Server] Failed to get stream!");
                    continue;
                }
                clientID += 'A';
                Console.WriteLine("New client connected");
            }
        }

        public void HandleMessage(Message message)
        {
            if (message.DestID == ID.GetServerID())
            {
                try
                {
                    _eventHandlersMap[message.ModuleName].HandleMessageRecv(message);
                }
                catch
                {
                    Console.WriteLine("[Server] " + message.ModuleName + " not subscribed");
                }
            }
            else
                Send(message.Data, message.ModuleName, message.DestID, message.SenderID);

        }
    }
}
