using Networking.Events;
using Networking.Models;
using System.Net.Sockets;

namespace Content.Server
{
    /// <summary>
    /// Notifier for message recieved by server
    /// </summary>
    internal class ServerRecieveHandler : IEventHandler
    {
        ContentServer _server;

        /// <summary>
        /// Notifies content server only on recieving an encoded file list
        /// </summary>
        /// <param name="server">ContentServer to be notified</param>
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

        public string HandleClientRegister(Message message, Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, string> senderIDToClientID)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(Message data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recieved file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string HandleFile(Message data)
        {
            _server.HandleRecieve(data.Data, data.SenderID);
            return "";
        }
    }
}
