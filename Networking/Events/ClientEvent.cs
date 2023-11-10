using Networking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Events
{
    public class ClientEvent : IEventHandler
    {
        public string HandleAnalyserResult(Message message)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Message message)
        {
            Console.WriteLine("[client] message recVd: " + message.Data);
            return "";
;        }

        public string HandleClientJoined(Message message)
        {
            Console.WriteLine("[client] joined: " + message.Data);
            return "";
        }

        public string HandleClientLeft(Message message)
        {
            throw new NotImplementedException();
        }

        public string HandleClientRegister(Message message, Dictionary<string, NetworkStream> clientIDToStream, Dictionary<string, string> senderIDToClientID)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(Message message)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
