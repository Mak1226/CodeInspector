/******************************************************************************
 * Filename    = Utils/ID.cs
 *
 * Author      = VM Sreeram
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
    public class ID
    {
        /// <summary>
        /// Gets the ID string for the server.
        /// </summary>
        /// <returns>The server ID string.</returns>
        public static string GetServerID()
        {
            return "server";
        }

        /// <summary>
        /// Gets the ID string for broadcast communication.
        /// </summary>
        /// <returns>The broadcast ID string.</returns>
        public static string GetBroadcastID()
        {
            return "broadcast";
        }

        /// <summary>
        /// Gets the ID string for the Networking module.
        /// </summary>
        /// <returns>The Networking module ID string.</returns>
        public static string GetNetworkingID()
        {
            return "networking";
        }

        /// <summary>
        /// Gets the ID string for broadcast communication within the Networking module.
        /// </summary>
        /// <returns>The Networking broadcast ID string.</returns>
        public static string GetNetworkingBroadcastID()
        {
            return "networkingBroadcast";
        }
    }
}
