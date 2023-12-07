/******************************************************************************
 * Filename    = Events/NetworkingEventHandler.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the event handler for the Networking module.
 *****************************************************************************/

using Logging;
using Networking.Communicator;
using Networking.Models;
using Networking.Serialization;
using Networking.Utils;

namespace Networking.Events
{
    /// <summary>
    /// Defines the event handler for the Networking module.
    /// </summary>
    public class NetworkingEventHandler : IEventHandler
    {
        /// <summary>
        /// The communicator that uses the <see cref="NetworkingEventHandler"/>
        /// </summary>
        readonly ICommunicator _communicator ;

        /// <summary>
        /// The constructor for the Networking module's event handler.
        /// </summary>
        /// <param name="communicator">The communicator that is subscribing to the <see cref="NetworkingEventHandler"/></param>
        public NetworkingEventHandler(ICommunicator communicator) { 
            _communicator = communicator;
        }

        /// <summary>
        /// Handles the message received by the subscriber.
        /// </summary>
        /// <param name="message">The received message</param>
        /// <returns>An empty string</returns>
        public string HandleMessageRecv(Message message)
        {
            Data data=Serializer.Deserialize<Data>(message.Data);
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

        /// <summary>
        /// Broadcasts that a new client has been joined
        /// </summary>
        /// <param name="message">TODO</param>
        /// <returns>A null string</returns>
        private string HandleClientJoined(Message message)
        {
            Data data = new(message.SenderId, EventType.NewClientJoined());
            _communicator.Send(Serializer.Serialize( data ), Id.GetNetworkingBroadcastId(), Id.GetBroadcastId());
            return "";
        }

        /// <summary>
        /// Broadcasts that a new client has been joined
        /// </summary>
        /// <param name="message">TODO</param>
        /// <returns>A null string</returns>
        private string HandleClientLeft(Message message)
        {
            Data data = new (message.SenderId, EventType.ClientLeft());
            _communicator.Send(Serializer.Serialize( data), Id.GetNetworkingBroadcastId(), Id.GetBroadcastId());
            return "";
        }

        /// <summary>
        /// Broadcasts that a client has left
        /// </summary>
        /// <param name="message">TODO</param>
        /// <returns>A null string</returns>
        private string HandleClientRegister(Message message)
        {
            lock(((Server)_communicator)._senderIdToClientId)
            {
                Data data=Serializer.Deserialize<Data>(message.Data);
                ((Server)_communicator)._senderIdToClientId[message.SenderId] = data.Payload;
            }
            HandleClientJoined(message);
            return "";
        }

        /// <summary>
        /// Removed the entry in the dictionaries corresponding to the client who left
        /// </summary>
        /// <param name="message">TODO</param>
        /// <returns>A null string</returns>
        private string HandleClientDeregister(Message message)
        {

            string clientId = ((Server)_communicator)._senderIdToClientId[message.SenderId];
            lock (((Server)_communicator)._clientIdToStream)
            {
                ((Server)_communicator)._clientIdToStream.Remove(clientId);
            }
            lock (((Server)_communicator)._senderIdToClientId)
            {
                ((Server)_communicator)._senderIdToClientId.Remove(message.SenderId);
            }
            Logger.Log( "[server] Removed client with: " + clientId + " " + message.SenderId ,LogLevel.INFO);
            HandleClientLeft(message);
            return "";
        }

        /// <summary>
        /// Removes the networkstream to the server from the client
        /// </summary>
        /// <param name="message">TODO</param>
        /// <returns>A null string</returns>
        private string HandleServerLeft(Message message)
        {
            string serverId = message.SenderId;
            lock (((Client)_communicator)._IdToStream)
            {
                ((Client)_communicator)._IdToStream.Remove(serverId);
            }
            Logger.Log("[client] Removed server with: " + serverId + " " + message.SenderId , LogLevel.INFO );
            return "";
        }
    }
}

