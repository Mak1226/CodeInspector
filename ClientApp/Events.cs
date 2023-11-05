using System;
using Networking;
namespace ClientApp
{
	public class Events : IEventHandler
    {
        public string HandleAnalyserResult(Networking.Utils.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleChatMessage(Networking.Utils.Message data)
        {
            Console.WriteLine("Recieved " + data.SerializedObj + " in call back function");
            return "";
        }

        public string HandleClientJoined(Networking.Utils.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleClientLeft(Networking.Utils.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleConnectionRequest(Networking.Utils.Message data)
        {
            throw new NotImplementedException();
        }

        public string HandleFile(Networking.Utils.Message data)
        {
            throw new NotImplementedException();
        }
    }
}

