using System;
using System.Diagnostics;

namespace Networking.Communicator
{
    public class Communicator : ICommunicator
    {
        void ICommunicator.Send(string serializedObj, string eventType, string? destID)
        {
            Trace.WriteLine("Send" + serializedObj + " " + eventType + " " + destID);
            //throw new NotImplementedException();
        }

        string ICommunicator.Start(string? destIP, string? destPort)
        {
            Trace.WriteLine("Start" + destIP + " " + destPort);
            return "";
            //throw new NotImplementedException();
        }

        void ICommunicator.Stop()
        {
            Trace.WriteLine("Stop");
            //throw new NotImplementedException();
        }
    }
}

