using Networking.Events;
using Networking.Models;

namespace ClientApp
{
    public class EventChatMessage : IEventHandler
    {
        public string HandleMessageRecv(Message message)
        {
            return HandleChatMessage(message);
        }

        private string HandleChatMessage(Message data)
        {
            Console.WriteLine("[HandleChatMessage, cl] Recieved ChatMessage" + data.Data + " in call back function");
            return "";
        }
    }
}
