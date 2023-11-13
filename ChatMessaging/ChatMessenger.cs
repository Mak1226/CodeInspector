/******************************************************************************
 * Filename    = ChatMessenger.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = GuiAndDistributedDemo
 * 
 * Project     = ChatMessaging
 *
 * Description = Defines a chat messenger.
 *****************************************************************************/

using Networking;

namespace ChatMessaging
{
    /// <summary>
    /// Handler for chat messages.
    /// </summary>
    /// <param name="message">received message</param>
    public delegate void ChatMessageReceived(string message);

    /// <summary>
    /// Processor for chat messages.
    /// </summary>
    public class ChatMessenger : IMessageListener
    {
        private readonly ICommunicator _communicator;

        /// <summary>
        /// The identity for this module.
        /// </summary>
        public const string Identity = "ChatMessenger";

        /// <summary>
        /// Event for handling received chat messages.
        /// </summary>
        public event ChatMessageReceived? OnChatMessageReceived;

        /// <summary>
        /// Creates an instance of the chat messenger.
        /// </summary>
        /// <param name="communicator">The communicator instance to use</param>
        public ChatMessenger(ICommunicator communicator)
        {
            _communicator = communicator;
            communicator.AddSubscriber(Identity, this);
        }

        /// <summary>
        /// Sends the given message to the given ip and port.
        /// </summary>
        /// <param name="ipAddress">IP address of the destination</param>
        /// <param name="port">Port of the destination</param>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(string ipAddress, int port, string message)
        {
            _communicator.SendMessage(ipAddress, port, Identity, message);
        }

        /// <inheritdoc />
        public void OnMessageReceived(string message)
        {
            OnChatMessageReceived?.Invoke(message);
        }
    }
}
