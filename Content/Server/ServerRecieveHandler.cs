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

        public string HandleMessageRecv(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
