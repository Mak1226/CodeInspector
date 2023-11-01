/////
/// Author:
/////

using System;
using System.Diagnostics;
using System.Text.Json; 
using System.Net.Sockets;

namespace Networking.Communicator
{
    public class Message
    {
        public string _serializedObj { get; }
        public string _eventType { get; }
        public Message(string serializedObj, string eventType)
        {
            _serializedObj = serializedObj;
            _serializedObj = eventType;
        }
    }
    public class Server : ICommunicator
    {
        Dictionary<string, NetworkStream>? _clientIDToStream;
        private byte[] returnBytes(string serializedObj, string eventType)
        {
            var data = new Message(serializedObj, eventType);
            string serStr = JsonSerializer.Serialize(data);
            return System.Text.Encoding.ASCII.GetBytes(serStr);
        }
        void ICommunicator.Send(string serializedObj, string eventType, string? destID)
        {
            Trace.WriteLine("[Server] Send" + serializedObj + " " + eventType + " " + destID);
            if(destID!=null)
            {
                byte[] message = returnBytes(serializedObj, eventType);
                _clientIDToStream[destID].Write(message);
            }
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
