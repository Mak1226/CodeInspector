using Networking.Events;
using Networking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client
{
    internal class ClientRecieveHandler : IEventHandler
    {
        private ContentClient _client;
        public ClientRecieveHandler(ContentClient client) 
        {
            _client = client;
        }
        public string HandleMessageRecv(Message message)
        {
            _client.HandleRecieve(message.Data);
        }
    }
}
