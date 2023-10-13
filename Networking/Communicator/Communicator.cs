using System;
using System.Diagnostics;

namespace Networking.Communicator
{
	public class Communicator : ICommunicator
	{
        public string Authenticate(string username, string password)
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }

        public bool Connect(string host, int port)
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }

        public void Send(string serializedData, string? destination)
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }
    }
}

