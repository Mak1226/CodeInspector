/******************************************************************************
 * Filename    = ClientRecieveHandler.cs
 * 
 * Author      = Jyothiradithya
 *
 * Product     = Analyzer
 * 
 * Project     = Content
 *
 * Description = Network subscriber for handling client recieve
 *****************************************************************************/
using Networking.Events;
using Networking.Models;

namespace Content.Model
{
    /// <summary>
    /// Notifies client on recieve
    /// </summary>
    internal class ClientRecieveHandler : IEventHandler
    {
        private readonly ContentClient _client;

        /// <summary>
        /// Initiate
        /// </summary>
        /// <param name="client">ContentClient to be notified</param>
        public ClientRecieveHandler(ContentClient client) 
        {
            _client = client;
        }

        /// <summary>
        /// Pass recieved message up to the client
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string HandleMessageRecv(Message message)
        {
            _client.HandleReceive(message.Data);
            return "";
        }
    }
}
