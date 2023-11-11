using Networking.Communicator;
using Networking.Models;
using Networking.Utils;

namespace Networking.Events
{
    public class NetworkingEventHandler : IEventHandler
    {

        ICommunicator server = CommunicationFactory.GetServer();
        public string HandleMessageRecv(Message message)
        {
            if (message.EventType == EventType.ChatMessage())
            {
                return HandleChatMessage(message);
            }
            else if (message.EventType == EventType.NewClientJoined())
            {
                return HandleClientJoined(message);
            }
            else if (message.EventType == EventType.ClientLeft())
            {
                return HandleClientLeft(message);
            }
            else if (message.EventType == EventType.ClientRegister())
            {
                return HandleClientRegister(message);
            }
            else if (message.EventType == EventType.ClientDeregister())
            {
                return HandleClientDeregister(message);
            }
            return "";
        }

        private string HandleChatMessage(Message message)
        {
            if (message.DestID != ID.GetServerID())
            {
                ((Server)server).Send(message.Data, message.EventType, message.DestID, message.SenderID);
            }
            else
            {
                Console.WriteLine("message received in server:" + message.Data);
            }
            return "";
        }

        private string HandleClientJoined(Message message)
        {
            server.Send(message.SenderID, EventType.NewClientJoined(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientLeft(Message message)
        {
            server.Send(message.SenderID, EventType.ClientLeft(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientRegister(Message message)
        {
            lock(((Server)server)._senderIDToClientID)
            {
                ((Server)server)._senderIDToClientID[message.SenderID] = message.Data;
            }
            HandleClientJoined(message);
            return "";
        }

        private string HandleClientDeregister(Message message)
        {
            Console.WriteLine("herererer");
            string clientID = ((Server)server)._senderIDToClientID[message.SenderID];
            lock (((Server)server)._clientIDToStream)
            {
                ((Server)server)._clientIDToStream.Remove(clientID);
            }
            Console.WriteLine("[server] removed client with: " + clientID + " " + message.SenderID);
            HandleClientLeft(message);
            return "";
        }
    }
}

