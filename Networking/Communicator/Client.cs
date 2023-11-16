/////
/// Author: 
/////


using System;
using System.Diagnostics;
using System.Net;

using System.Net.Sockets;
using Networking.Events;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        private string _moduleName;
        private Sender _sender;
        private Receiver _receiver;
        private Dictionary<string, NetworkStream> _IDToStream = new();
        Dictionary<string, string> _senderIDToClientID = new();

        private string _senderId;
        private NetworkStream _networkStream;
        private Dictionary<string, IEventHandler> _eventHandlersMap = new();

        private bool _isStarted = false;


        public void Send(string serializedData, string moduleName, string destId)
        {
            if (!_isStarted)
                throw new Exception("Start the client first");

            // NOTE: destID SHOULD be ID.GetServerID() to send to the server.
            Console.WriteLine("[Client] Send" + serializedData + " " + moduleName + " " + destId);
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
        public string Start(string? destIP, int? destPort, string senderId,string moduleName)
        {
            if (_isStarted)
                return "already started";
            _isStarted = true;
            _moduleName = moduleName;
            _senderId = senderId;

            Console.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();

            if (destIP != null && destPort != null)
                tcpClient.Connect(destIP, destPort.Value);


            IPEndPoint localEndPoint = (IPEndPoint)tcpClient.Client.LocalEndPoint;
            Console.WriteLine("[Client] IP Address: " + localEndPoint.Address.MapToIPv4());
            Console.WriteLine("[Client] Port: " + localEndPoint.Port);

            Data data = new Data(_senderId,EventType.ClientRegister());
            Message message = new Message(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetServerID(), _senderId);

            _networkStream = tcpClient.GetStream();
            lock (_IDToStream) { _IDToStream[ID.GetServerID()] = _networkStream; }

            Console.WriteLine("[Client] Starting sender");
            _sender = new(_IDToStream,_senderIDToClientID, true);
            Console.WriteLine("[Client] Starting receiver");
            _receiver = new(_IDToStream, this);
            _sender.Send(message);

            Console.WriteLine("[Client] Started");
            return localEndPoint.Address.MapToIPv4()+":"+localEndPoint.Port;
        }

        public void Stop()
        {
            if (!_isStarted)
                throw new Exception("Start the client first");

            Console.WriteLine("[Client] Stop");
            Data data = new Data(EventType.ClientDeregister());
            _sender.Send(new Message(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetServerID(), _senderId));
            _sender.Stop();
            _receiver.Stop();

            _networkStream.Close();
            Console.WriteLine("[Client] Stopped");
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            if (!_isStarted)
                throw new Exception("Start the client first");

            Console.WriteLine("[Client] Subscribe "+ moduleName);
            //List<IEventHandler> eventHandlers = new();
            //if (_eventHandlersMap.ContainsKey(theEvent))
            //    eventHandlers = _eventHandlersMap[theEvent];
            //eventHandlers.Add(eventHandler);
            //_eventHandlersMap[theEvent] = eventHandlers;
            if (_eventHandlersMap.ContainsKey(moduleName))
                Console.WriteLine("");// already subs
            else
                _eventHandlersMap[moduleName] = eventHandler;

        }

        public void HandleMessage(Message message)
        {
            //foreach (IEventHandler eventHandler in _eventHandlersMap[message.EventType])
            //{
            //    eventHandler.HandleMessageRecv(message);
            //}
            try
            {
                _eventHandlersMap[message.ModuleName].HandleMessageRecv(message);
            }
            catch
            {

            }

        }


    }
}

