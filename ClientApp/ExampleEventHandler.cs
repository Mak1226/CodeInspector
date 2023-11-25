using Networking.Events;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace ClientApp
{
    public class ExampleEventHandler : IEventHandler
    {
        readonly HashSet<string> _clients;
        public ExampleEventHandler()
        {
            _clients = new ();
        }
        public ExampleEventHandler(HashSet<string> clients )
        {
            _clients = clients;
        }
        public string HandleMessageRecv(Message message)
        {
            Data data=Serializer.Deserialize<Data>(message.Data);
            if (data.EventType == EventType.NewClientJoined())
            {
                return HandleClientJoined(message);
            }
            if(data.EventType==EventType.ChatMessage())
            {
                return HandleChatMessage(message);
            }

            if (data.EventType == EventType.ServerLeft())
            {
                return HandleServerLeft(message);
            }
            if (data.EventType == EventType.ClientLeft())
            { return HandleClientLeft( message ); }

            return "";
        }
        private string HandleClientJoined(Message message)
        {
            Data data = Serializer.Deserialize<Data>(message.Data);
            Console.WriteLine("[HandleClientJoined, cl] new client joinded: " + data.Payload);
            _clients.Add(data.Payload);
            return "";
        }
        private string HandleServerLeft(Message message)
        {
            Data data = Serializer.Deserialize<Data>(message.Data);
            Console.WriteLine("[HandleServerLeft, cl] server left: " + data.Payload);
            return "";
        }
        private string HandleClientLeft( Message message )
        {
            Data data = Serializer.Deserialize<Data>( message.Data );
            Console.WriteLine( "[HandleServerLeft, cl] server left: " + data.Payload );
            _clients.Remove( data.Payload );
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
