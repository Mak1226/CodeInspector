using Networking.Communicator;
using Networking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class NetworkingEventHandler : IEventHandler
    {
        ICommunicator server = CommunicationFactory.GetCommunicator(true);
        public string HandleAnalyserResult(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Message data)
        {
            if (data.DestID != "server")
            {
                ((Server)server).Send(data.SerializedObj, data.EventType, data.DestID, data.SenderID);
            }
            return "";
        }

        public string HandleClientJoined(Message data)
        {
            server.Send(data.SenderID, EventType.NewClientJoined(), "broadcast");
            return "";
        }

        public string HandleClientLeft(Message data)
        {
            server.Send(data.SerializedObj, EventType.ClientLeft(), "broadcast");
            return "";
        }

        public string HandleConnectionRequest(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Message data)
        {
            throw new NotImplementedException();
        }
        public string HandleClientRegister(Message data, Dictionary<string, NetworkStream> clientIDToStream)
        {
            lock (clientIDToStream)
            {
                clientIDToStream[data.SenderID] = clientIDToStream[data.SerializedObj];
                clientIDToStream.Remove(data.SerializedObj);
            }
            this.HandleClientJoined(data);
            return "";
        }
    }
}
