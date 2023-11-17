using Networking.Communicator;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace Networking.Events
{
    public class NetworkingEventHandler : IEventHandler
    {

        ICommunicator server = CommunicationFactory.GetServer();
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
            return "";
        }

        private string HandleChatMessage(Message message)
        {
            //if (message.DestID != ID.GetServerID())
            //{
            //    ((Server)server).Send(message.Data, message.ModuleName, message.DestID, message.SenderID);
            //}
            //else
            //{
            Console.WriteLine("message received in server:" + message.Data);
            //}
            return "";
        }

        private string HandleClientJoined(Message message)
        {
            //TODO: add respective module name
            Data data = new Data(message.SenderID, EventType.NewClientJoined());
            server.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientLeft(Message message)
        {
            Data data = new Data(message.SenderID, EventType.ClientLeft());
            server.Send(Serializer.Serialize<Data>(data), ID.GetNetworkingID(), ID.GetBroadcastID());
            return "";
        }

        private string HandleClientRegister(Message message)
        {
            lock(((Server)server)._senderIDToClientID)
            {
                Data data=Serializer.Deserialize<Data>(message.Data);
                ((Server)server)._senderIDToClientID[message.SenderID] = data.Payload;
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

