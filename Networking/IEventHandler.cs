/////
/// Author: 
/////

namespace Networking
{
    public interface IEventHandler
    {
        public string handleFile();
        public string handleChatMessage();
        public string handleAnalyserResult();
        public string connectionRequest();
        public string clientJoined();
        public string clientLeft();
    }
}
