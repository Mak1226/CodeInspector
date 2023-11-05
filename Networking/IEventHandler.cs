/////
/// Author: 
/////

namespace Networking
{
    public interface IEventHandler
    {
        public string HandleFile(string data);
        public string HandleChatMessage(string data);
        public string HandleAnalyserResult(string data);
        public string HandleConnectionRequest(string data);
        public string HandleClientJoined(string data);
        public string HandleClientLeft(string data);
    }
}
