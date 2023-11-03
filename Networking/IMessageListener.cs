/******************************************************************************
 * Filename    = IMessageListener.cs
 *
 * Author      = Ramaswamy Krishnan-Chittur
 *
 * Product     = GuiAndDistributedDemo
 * 
 * Project     = Networking
 *
 * Description = Declares the interface for a message listener.
 *****************************************************************************/

namespace Networking
{
    /// <summary>
    /// Declares a listener for messages from the communicator.
    /// </summary>
    public interface IMessageListener
    {
        /// <summary>
        /// Handles reception of a message.
        /// </summary>
        /// <param name="message">Message that is received</param>
        void OnMessageReceived(string message);
    }
}
