/////
/// Author:
/////

namespace Networking.Communicator
{
    public static class CommunicationFactory
    {
        private static readonly Client _client = new();
        private static readonly Server _server = new();

        /// <summary>
        /// Gets the server from the communication factory
        /// </summary>
        /// <returns>The server</returns>
        public static ICommunicator GetServer()
        {
            return _server;
        }

        /// <summary>
        /// Gets the client from the communication factory
        /// </summary>
        /// <returns>The client</returns>
        public static ICommunicator GetClient()
        {
            return _client;
        }
    }
}

