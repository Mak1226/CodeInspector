/////
/// Author:
/////

using System;
using System.Diagnostics;

namespace Networking.Communicator
{
    public static class CommunicationFactory
    {
        private static readonly Client _client = new();
        private static readonly Server _server = new();

        /// <summary>
        /// Communication factory
        /// </summary>
        /// <param name="isServer">True iff server communicator is requested</param>
        /// <returns>The requested communicator</returns>
        public static ICommunicator GetServer()
        {
            return _server;
        }
        public static ICommunicator GetClient()
        {
            return _client;
        }
    }
}

