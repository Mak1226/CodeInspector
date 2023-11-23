using Networking.Communicator;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;
using Networking.Queues;

namespace Networking.Events
{
    public class NetworkingEventHandler : IEventHandler
    {

        readonly ICommunicator _communicator ;

        public NetworkingEventHandler(ICommunicator server) { 
            _communicator = server;
        }
        public string HandleMessageRecv(Message message)
        {
            Data data=Serializer.Deserialize<Data>(message.Data);
            //if (data.EventType == EventType.ChatMessage())
            //{
            //    return HandleChatMessage(message);
            //}
            //if (data.EventType == EventType.NewClientJoined())
            //{
            //    return HandleClientJoined(message);
            //}
            //else if (data.EventType == EventType.ClientLeft())
            //{
            //    return HandleClientLeft(message);
            //}
            if (data.EventType == EventType.ClientRegister())
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

        //private string HandleChatMessage(Message message)
        //{
        //    Console.WriteLine("message received in server:" + message.Data);
        //    return "";
        //}

        private string HandleClientJoined(Message message)
        {
            //TODO: add respective module name
            Data data = new(message.SenderID, EventType.NewClientJoined());
            _communicator.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientLeft(Message message)
        {
            Data data = new (message.SenderID, EventType.ClientLeft());
            _communicator.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingBroadcastID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientRegister(Message message)
        {
            lock(((Server)_communicator)._senderIDToClientID)
            {
                Data data=Serializer.Deserialize<Data>(message.Data);
                ((Server)_communicator)._senderIDToClientID[message.SenderID] = data.Payload;
            }
            HandleClientJoined(message);
            return "";
        }

        private string HandleClientDeregister(Message message)
        {
            Console.WriteLine("herererer");
            string clientID = ((Server)_communicator)._senderIDToClientID[message.SenderID];
            lock (((Server)_communicator)._clientIDToStream)
            {
                ((Server)_communicator)._clientIDToStream.Remove(clientID);
            }
            lock (((Server)_communicator)._senderIDToClientID)
            {
                ((Server)_communicator)._senderIDToClientID.Remove(message.SenderID);
            }
            Console.WriteLine("[server] removed client with: " + clientID + " " + message.SenderID);
            HandleClientLeft(message);
            return "";
        }
        private string HandleServerLeft(Message message)
        {
            string serverID = message.SenderID;
            lock (((Client)_communicator)._IDToStream)
            {
                ((Client)_communicator)._IDToStream.Remove(serverID);
            }
            Console.WriteLine("[client] removed server with: " + serverID + " " + message.SenderID);
            return "";
        }
    }
}

