using Networking.Communicator;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace Networking.Events
{
    public class NetworkingEventHandler : IEventHandler
    {

        ICommunicator communicator ;
        public NetworkingEventHandler(ICommunicator server) { 
            this.communicator = server;
        }
        public string HandleMessageRecv(Message message)
        {
            Data data=Serializer.Deserialize<Data>(message.Data);
            if (data.EventType == EventType.ChatMessage())
            {
                return HandleChatMessage(message);
            }
            else if (data.EventType == EventType.NewClientJoined())
            {
                return HandleClientJoined(message);
            }
            else if (data.EventType == EventType.ClientLeft())
            {
                return HandleClientLeft(message);
            }
            else if (data.EventType == EventType.ClientRegister())
            {
                return HandleClientRegister(message);
            }
            else if (data.EventType == EventType.ClientDeregister())
            {
                return HandleClientDeregister(message);
            }
            else if (data.EventType == EventType.ServerLeft())
            {
                return HandleServerLeft(message);
            }
            return "";
        }

        private string HandleChatMessage(Message message)
        {
            Console.WriteLine("message received in server:" + message.Data);
            return "";
        }

        private string HandleClientJoined(Message message)
        {
            //TODO: add respective module name
            Data data = new Data(message.SenderID, EventType.NewClientJoined());
            communicator.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientLeft(Message message)
        {
            Data data = new Data(message.SenderID, EventType.ClientLeft());
            communicator.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientRegister(Message message)
        {
            lock(((Server)communicator)._senderIDToClientID)
            {
                Data data=Serializer.Deserialize<Data>(message.Data);
                ((Server)communicator)._senderIDToClientID[message.SenderID] = data.Payload;
            }
            HandleClientJoined(message);
            return "";
        }

        private string HandleClientDeregister(Message message)
        {
            Console.WriteLine("herererer");
            string clientID = ((Server)communicator)._senderIDToClientID[message.SenderID];
            lock (((Server)communicator)._clientIDToStream)
            {
                ((Server)communicator)._clientIDToStream.Remove(clientID);
            }
            Console.WriteLine("[server] removed client with: " + clientID + " " + message.SenderID);
            HandleClientLeft(message);
            return "";
        }
        private string HandleServerLeft(Message message)
        {
            string serverID = message.SenderID;
            lock (((Client)communicator)._IDToStream)
            {
                ((Client)communicator)._IDToStream.Remove(serverID);
            }
            Console.WriteLine("[client] removed server with: " + serverID + " " + message.SenderID);
            return "";
        }
    }
}

