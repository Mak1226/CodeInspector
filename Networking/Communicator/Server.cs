/////
/// Author:
/////

using System.Net.Sockets;
using System.Net;
using Networking.Utils;
using Networking.Models;
using Networking.Events;
using System.Diagnostics;
using System;

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

        public string Start(string? destIP, int? destPort, string senderId,string moduleName)
        {
            if (_isStarted)
                return "already started";
            
            _isStarted = true;
            Console.WriteLine("[Server] Start" + destIP + " " + destPort);
            _moduleName=moduleName;
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
                        throw ex;
                    }
                }
            }
            IPEndPoint localEndPoint = (IPEndPoint)_serverListener.LocalEndpoint;
            Console.WriteLine("[Server] Server is listening on:");
            Console.WriteLine("[Server] IP Address: " + GetLocalIPAddress());
            Console.WriteLine("[Server] Port: " + localEndPoint.Port);
            _listenThread = new Thread(AcceptConnection);
            _listenThread.Start();
            //Subscribe(new NetworkingEventHandler(), EventType.ChatMessage());
            //Subscribe(new NetworkingEventHandler(), EventType.NewClientJoined());
            //Subscribe(new NetworkingEventHandler(), EventType.ClientLeft());
            //Subscribe(new NetworkingEventHandler(), EventType.ClientRegister());
            //Subscribe(new NetworkingEventHandler(), EventType.ClientDeregister());
            Subscribe(new NetworkingEventHandler(), ID.GetNetworkingID());
            return GetLocalIPAddress() + ":" + localEndPoint.Port;
        }

        public void Stop()
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Stop");
            _stopThread = true;
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
            Console.WriteLine("[Server] Stopped");
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            if (!_isStarted)
                throw new Exception("Start server first");

            Console.WriteLine("[Server] Subscribe " + moduleName);

            //List<IEventHandler> eventHandlers = new();
            if (_eventHandlersMap.ContainsKey(moduleName))
                Console.WriteLine("");// already subs
            else
                _eventHandlersMap[moduleName] = eventHandler;

                //eventHandlers = _eventHandlersMap[theEvent];
                //eventHandlers.Add(eventHandler);
                //_eventHandlersMap[theEvent] = eventHandlers;
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
                }
                NetworkStream stream = client.GetStream();
                lock (_clientIDToStream) { _clientIDToStream.Add(clientID, stream); }
                clientID += 'A';
                Console.WriteLine("client connected");
            }
        }

        public void HandleMessage(Message message)
        {
            //foreach (IEventHandler eventHandler in _eventHandlersMap[message.EventType])
            //{
            //    eventHandler.HandleMessageRecv(message);
            //}
            //eventHandler.HandleMessageRecv(message);
            _eventHandlersMap[message.ModuleName].HandleMessageRecv(message);

        }
    }
}
