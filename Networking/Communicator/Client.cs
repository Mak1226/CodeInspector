/////
/// Author: 
/////

using System;
using System.Diagnostics;

namespace Networking.Communicator
{
    public class Client : ICommunicator
    {
        void ICommunicator.Send(string serializedObj, string eventType, string? destID)
        {
            Trace.WriteLine("[Client] Send" + serializedObj + " " + eventType + " " + destID);
            //throw new NotImplementedException();
        }

        string ICommunicator.Start(string? destIP, string? destPort)
        {
            Trace.WriteLine("[Client] Start" + destIP + " " + destPort);
            return "";
            //throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("[Client] Stop");
            //throw new NotImplementedException();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Client] Subscribe");
            //throw new NotImplementedException();
        }
    }
}

