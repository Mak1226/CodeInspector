using Networking.Events;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace ClientApp
{
    public class ExampleEventHandler : IEventHandler
    {
        public string HandleMessageRecv(Message message)
        {
            Data data=Serializer.Deserialize<Data>(message.Data);
            if (data.EventType == EventType.NewClientJoined())
            {
                return HandleClientJoined(message);
            }
            if(data.EventType==EventType.ChatMessage())
                return HandleChatMessage(message);
            return "";
        }
        private string HandleClientJoined(Message message)
        {
            Data data = Serializer.Deserialize<Data>(message.Data);
            Console.WriteLine("[HandleClientJoined, cl] new client joinded: " + data.Payload);
            return "";
        }
        private string HandleChatMessage(Message message)
        {
            Data data = Serializer.Deserialize<Data>(message.Data);

            Console.WriteLine("[HandleChatMessage, cl] Recieved ChatMessage" + data.Payload + " in call back function");
            return "";
        }
    }
}
