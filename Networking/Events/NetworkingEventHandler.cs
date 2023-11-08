using Networking.Communicator;
using Networking.Models;
using System.Net.Sockets;
using Networking.Utils;

namespace Networking.Events
{
    public class NetworkingEventHandler : IEventHandler
    {
        ICommunicator server = CommunicationFactory.GetServer();
        public string HandleAnalyserResult(Message message)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Message message)
        {
            if (message.DestID != ID.GetServerID())
            {
                ((Server)server).Send(message.Data, message.EventType, message.DestID, message.SenderID);
            }
            return "";
        }

        public string HandleClientJoined(Message message)
        {
            server.Send(message.SenderID, EventType.NewClientJoined(), ID.GetBroadcastID());
            return "";
        }

        public string HandleClientLeft(Message message)
        {
            server.Send(message.Data, EventType.ClientLeft(), ID.GetBroadcastID());
            return "";
        }

        public string HandleConnectionRequest(Message message)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Message message)
        {
            throw new NotImplementedException();
        }
        public string HandleClientRegister(Message message, Dictionary<string, NetworkStream> clientIDToStream)
        {
            lock (clientIDToStream)
            {
                clientIDToStream[message.SenderID] = clientIDToStream[message.Data];
                clientIDToStream.Remove(message.Data);
            }
            HandleClientJoined(message);
            return "";
        }
    }
}
