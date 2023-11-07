using Networking;
using Networking.Utils;

namespace Content.Server
{
    internal class ServerRecieveHandler : IEventHandler
    {
        ContentServer _server;

        public ServerRecieveHandler(ContentServer server)
        {
            _server = server;
        }
        public string HandleAnalyserResult(Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Message data)
        {
            throw new NotImplementedException();
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
            _server.HandleRecieve(data.SerializedObj, data.SenderID);
            return "";
        }
    }
}
