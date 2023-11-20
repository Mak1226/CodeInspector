using Networking.Events;
using Networking.Models;

namespace Content.Model
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
        /// <param name="message"></param>
        /// <returns></returns>
        public string HandleMessageRecv(Message message)
        {
            _server.HandleRecieve(message.Data, message.SenderID);
            return "";
        }
    }
}
