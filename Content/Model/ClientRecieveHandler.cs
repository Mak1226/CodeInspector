using Networking.Events;
using Networking.Models;

namespace Content.Model
{
    internal class ClientRecieveHandler : IEventHandler
    {
        private readonly ContentClient _client;
        public ClientRecieveHandler(ContentClient client) 
        {
            _client = client;
        }
        public string HandleMessageRecv(Message message)
        {
            _client.HandleReceive(message.Data);
            return "";
        }
    }
}
