/////
/// Author:
/////

using System;
using System.Diagnostics;

namespace Networking.Communicator
{
    public class Server : ICommunicator
    {
        void ICommunicator.Send(string serializedObj, string eventType, string? destID)
        {
            Trace.WriteLine("[Server] Send" + serializedObj + " " + eventType + " " + destID);
            //throw new NotImplementedException();
        }

        string ICommunicator.Start(string? destIP, string? destPort)
        {
            Trace.WriteLine("[Server] Start" + destIP + " " + destPort);
            return "";
            //throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("[Server] Stop");
            //throw new NotImplementedException();
        }

        public void Subscribe(IEventHandler eventHandler, string moduleName)
        {
            Trace.WriteLine("[Server] Subscribe");
            //throw new NotImplementedException();
        }
    }
}
