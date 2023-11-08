using System;
using System.Net.Sockets;
using Networking.Events;
using Networking.Models;

namespace ServerApp
{
    public class Events : IEventHandler
    {
        public string HandleAnalyserResult(Networking.Models.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Networking.Models.Message data)
        {
            Console.WriteLine("Recieved " + data.Data + " in call back function");
            return "";
        }

        public string HandleClientJoined(Networking.Models.Message data)
        {
            Console.WriteLine("new client joinded: " + data.Data);
            return "";
        }

        public string HandleClientLeft(Networking.Models.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(Networking.Models.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Networking.Models.Message data)
        {
            throw new NotImplementedException();
        }

        string IEventHandler.HandleClientRegister(Message message, Dictionary<string, NetworkStream> clientIDToStream)
        {
            throw new NotImplementedException();
        }
    }
}

