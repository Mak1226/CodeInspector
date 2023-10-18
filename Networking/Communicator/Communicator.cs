using System;
using System.Diagnostics;

namespace Networking.Communicator
{
    public class Communicator : ICommunicator
    {
        void ICommunicator.Send(object obj, string? destID)
        {
            throw new NotImplementedException();
        }

        string ICommunicator.Start(string? destIP, string? destPort)
        {
            throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            throw new NotImplementedException();
        }
    }
}

