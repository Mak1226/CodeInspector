/////
/// Author:
/////

using System;
using System.Diagnostics;
using Networking.Communicator;

namespace Networking
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
		public static ICommunicator GetCommunicator(bool isServer)
		{
            Trace.WriteLine("[CommFact] GetCommunicator");
			if (isServer)
            {
                Trace.WriteLine("[CommFact] Server");
                return _server;
			}
			else
			{
                Trace.WriteLine("[CommFact] Client");
                return _client;
			}
		}
	}
}

