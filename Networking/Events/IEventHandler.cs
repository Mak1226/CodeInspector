/////
/////

using System.Net.Sockets;
using Networking.Models;
/// Author: 
namespace Networking.Events
{
    public interface IEventHandler
    {
        public string HandleFile(Message message);
        public string HandleChatMessage(Message message);
        public string HandleAnalyserResult(Message message);
        public string HandleConnectionRequest(Message message);
        public string HandleClientJoined(Message message);
        public string HandleClientLeft(Message message);
        public string HandleClientRegister(Message message, Dictionary<string, NetworkStream> clientIDToStream);
    }
}
