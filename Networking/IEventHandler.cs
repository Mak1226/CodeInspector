/////
/////

using System.Net.Sockets;
using Networking.Models;
/// Author: 
namespace Networking
{
    public interface IEventHandler
    {
        public string HandleFile(Message data);
        public string HandleChatMessage(Message data);
        public string HandleAnalyserResult(Message data);
        public string HandleConnectionRequest(Message data);
        public string HandleClientJoined(Message data);
        public string HandleClientLeft(Message data);
        public string HandleClientRegister(Message data, Dictionary<string, NetworkStream> clientIDToStream);
    }
}
