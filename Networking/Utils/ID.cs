/******************************************************************************
 * Filename    = Utils/Id.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines static methods to retrieve predefined string IDs for various purposes.
 *****************************************************************************/

namespace Networking.Utils
{
    /// <summary>
    /// Defines static methods to retrieve predefined string IDs for various purposes.
    /// </summary>
    public class Id
    {
        /// <summary>
        /// Gets the ID string for the server.
        /// </summary>
        /// <returns>The server ID string.</returns>
        public static string GetServerId()
        {
            return "server";
        }

        /// <summary>
        /// Gets the ID string for broadcast communication.
        /// </summary>
        /// <returns>The broadcast ID string.</returns>
        public static string GetBroadcastId()
        {
            return "broadcast";
        }

        /// <summary>
        /// Gets the ID string for the Networking module.
        /// </summary>
        /// <returns>The Networking module ID string.</returns>
        public static string GetNetworkingId()
        {
            return "networking";
        }

        /// <summary>
        /// Gets the ID string for broadcast communication within the Networking module.
        /// </summary>
        /// <returns>The Networking broadcast ID string.</returns>
        public static string GetNetworkingBroadcastId()
        {
            return "networkingBroadcast";
        }
    }
}
