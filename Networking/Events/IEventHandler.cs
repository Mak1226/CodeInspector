/******************************************************************************
 * Filename    = Events/IEventHandler.cs
 *
 * Author      = Shubhang kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the interface with the blueprint of the function called 
 * when a message has been recieved to the subscriber.
 *****************************************************************************/

using Networking.Models;

namespace Networking.Events
{
    /// <summary>
    /// Defines the interface with the blueprint of the function called when a message has been recieved by the subscriber.
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// The function called when a message has been received by the subscriber.
        /// </summary>
        /// <param name="message">The received message</param>
        /// <returns>The result of handling the message</returns>
        public string HandleMessageRecv(Message message);
    }
}
