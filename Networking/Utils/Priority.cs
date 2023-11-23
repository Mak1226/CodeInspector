/******************************************************************************
 * Filename    = Utils/Priority.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines the priority levels for message handling based on module names.
 *****************************************************************************/

namespace Networking.Utils
{
    /// <summary>
    /// Defines priority levels for message handling based on module names.
    /// </summary>
    public class Priority
    {
        /// <summary>
        /// Gets the priority level for a given module name.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <returns>The priority level for the module.</returns>
        public static int GetPriority( string moduleName )
        {
            int priority = 10;  // Default priority level

            // Assign priority based on the module name
            if (moduleName == Id.GetNetworkingBroadcastId())
            {
                priority = 0;
            }
            else if (moduleName == Id.GetNetworkingId())
            {
                priority = 1;
            }

            return priority;
        }
    }
}
