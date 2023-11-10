using System;
using Networking;
namespace ClientApp
{
	public class Events : IEventHandler
    {
        public string HandleAnalyserResult(string data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(string data)
        {
            Console.WriteLine("Recieved " + data + " in call back function");
            return "";
        }

        public string HandleClientJoined(string data)
        {
            throw new NotImplementedException();
        }

        public string HandleClientLeft(string data)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(string data)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(string data)
        {
            throw new NotImplementedException();
        }
    }
}

