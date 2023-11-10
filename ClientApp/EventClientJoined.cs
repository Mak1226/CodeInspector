using Networking.Events;
using Networking.Models;

namespace ClientApp
{
    public class EventClientJoined : IEventHandler
    {
        public string HandleMessageRecv(Message message)
        {
            return HandleClientJoined(message);
        }

        private string HandleClientJoined(Message data)
        {
            Console.WriteLine("[HandleClientJoined, cl] new client joinded: " + data.Data);
            return "";
        }
    }
}
