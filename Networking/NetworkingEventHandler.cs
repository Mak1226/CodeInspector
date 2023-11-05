using Networking.Communicator;
using Networking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    public class NetworkingEventHandler : IEventHandler
    {
        ICommunicator server=CommunicationFactory.GetCommunicator(true);
        public string HandleAnalyserResult(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Message data)
        {
            if (data.DestID != "server")
            {
                server.Send(data.SerializedObj, data.EventType, data.DestID);
            }
            return "";
        }

        public string HandleClientJoined(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleClientLeft(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Message data)
        {
            throw new NotImplementedException();
        }
    }
}
