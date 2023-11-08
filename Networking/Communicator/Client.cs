/////
/// Author: 
/////


using System.Net.Sockets;
using Networking.Models;
using Networking.Utils;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        private Sender _sender;
        private Receiver _receiver;
        private Dictionary<string, NetworkStream> _IDToStream = new();
        private string _senderID;
        private NetworkStream _networkStream;
        private Dictionary<string, IEventHandler> _moduleEventMap = new();

        public void Send(string Data, string eventType, string destID)
        {
            // NOTE: destID SHOULD be ID.GetServerID() to send to the server.
            Console.WriteLine("[Client] Send" + Data + " " + eventType + " " + destID);
            Message message = new Message(
                Data, eventType, destID, _senderID
            );
            _sender.Send(message);
        }

        public string Start(string? destIP, int? destPort, string senderID)
        {
            _senderID = senderID;

            Console.WriteLine("[Client] Start" + destIP + " " + destPort);
            TcpClient tcpClient = new();

            if (destIP != null && destPort != null)
                tcpClient.Connect(destIP, destPort.Value);
            Message message = new Message("", EventType.ClientRegister(), ID.GetServerID(), senderID);
            _sender.Send(message);
            _networkStream = tcpClient.GetStream();
            lock (_IDToStream) { _IDToStream[ID.GetServerID()] = _networkStream; }

            Console.WriteLine("[Client] Starting sender");
            _sender = new(_IDToStream, true);
            Console.WriteLine("[Client] Starting receiver");
            _receiver = new(_IDToStream, _moduleEventMap);

            Console.WriteLine("[Client] Started");
            return "";
        }

        public void Stop()
        {
            Console.WriteLine("[Client] Stop");
            _sender.Stop();
            _receiver.Stop();

            _networkStream.Close();
            Console.WriteLine("[Client] Stopped");
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Console.WriteLine("[Client] Subscribe");
            _moduleEventMap.Add(moduleName, eventHandler);
        }

    }
}

