/******************************************************************************
 * Filename    = Communicator/CommunicatorFactory.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Factory for creating a communicator.
 *****************************************************************************/


namespace Networking.Communicator
{
    public static class CommunicationFactory
    {
        private static readonly Client s_client = new();
        private static readonly Server s_server = new();

        /// <summary>
        /// Gets the server from the communication factory
        /// </summary>
        /// <returns>The server</returns>
        public static ICommunicator GetServer()
        {
            return s_server;
        }

        /// <summary>
        /// Gets the client from the communication factory
        /// </summary>
        /// <returns>The client</returns>
        public static ICommunicator GetClient()
        {
            return s_client;
        }
    }
}

