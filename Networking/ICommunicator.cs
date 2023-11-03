/******************************************************************************
 * Filename    = ICommunicator.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = GuiAndDistributedDemo
 * 
 * Project     = Networking
 *
 * Description = Defines the interface for a communicator.
 *****************************************************************************/

namespace Networking
{
    /// <summary>
    /// Declares a communicator that can send and receive messages.
    /// </summary>
    public interface ICommunicator
    {
        /// <summary>
        /// Gets the port that is used for listening.
        /// </summary>
        int ListenPort { get; }

        /// <summary>
        /// Adds a subscriber.
        /// </summary>
        /// <param name="id">Identity of the subscriber to be added</param>
        /// <param name="subscriber">The message listener instance</param>
        void AddSubscriber(string id, IMessageListener subscriber);

        /// <summary>
        /// Removes a subscriber
        /// </summary>
        /// <param name="id">Identity of the subscriber to be removed</param>
        void RemoveSubscriber(string id);

        /// <summary>
        /// Sends the given message to the given ip and port.
        /// </summary>
        /// <param name="ipAddress">IP address of the destination</param>
        /// <param name="port">Port of the destination</param>
        /// <param name="senderId">Identity of the sender</param>
        /// <param name="message">Message to be sent</param>
        void SendMessage(string ipAddress, int port, string senderId, string message);
    }
}
