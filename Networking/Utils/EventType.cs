/******************************************************************************
 * Filename    = Utils/EventType.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = EventTypes of Networking Module
 *****************************************************************************/

namespace Networking.Utils
{
    /// <summary>
    /// Defines static methods to retrieve predefined string event types.
    /// </summary>
    public static class EventType
    {
        /// <summary>
        /// Gets the event type string for handling chat messages.
        /// </summary>
        /// <returns>The chat message event type string.</returns>
        public static string ChatMessage()
        {
            return "HandleChatMessage";
        }

        /// <summary>
        /// Gets the event type string for handling new client joined events.
        /// </summary>
        /// <returns>The new client joined event type string.</returns>
        public static string NewClientJoined()
        {
            return "HandleClientJoined";
        }

        /// <summary>
        /// Gets the event type string for handling client left events.
        /// </summary>
        /// <returns>The client left event type string.</returns>
        public static string ClientLeft()
        {
            return "HandleClientLeft";
        }

        /// <summary>
        /// Gets the event type string for handling client registration events.
        /// </summary>
        /// <returns>The client register event type string.</returns>
        public static string ClientRegister()
        {
            return "HandleClientRegister";
        }

        /// <summary>
        /// Gets the event type string for handling client deregistration events.
        /// </summary>
        /// <returns>The client deregister event type string.</returns>
        public static string ClientDeregister()
        {
            return "HandleClientDeregister";
        }

        /// <summary>
        /// Gets the event type string for handling server left events.
        /// </summary>
        /// <returns>The server left event type string.</returns>
        public static string ServerLeft()
        {
            return "HandleServerLeft";
        }
    }
}
